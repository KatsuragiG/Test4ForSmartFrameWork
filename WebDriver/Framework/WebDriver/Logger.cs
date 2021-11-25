using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebdriverFramework.Framework.Util;

namespace WebdriverFramework.Framework.WebDriver
{
    /// <summary>
    /// class for log
    /// </summary>
    public class Logger
    {
        private static readonly ThreadLocal<Logger> _instance = new ThreadLocal<Logger>();
        private readonly ILog _log;
        /// <summary>
        /// delimeter betweeen inline log messages
        /// </summary>
        public static readonly string LogDelimiter = "::";
        private int _lastExecutedStep;
        private string _preconditionMessage = "";
        private const string TemplatePreconditionMessage = " in precondition id {0}";
        private string _testName;
        private static readonly string TEST_NAME_START = "<(";
        private static readonly string TEST_NAME_END = ")>";
        private static readonly string SCREENSHOT = "<!({0}";
        private readonly string patternForLogTime = "MM/dd/yyyy hh:mm:ss.fff tt";
        /// <summary>
        /// constructor
        /// </summary>
        private Logger()
        {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger("fileApp");
        }

        /// <summary>
        /// instance log
        /// </summary>
        public static Logger Instance => _instance.Value ?? (_instance.Value = new Logger());

        /// <summary>
        /// Store last executed step description
        /// </summary>
        public string LastExecutedStepMessage { get; set; } = "";

        /// <summary>
        /// fail in log
        /// </summary>
        /// <param name="message">message for log</param>
        public void Fail(string message)
        {
            _log.Fatal($"{_testName}{DateTime.Now.ToString(patternForLogTime)} - {message}");
            var errors = ErrorsCollector.GetBrowserErrors(Browser.Instance.GetDriver());
            Assert.Fail(errors.Count != 0
                    ? $"{_testName}{message}. Browser errors are:{errors.Aggregate("", (current, error) => current + error)}"
                    : $"{_testName}{message}. ");
        }

        /// <summary>
        /// info in log
        /// </summary>
        /// <param name="message">message for log</param>
        public void Info(string message)
        {
            _log.Info($"{_testName}{DateTime.Now.ToString(patternForLogTime)} - {message}");
        }

        /// <summary>
        /// Info in log Without TimeStamp
        /// </summary>
        /// <param name="message"></param>
        public void InfoWithoutTimeStamp(string message)
        {
            _log.Info($"{_testName}{message}");
        }

        /// <summary>
        /// precondition for start
        /// </summary>
        /// <param name="title">name of the step</param>
        public void PreconditionStart(String title)
        {
            SetPreconditionMessage();
            string formattedName = $"===== CommandLineExecutor: '{title}' =====";
            string delims = "";
            int nChars = formattedName.Length;
            for (int i = 0; i < nChars; i++)
            {
                delims += "+";
            }
            Info(delims);
            Info(formattedName);
            Info(delims);
        }

        /// <summary>
        /// precondition for failed
        /// </summary>
        /// <param name="title">name of the step</param>
        public void PreconditionFailed(String title)
        {
            
            string formattedName = $"===== CommandLineExecutor Failed: '{title}' =====";
            string delims = "";
            int nChars = formattedName.Length;
            for (int i = 0; i < nChars; i++)
            {
                delims += "#";
            }
            Info(delims);
            Info(formattedName);
            Info(delims);
        }

        /// <summary>
        /// precondition for passed
        /// </summary>
        /// <param name="title">name of the step</param>
        public void PreconditionPassed(String title)
        {
            string formattedName = $"===== CommandLineExecutor Passed: '{title}' =====";
            string delims = "";
            int nChars = formattedName.Length;
            for (int i = 0; i < nChars; i++)
            {
                delims += "+";
            }
            Info(delims);
            Info(formattedName);
            Info(delims);
        }
        /// <summary>
        /// Report in log abount screenshot
        /// </summary>
        /// <param name="path"></param>
        public void ReportScreenshot(string path)
        {
            InfoWithoutTimeStamp(string.Format(SCREENSHOT, path));
        }

        /// <summary>
        /// name of the test
        /// </summary>
        /// <param name="title">name of the test</param>
        public void TestName(String title)
        {
            if (_testName == null)
            {
                _testName = MakeNameLikeInAllure(title, null);
            }
            string formattedName = $"========== Test Case: '{title}' ==========";
            string delims = "";
            int nChars = formattedName.Length;
            for (int i = 0; i < nChars; i++)
            {
                delims += "-";
            }
            Info(delims);
            Info(formattedName.Replace(TEST_NAME_START, "").Replace(TEST_NAME_END, "").Trim());
            Info(delims);
        }
        /// <summary>
        /// name of the test with row number
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rowNumber"></param>
        public void TestName(String title, int? rowNumber)
        {
            _testName = MakeNameLikeInAllure(title, rowNumber);
            TestName(_testName);
        }

        private string MakeNameLikeInAllure(string title, int? row)
        {
            title = title.Replace("_", string.Empty);
            var match = Regex.Matches(title, "[A-Z][a-z]|[0-9]{1,}");
            title = match.Cast<Group>().Aggregate(title, (current, matchGroup) => current.Replace(matchGroup.Value, $" {matchGroup.Value.ToLower()}"));
            var match2 = Regex.Matches(title, "[a-z][A-Z][ ]");
            foreach (Group group in match2)
            {
                var match3 = Regex.Match(group.Value,"[A-Z]");
                title = title.Replace(match3.Value, $" {match3.Value}");
            }
            return $"<({title} {row + 1} )>";

        }
        /// <summary>
        /// Log about managedThreadId
        /// </summary>
        public void LogTreadId()
        {
            var managedThreadId = Thread.CurrentThread.ManagedThreadId;
            Instance.Warn($"class is executed in '{managedThreadId}' managedThreadId");
        }
        /// <summary>
        /// simple log range of steps
        /// </summary>
        /// <param name="step">step</param>
        /// <param name="toStep">toStep</param>
        public void LogStep(int step, int toStep)
        {
            StoreLastStepInfo(step);
            Info(
                $"[Steps {step} - {toStep}]");
        }

        /// <summary>
        /// simple log step
        /// </summary>
        /// <param name="step"></param>
        public void LogStep(int step)
        {
            StoreLastStepInfo(step);
            Info(
                $"[ Step {step} ]");
        }

        /// <summary>
        /// log step with action message from MTM
        /// </summary>
        /// <param name="step">step</param>
        /// <param name="message">message</param>
        public void LogStep(int step, string message)
        {
            StoreLastStepInfo(step);
            Info(
                $"[ Step {step} ]: {message}");
        }

        private void StoreLastStepInfo(int step)
        {
            _lastExecutedStep = step;
            LastExecutedStepMessage = $" [step #'{_lastExecutedStep}']";
        }

        /// <summary>
        /// log step with action message from MTM
        /// </summary>
        /// <param name="step">step</param>
        /// <param name="toStep">toStep</param>
        /// <param name="message">message</param>
        public void LogStep(int step, int toStep, string message)
        {
            LogStep(step, toStep);
            Info(message);
            Info("----------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// log if test is passed
        /// </summary>
        /// <param name="title">name test</param>
        public void Passed(String title)
        {
            string formattedName = $"******** Test Case: '{title}' Passed! ********";
            string delims = "";
            int nChars = formattedName.Length;
            for (int i = 0; i < nChars; i++)
            {
                delims += "*";
            }
            Info(delims);
            Info(formattedName);
            Info(delims);
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="format">format</param>
        public void Debug(string format)
        {
           _log.Debug($"{_testName}{DateTime.Now.ToString(patternForLogTime)} - {format}");  
        }

        /// <summary>
        /// warn
        /// </summary>
        /// <param name="formatLogMsg">format for message</param>
        public void Warn(string formatLogMsg)
        {
            _log.Warn($"{_testName}{DateTime.Now.ToString(patternForLogTime)} - {formatLogMsg}");
        }
        
        /// <summary>
        /// error
        /// </summary>
        /// <param name="formatLogMsg">format for message</param>
        public void Error(string formatLogMsg)
        {
            _log.Error($"{_testName}{DateTime.Now.ToString(patternForLogTime)} - {formatLogMsg}");
        }

        /// <summary>
        /// get TC id
        /// </summary>
        /// <param name="intput">intput</param>
        /// <returns>intput</returns>
        public static string GetTcId(string intput)
        {
            string step = null;
            try
            {
                Match match = Regex.Match(intput, @"TC_(\d+)", RegexOptions.IgnoreCase); //
                if (match.Success)
                {
                    step = match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                intput += ex;
            }
            return step ?? intput;
        }

        /// <summary>
        /// Set precondition message
        /// </summary>
        public void SetPreconditionMessage()
        {
            _preconditionMessage = string.Format(TemplatePreconditionMessage, GetTcId((String)_deepCounter.Peek()));
        }

        private readonly Stack _deepCounter = new Stack();
        /// <summary>
        /// Dispose log stack
        /// </summary>
        public static void Dispose()
        {
            _instance.Value = null;
        }
    }
}
