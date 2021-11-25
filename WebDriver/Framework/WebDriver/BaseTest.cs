using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.Extensions;
using WebdriverFramework.Framework.Util;


namespace WebdriverFramework.Framework.WebDriver
{
    /// <summary>
    /// Base test class. Does browser initialization and closes it after test is finished.
    /// </summary>
    [TestClass]
    public abstract class BaseTest : BaseEntity
    {
        /// <summary>
        /// allow marks test as failure or success
        /// if HasWarn has true value test will be failed in the end
        /// this variable used in the Checker class for make soft assertions
        /// </summary>
        public bool HasWarn;

        private readonly Regex regexURL = new Regex(@"(http|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?");

        /// <summary>
        /// Checker to make soft assert
        /// </summary>
        protected Checker Checker;

        /// <summary>
        /// Context of the current test execution
        /// </summary>
        public virtual TestContext TestContext { get; set; }

        /// <summary>
        /// override method toString()
        /// </summary>
        /// <returns>name</returns>
        public override string ToString()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Initialization before test case.
        /// </summary>
        [TestInitialize]
        public virtual void InitBeforeTest()
        {
            if (TestContext.DataRow == null)
            {
                Log.TestName(GetType().Name, null);
            }
            else
            {
                Log.TestName(GetType().Name, TestContext.DataRow.Table.Rows.IndexOf(TestContext.DataRow));
            }
            Browser.InitDriver();
            Checker = new Checker(Log, Browser);
            HasWarn = false;
            Browser.NavigateToStartPage();
            try
            {
                Browser.WindowMaximise();
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Contains("Session not started or terminated"))
                    // Sometimes browserstack terminates connection unexpectedly. Just workaroud below
                {
                    var type = typeof(Browser);
                    type.GetField("_currentInstance", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, null);
                    type.GetField("_currentDriver", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, null);
                    type.GetField("_firstInstance", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, null);
                    InitBeforeTest();
                }
            }
        }

        /// <summary>
        /// should be implemented in childs
        /// </summary>
        public abstract void RunTest();

        /// <summary>
        /// получает список некритичных сообщений и выводит в лог
        /// </summary>
        protected void ProcessingErrors()
        {
            if (!Checker.Messages.Any())
            {
                return;
            }

            var allMessages = "";
            Info("====================== See autotest errors below ===============");
            for (var i = 0; i < Checker.Messages.Count; i++)
            {
                allMessages += Checker.Messages[i];
                Error("Error " + (i + 1) + " : " + Checker.Messages[i]);
            }
            Info("====================== end of errors ======================");
            Assert.Fail(allMessages);
        }

        /// <summary>
        /// closing browser
        /// </summary>
        [TestCleanup]
        public virtual void CleanAfterTest()
        {
            var methodCustomAttributes = GetType().GetMethod("RunTest").GetCustomAttribute(typeof(DescriptionAttribute));
            var descriptionUrl = string.Empty;
            if (methodCustomAttributes != null)
            {
                descriptionUrl = regexURL.Match(((DescriptionAttribute)methodCustomAttributes).Description).Value;
            }
            if (descriptionUrl.Equals(string.Empty))
            {
                descriptionUrl = "Link for the Test Case is absent because Description is not added to test or does not contain link";
            }
            Info($"Link for the Test Case: {descriptionUrl}");

            try
            {
                ProcessingErrors();
            }
            catch (Exception e)
            {
                Log.Fail(
                    "Test was finished but there are some issues with result analyzing. Please check that all right configured\r\n" +
                    "Exception:" + e.Message + $"Link for the Test Case: {descriptionUrl}");
            }
            finally
            {
                SqlConnection.CloseConnectons();
                Browser.Quit();
                Logger.Dispose();
                Checker.Messages = null;
            }
        }

        /// <summary>
        /// formats the value for logging "name test - log splitter - the message"
        /// </summary>
        /// <param name="message">message for format</param>
        /// <returns>a formatted value for logging "element type - name - log splitter - the message"</returns>
        protected override string FormatLogMsg(string message)
        {
            return message;
        }

        /// <summary>
        /// zoom body for percent
        /// </summary>
        /// <param name="percent">percent for zoom</param>
        public void ChangeZoomAndLogStep(int percent)
        {
            Logger.Instance.Info($"Zoom out to: {percent}");
            Browser.GetDriver().ExecuteJavaScript($"document.body.style.zoom='{percent}%'");
            Browser.WaitForPageToLoad();
        }
    }
}
