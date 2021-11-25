using System;
using System.Threading.Tasks;

namespace TradeStops.Common.Helpers
{
    // consider to convert into ActionExtensions with extension methods if there will more usages
    public static class DelegateHelper
    {
        /// <summary>
        /// Wrap an Action (method that returns void) in method that returns 'true' to be able to pass this Action as Func.
        /// This is usually done to avoid code duplication for Func and Action.
        /// </summary>
        /// <param name="code">Code to execute.</param>
        /// <returns>Stub result.</returns>
        public static Func<bool> ToFunc(Action code)
        {
            return () =>
            {
                code();

                return true;
            };
        }

        /// <summary>
        /// Wrap a method that returns Task to a internal function to execute with Func.
        /// </summary>
        /// <param name="code">Code to execute.</param>
        /// <returns>Stub result.</returns>
        public static Func<Task<bool>> ToFunc(Func<Task> code)
        {
            return async () =>
            {
                await code();

                return true;
            };
        }
    }
}
