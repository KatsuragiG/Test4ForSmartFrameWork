using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradeStops.Common.Helpers
{
    /// <summary>
    /// Provides functionality to automatically try the given piece of logic some number of times before re-throwing the exception. 
    /// This is useful for any piece of code which may experience transient failures.
    /// </summary>
    // https://github.com/Microsoft/RetryOperationHelper/blob/master/RetryOperationHelper/RetryOperationHelper.cs
    public class RetryHelper
    {
        private readonly int _maxNumberOfAttempts;
        private readonly int _secondsToSleepBeforeRetry;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxNumberOfAttempts">The maximum number of attempts.</param>
        /// <param name="secondsToSleepBeforeRetry">Retry interval in seconds to wait between attempts of the execution</param>
        public RetryHelper(int maxNumberOfAttempts, int secondsToSleepBeforeRetry)
        {
            _maxNumberOfAttempts = maxNumberOfAttempts;
            _secondsToSleepBeforeRetry = secondsToSleepBeforeRetry;
        }

        /// <summary>
        /// Execute synchronous method that returns void with synchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <param name="code">Synchronous code to execute that returns void.</param>
        /// <param name="failedAttemptHandler">Synchronous callback that will be executed when an attempt is failed. Usually _logger.Warn().</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        public void Execute(Action code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            ExecuteInternal(DelegateHelper.ToFunc(code), failedAttemptHandler, exceptionFilter);
        }

        /// <summary>
        /// Execute synchronous method that returns object with synchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <typeparam name="TResult">Return object type.</typeparam>
        /// <param name="code">Synchronous code to execute that returns object.</param>
        /// <param name="failedAttemptHandler">Synchronous callback that will be executed when an attempt is failed. Usually _logger.Warn().</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        /// <returns>Return result.</returns>
        public TResult Execute<TResult>(Func<TResult> code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            return ExecuteInternal(code, failedAttemptHandler, exceptionFilter);
        }

        /// <summary>
        /// Execute asynchronous method, that returns Task, with asynchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <param name="code">Asynchronous code to execute, that returns Task.</param>
        /// <param name="failedAttemptHandlerAsync">Asynchronous callback that will be executed when an attempt is failed.</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        /// <returns>Task to wait.</returns>
        public async Task ExecuteAsync(Func<Task> code, Func<Exception, int, Task> failedAttemptHandlerAsync, Func<Exception, bool> exceptionFilter)
        {
            await ExecuteInternalAsync(DelegateHelper.ToFunc(code), failedAttemptHandlerAsync, exceptionFilter);
        }

        /// <summary>
        /// Execute asynchronous method, that returns object wrapped in Task, with asynchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <typeparam name="TResult">Return object type.</typeparam>
        /// <param name="code">Asynchronous code to execute, that returns Task.</param>
        /// <param name="failedAttemptHandlerAsync">Asynchronous callback that will be executed when an attempt is failed.</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        /// <returns>Object wrapped in Task to wait.</returns>
        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> code, Func<Exception, int, Task> failedAttemptHandlerAsync, Func<Exception, bool> exceptionFilter)
        {
            return await ExecuteInternalAsync(code, failedAttemptHandlerAsync, exceptionFilter);
        }

        /// <summary>
        /// Execute asynchronous method, that returns Task, with synchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <param name="code">Asynchronous code to execute, that returns Task.</param>
        /// <param name="failedAttemptHandler">Synchronous callback that will be executed when an attempt is failed. Usually _logger.Warn().</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        /// <returns>Task to wait.</returns>
        public async Task ExecuteAsync(Func<Task> code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            await ExecuteInternalAsync(DelegateHelper.ToFunc(code), failedAttemptHandler, exceptionFilter);
        }

        /// <summary>
        /// Execute asynchronous method, that returns object wrapped in Task, with synchronous failed attempt handler and synchronous exception filter.
        /// </summary>
        /// <typeparam name="TResult">Return object type.</typeparam>
        /// <param name="code">Asynchronous code to execute that returns Task.</param>
        /// <param name="failedAttemptHandler">Synchronous callback that will be executed when an attempt is failed. Usually _logger.Warn().</param>
        /// <param name="exceptionFilter">Synchronous method to determine when exception must be caught. Example: (ex) => ex is SqlException.</param>
        /// <returns>Object wrapped in Task to wait.</returns>
        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            return await ExecuteInternalAsync(code, failedAttemptHandler, exceptionFilter);
        }

        private TResult ExecuteInternal<TResult>(Func<TResult> code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            for (var attempt = 1; attempt < _maxNumberOfAttempts; attempt++)
            {
                try
                {
                    return code();
                }
                catch (Exception ex) when (exceptionFilter(ex))
                {
                    failedAttemptHandler(ex, attempt);

                    Thread.Sleep(_secondsToSleepBeforeRetry * 1000);
                }
            }

            return code();
        }

        private async Task<TResult> ExecuteInternalAsync<TResult>(Func<Task<TResult>> code, Func<Exception, int, Task> failedAttemptHandlerAsync, Func<Exception, bool> exceptionFilter)
        {
            for (var attempt = 1; attempt < _maxNumberOfAttempts; attempt++)
            {
                try
                {
                    return await code();
                }
                catch (Exception ex) when (exceptionFilter(ex))
                {
                    await failedAttemptHandlerAsync(ex, attempt);

                    await Task.Delay(_secondsToSleepBeforeRetry * 1000);
                }
            }

            return await code();
        }

        private async Task<TResult> ExecuteInternalAsync<TResult>(Func<Task<TResult>> code, Action<Exception, int> failedAttemptHandler, Func<Exception, bool> exceptionFilter)
        {
            for (var attempt = 1; attempt < _maxNumberOfAttempts; attempt++)
            {
                try
                {
                    return await code();
                }
                catch (Exception ex) when (exceptionFilter(ex))
                {
                    failedAttemptHandler(ex, attempt);

                    await Task.Delay(_secondsToSleepBeforeRetry * 1000);
                }
            }

            return await code();
        }
    }
}
