using System;
using System.Collections.Generic;
using System.Linq;
using WebdriverFramework.Framework.WebDriver;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// class provides methods for soft assertions
    /// sometimes we need verify some condition and proceed test in case if verification was failed
    /// this method allow make this
    /// after as the test was ended and has failed soft assertions test still be marked as failed with messages of the reasons
    /// class doesn't support running in several threads
    /// </summary>
    public class Checker
    {
        private readonly Random Random;
        /// <summary>
        /// messages
        /// </summary>
        public List<string> Messages;
        private readonly Logger Logger;
        private readonly Browser Browser;
        /// <summary>
        /// checker
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="Browser">browser</param>
        public Checker(Logger logger, Browser Browser)
        {
            this.Logger = logger;
            Random = new Random(int.MaxValue);
            Messages = new List<string>();
            this.Browser = Browser;
        }

        /// <summary>
        /// verify that two object are equal
        /// </summary>
        /// <param name="message">message if objects don't equal</param>
        /// <param name="actual">actual object</param>
        /// <param name="expected">expected object</param>
        public bool CheckEquals(Object expected, Object actual, string message)
        {
            var condition = true;
            if (actual != null)
            {
                condition = actual.Equals(expected);
            }
            else if (expected != null)
            {
                condition = expected.Equals(null);
            }
            else
            {
                Logger.Info($"Both argument are null, string message: {message}");
            }

            if (condition)
            {
                Logger.Info($"Assertion passed - Expected: [{expected}]. Actual: [{actual}]");
            }
            else
            {
                SetErrorInfo(message + "." + "\r\nObject Actual not equals object Expected", actual, expected);
            }

            return condition;
        }

        /// <summary>
        /// verify that object is true
        /// </summary>
        /// <param name="message">message if objects don't equal true</param>
        /// <param name="actual">actual object</param>
        public bool IsTrue(Object actual, string message)
        {
            if (!actual.Equals(true))
            {
                SetErrorInfo(message + "." + "\r\nObject Actual not equals object Expected", actual, true);
                return false;
            }
            Logger.Info($"Assertion passed - Expected: [{true}]. Actual: [{actual}]");
            return true;
        }

        /// <summary>
        ///  verify that two object are not equal
        /// </summary>
        /// <param name="message">message if objects equal</param>
        /// <param name="actual">actual object</param>
        /// <param name="expected">xpected object</param>
        /// <returns></returns>
        public bool CheckNotEquals(Object expected, Object actual, string message)
        {
            if (actual.Equals(expected))
            {
                SetErrorInfoNotEquals(message + "." + "\r\nObject Actual equals object Expected", actual, expected);
                return false;
            }
            Logger.Info($"Assertion passed - Not expected: [{expected}]. Actual: [{actual}]");
            return true;
        }

        /// <summary>
        /// verify equality of two List
        /// </summary>
        /// <param name="message">message if objects equal</param>
        /// <param name="actList">actual list</param>
        /// <param name="expList">expected list</param>
        /// <returns></returns>
        public bool CheckListsEquals<T>(List<T> expList, List<T> actList, string message)
        {
            var actualForLog = string.Join("\n\t", actList.Select(e => e.ToString()));
            var expectedForLog = string.Join("\n\t", expList.Select(e => e.ToString()));
            if (!BaseObjectComparator.AreListsEquals(actList, expList))
            {
                SetErrorInfo(message + "." + $"\r\nList Actual[{actList.Count}] not equals List Expected[{expList.Count}]", actualForLog, expectedForLog);
                return false;
            }
            Logger.Info($"Assertion passed - Expected:\n {expectedForLog}\nActual:\n {actualForLog}");
            return true;
        }

        /// <summary>
        /// verify equality of two Dictionary
        /// </summary>
        /// <param name="message">message if objects equal</param>
        /// <param name="actDict">actual dictionary</param>
        /// <param name="expDict">expected dictionary</param>
        /// <returns></returns>
        public bool CheckDictionariesEquals<TKey, TValue>(Dictionary<TKey, TValue> expDict, Dictionary<TKey, TValue> actDict, string message)
        {
            var actualForLog = string.Join("\n\t", actDict.Select(e => $"{e.Key}: {e.Value}"));
            var expectedForLog = string.Join("\n\t", expDict.Select(e => $"{e.Key}: {e.Value}"));
            if (!BaseObjectComparator.AreDictionariesEquals(actDict, expDict))
            {
                SetErrorInfo(message + "." + $"\r\nDictionary Actual[{actDict.Count}] not equals Dictionary Expected[{expDict.Count}]", actualForLog, expectedForLog);
                return false;
            }
            Logger.Info($"Assertion passed - Expected:\n {expectedForLog}\nActual:\n {actualForLog}");
            return true;
        }

        /// <summary>
        /// verify inequality of two lists
        /// </summary>
        /// <param name="message">message if objects not equal</param>
        /// <param name="actList">actual list</param>
        /// <param name="expList">expected list</param>
        /// <returns></returns>
        public bool CheckListsNotEquals<T>(List<T> expList, List<T> actList, string message)
        {
            var actualForLog = string.Join("\n\t", actList.Select(e => e.ToString()));
            var expectedForLog = string.Join("\n\t", expList.Select(e => e.ToString()));
            if (BaseObjectComparator.AreListsEquals(actList, expList))
            {
                SetErrorInfoNotEquals(message + "." + $"\r\nList Actual[{actList.Count}] equals List Expected[{expList.Count}]", actualForLog, expectedForLog);
                return false;
            }
            Logger.Info($"Assertion passed - Not expected:\n {expectedForLog}\nActual:\n {actualForLog}");
            return true;
        }

        /// <summary>
        ///  verify that object is false
        /// </summary>
        /// <param name="message">message if objects equal</param>
        /// <param name="actual">actual object</param>
        /// <returns></returns>
        public bool IsFalse(Object actual, string message)
        {
            if (!actual.Equals(false))
            {
                SetErrorInfoNotEquals(message + "." + "\r\nObject Actual equals object Expected", actual, false);
                return false;
            }
            Logger.Info($"Assertion passed - Not expected: [{false}]. Actual: [{actual}]");
            return true;
        }

        /// <summary>
        /// Fail check
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Fail(string message)
        {
            SetErrorInfo(message + "." + "\r\nObject Actual not equals object Expected", false, true);
        }

        /// <summary>
        /// verify that actual string contains another(expected)
        /// </summary>
        /// <param name="message">message if verification was failed</param>
        /// <param name="actual">actual string</param>
        /// <param name="expected">expected string</param>
        public bool CheckContains(string expected, string actual, string message)
        {
            if (BaseForm.TitleForm != null) 
            {
                message = $"{BaseForm.TitleForm} :: {message}";
            }
            if (!actual.Contains(expected))
            {
                SetErrorInfo($"{message}.", actual, expected);
                return false;
            }
            Logger.Info(message);
            return true;
        }

        /// <summary>
        /// Marks test as failed with changing static HasWarn variable of the BaseTest to true
        /// also make screenshot and post log message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="actual">actual object</param>
        /// <param name="expected">expected object</param>
        private void SetErrorInfo(string message, Object actual, Object expected)
        {
            Messages.Add(
                $"Assertion failed:\r\n Condition: {message} \r\n Actual: [{actual}] but Expected: [{expected}]\r\n");
            var fileName = "Error_Screen_" + Random.Next();
            Browser.SaveScreenShot(fileName);
        }

        /// <summary>
        /// Marks test as failed with changing static HasWarn variable of the BaseTest to true
        /// also make screenshot and post log message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="actual">actual object</param>
        /// <param name="expected">expected object</param>
        private void SetErrorInfoNotEquals(string message, Object actual, Object expected)
        {
            Messages.Add(
                $"Assertion failed:\r\n Condition: {message} \r\n Actual: [{actual}] but not Expected: [{expected}]\r\n");
            var fileName = "Error_Screen_" + Random;
            Browser.SaveScreenShot(fileName);
        }

        /// <summary>
        /// Throws
        /// </summary>
        /// <param name="message"></param>
        /// <param name="asserMethods"></param>
        /// <typeparam name="T"></typeparam>
        public void Throws<T>(string message, params Action[] asserMethods) where T : Exception
        {
            try
            {
                foreach (var asserMethod in asserMethods)
                {
                    asserMethod();
                }
                Fail(message);
            }
            catch (T)
            {
                Logger.Info($"{message} Assert passed");
            }

        }

        /// <summary>
        /// Not Throws
        /// </summary>
        /// <param name="message"></param>
        /// <param name="asserMethods"></param>
        /// <typeparam name="T"></typeparam>
        public void NotThrows<T>(string message, params Action[] asserMethods) where T : Exception
        {
            try
            {
                foreach (var asserMethod in asserMethods)
                {
                    asserMethod();
                }
                Logger.Info($"{message} Assert passed");
            }
            catch (T)
            {
                Fail(message);
            }
        }
    }
}