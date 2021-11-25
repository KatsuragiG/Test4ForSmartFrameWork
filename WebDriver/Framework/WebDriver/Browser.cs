using System;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace WebdriverFramework.Framework.WebDriver
{
    /// <summary>
    /// class for setting parameters of the browser
    /// </summary>
    public class Browser
    {
        private static ThreadLocal<Browser> _currentInstance = new ThreadLocal<Browser>();
        private static ThreadLocal<RemoteWebDriver> _currentDriver = new ThreadLocal<RemoteWebDriver>();
        /// <summary>
        /// number of screenshots
        /// </summary>
        public static int _counter = 1;
        private static object syncLock = new object();

        /// <summary>
        /// stores value of the current Browser type
        /// </summary>
        public static BrowserFactory.BrowserType CurrentBrowser;

        private string _propBrowser;
        /// <summary>
        /// stores value of the implicity wait
        /// </summary>
        public double ImplWait;
        private double _timeoutForPageToLoad;
        /// <summary>
        /// stores value of the time when autotest will be wait until element exists
        /// </summary>
        public double TimeoutForElementWaiting;
       /// <summary>
       /// stores value to the base application url
       /// very often it is login page
       /// </summary>
        public static string LoginPage;

        /// <summary>
        /// directory to store any artifacts of tests
        /// for example: screenshots of the steps
        /// </summary>
        public static readonly string ActiveDir = "..\\artifacts";

        /// <summary>
        /// singleton
        /// </summary>
        public static Browser Instance => _currentInstance.Value ?? (_currentInstance.Value = new Browser());

        /// <summary>
        /// constructor
        /// </summary>
        private Browser()
        {
            InitProperties();
        }

        /// <summary>
        /// Init
        /// </summary>
        public void InitDriver()
        {
            if (_currentDriver.Value != null)
            {
                return;
            }

            Logger.Instance.Info($"Start of '{_propBrowser}' browser instance construction");
            _currentDriver.Value = BrowserFactory.SetUp(_propBrowser, out CurrentBrowser);
            _currentDriver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplWait);
            Logger.Instance.Info($"End of '{CurrentBrowser}' browser instance construction");
        }

        /// <summary>
        /// read params
        /// </summary>
        private void InitProperties()
        {
            //read params
            LoginPage = Configuration.LoginUrl;
            ImplWait = 0;
            _timeoutForPageToLoad = Convert.ToDouble(Configuration.PageTimeout);
            TimeoutForElementWaiting = Convert.ToDouble(Configuration.ElementTimeout);
            _propBrowser = Configuration.Browser;
        }

        /// <summary>
        /// returns default timeout for wait elements
        /// migth be used for other waitings
        /// </summary>
        /// <returns>timeout</returns>
        public static TimeSpan GetElementTimeoutInSeconds()
        {
            return GetElementTimeoutInSeconds(1);
        }

        /// <summary>
        /// returns default timeout for wait elements
        /// migth be used for other waitings
        /// </summary>
        /// <returns>timeout</returns>
        public static TimeSpan GetElementTimeoutInSeconds(int multyplyerForTimeout)
        {
            return TimeSpan.FromSeconds(multyplyerForTimeout * Convert.ToDouble(Configuration.ElementTimeout));
        }

        /// <summary>
        /// start page navigation
        /// </summary>
        public void NavigateToStartPage()
        {
            _currentDriver.Value.Navigate().GoToUrl(LoginPage);
            WaitForPageToLoad();
        }

        /// <summary>
        /// wait for page to load
        /// </summary>
        public void WaitForPageToLoad()
        {
            var wait = new WebDriverWait(_currentDriver.Value, TimeSpan.FromSeconds(Convert.ToDouble(_timeoutForPageToLoad)));
            try
            {
                wait.Until(waiting =>
                {
                    var result =
                        ((IJavaScriptExecutor) _currentDriver.Value).ExecuteScript(
                            "return document['readyState'] ? 'complete' == document.readyState : true");
                    return result is bool && (Boolean)result;
                });
            }
            catch (Exception e)
            {
                Logger.Instance.Debug(e.StackTrace);
            }
        }

        /// <summary>
        /// waits for alert
        /// <returns>alert</returns>
        /// </summary>
        private IAlert WaitForAlert()
        {
            var wait = new WebDriverWait(_currentDriver.Value, GetElementTimeoutInSeconds());
            try
            {
                IAlert alert = wait.Until(waiting =>
                {
                    try
                    {
                        IAlert expectedAlert = _currentDriver.Value.SwitchTo().Alert();
                        return expectedAlert;
                    }
                    catch (NoAlertPresentException)
                    {
                        return null;
                    }
                });
                return alert;
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("No alert was shown in " + GetElementTimeoutInSeconds());
            }
        }

        /// <summary>
        /// wait until alert is present and the click Accept button
        /// <returns>text from alert</returns>
        /// </summary>
        public string WaitAlertAndAccept()
        {
            var alert = WaitForAlert();
            string text = alert.Text;
            Logger.Instance.Info("Text in the Alert: " + text);
            alert.Accept();
            WaitForPageToLoad();
            return text;
        }

        /// <summary>
        /// Wait before spenner is not shown
        /// </summary>
        public void WaitForSpinnerAppears()
        {
            var wait = new WebDriverWait(_currentDriver.Value, TimeSpan.FromSeconds(5));
            // waiting before spinner appears
            try
            {
                wait.Until(waiting =>
                {
                    var spinnerDisplayed = _currentDriver.Value.FindElement(By.XPath("//img[contains(@src,'spinner.gif')]")).Displayed;
                    return spinnerDisplayed;
                });
            }
            catch (Exception e)
            {
                Logger.Instance.Debug(e.StackTrace);
            }
        }


        /// <summary>
        /// waits until page url will be changed
        /// </summary>
        /// <param name="oldUrl">old value of page url</param>
        public void WaitForUrlChange(string oldUrl)
        {
            var wait = new WebDriverWait(_currentDriver.Value, TimeSpan.FromSeconds(5));
            // waiting before spinner appears
            try
            {
                wait.Until(waiting =>
                {
                    var cuurentUrl = _currentDriver.Value.Url;
                    return !oldUrl.Equals(cuurentUrl);
                });
            }
            catch (Exception e)
            {
                Logger.Instance.Debug(e.StackTrace);
            }
        }

        /// <summary>
        /// waits until url will contains some value
        /// </summary>
        /// <param name="value">part of url</param>
        public void WaitForUrlContains(string value)
        {
            var wait = new WebDriverWait(_currentDriver.Value, TimeSpan.FromSeconds(5));
            try
            {
                wait.Until(waiting =>
                {
                    var cuurentUrl = _currentDriver.Value.Url;
                    return !cuurentUrl.Contains(value);
                });
            }
            catch (Exception e)
            {
                Logger.Instance.Debug(e.StackTrace);
            }
        }

        /// <summary>
        /// refreshes page
        /// </summary>
        public void Refresh()
        {
            try
            {
                GetDriver().Navigate().Refresh();
            }
            catch (Exception e)
            {
                Logger.Instance.Debug("Error while page was refreshed.\r\n Exception: " + e.Message);
            }
        }

        /// <summary>
        /// get driver
        /// </summary>
        /// <returns>current driver</returns>
        public RemoteWebDriver GetDriver()
        {
            InitDriver();
            return _currentDriver.Value;
        }

        /// <summary>
        /// switch frame by index
        /// </summary>
        /// <param name="index">index</param>
        public void SwitchFrameByIndex(int index)
        {
            _currentDriver.Value.SwitchTo().Frame(index);
            Logger.Instance.Info($"Switching to frame '{index}' ");
        }

        /// <summary>
        /// switch to last window
        /// </summary>
        public void SwitchToLastWindow()
        {
            var availableWindows = _currentDriver.Value.WindowHandles;
            if (availableWindows.Count > 1)
            {
                _currentDriver.Value.SwitchTo().Window(availableWindows[availableWindows.Count - 1]);
            }

        }

        /// <summary>
        /// switch to first window
        /// </summary>
        public void SwitchToFirstWindow()
        {
            _currentDriver.Value.SwitchTo().Window(_currentDriver.Value.WindowHandles[0]);
        }

        /// <summary>
        /// quit
        /// </summary>
        public void Quit()
        {
            if (_currentDriver.Value != null && _currentDriver.IsValueCreated)
            {
                var name = "";
                lock (syncLock)
                {
                    name = GetType().Name + _counter++;
                }
                try
                {
                    Logger.Instance.ReportScreenshot(SaveAndGetPathToScreenShot(name));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Info($"Error of '{CurrentBrowser}' at getting exit screenshot: '{ex}'");
                }
                
                Logger.Instance.Info($"Start of '{CurrentBrowser}' browser instance destroying");
                _currentDriver.Value.Quit();
                _currentDriver.Value = null;
                _currentInstance.Value = null;
                Logger.Instance.Info($"Instance of Browser '{CurrentBrowser}' was destroyed");
            }
        }

        /// <summary>
        /// get screenshot
        /// </summary>
        /// <returns>screenshot</returns>
        public Screenshot GetScreenshot()
        {
            var screenShot = ((ITakesScreenshot) _currentDriver.Value).GetScreenshot();
            return screenShot;
        }

        /// <summary>
        /// fullscreen
        /// now this works with the standart function for all possible browsers: IE, FireFox and Chrome
        /// </summary>
        public void WindowMaximise()
        {
            if (!Configuration.MobileTesting || Configuration.Browser.ToLower() != "browserstack")
            {
                _currentDriver.Value.Manage().Window.Maximize();
            }
        }

        /// <summary>
        /// sleep process
        /// </summary>
        /// <param name="mSeconds">the number of seconds for sleep</param>
        public static void Sleep(int mSeconds)
        {
            Thread.Sleep(mSeconds);
        }

        /// <summary>
        /// sleep process
        /// </summary>
        /// <param name="mSeconds">the number of milliseconds for sleep</param>
        public void Sleep(double mSeconds)
        {
            Thread.Sleep((int)mSeconds);
        }

        /// <summary>
        /// sleep process
        /// </summary>
        public void Sleep()
        {
            Thread.Sleep(3000);
        }

        /// <summary>
        /// save screenshot
        /// </summary>
        /// <param name="fileName">name screenshot</param>
        public void SaveScreenShot(string fileName)
        {
            try
            {
                SaveAndGetPathToScreenShot(fileName);
            }
            catch (Exception e)
            {
                Logger.Instance.Warn("Can't save screenshot, trying one more time. Reason: " + e.Message);
                SaveAndGetPathToScreenShot(fileName);
            }
        }

        /// <summary>
        /// save screenshot
        /// </summary>
        /// <param name="fileName">name screenshot</param>
        public string SaveAndGetPathToScreenShot(string fileName)
        {
            var screenshot = GetScreenshot();
            //Create a new subfolder under the current active folder
            var newPath = Path.Combine(Directory.GetCurrentDirectory(), ActiveDir);
            // Create the subfolder
            Directory.CreateDirectory(newPath);
            newPath = Path.Combine(newPath, fileName + ".jpg");
            screenshot.SaveAsFile(newPath, ScreenshotImageFormat.Jpeg);

            Logger.Instance.InfoWithoutTimeStamp($"Screenshot was saved '{newPath}'");
            return newPath;
        }
        
        /// <summary>
        /// Navigate to page with URL "url"
        /// </summary>
        /// <param name="url">Page URL</param>
        public void NavigateTo(string url)
        {
            Logger.Instance.Info("Navigate to url:" + url);
            try
            {
                _currentDriver.Value.Navigate().GoToUrl(url);
            }
            catch (WebDriverException e)
            {
                Logger.Instance.Info($"page with url '{url}' causes error: '{e}'");
                Refresh();
                _currentDriver.Value.Navigate().GoToUrl(url);
            }
        }

        /// <summary>
        /// waiting, while number of open windows will be more than previous
        /// </summary>
        /// <param name="prevWndCount">number of windows before some action</param>
        public void WaitForNewWindow(int prevWndCount)
        {
            int wndCount = prevWndCount;
            var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(TimeoutForElementWaiting));
            wait.Until(d => d.WindowHandles.Count > wndCount);
        }

        /// <summary>
        /// Get number of open windows
        /// </summary>
        /// <returns>number of open windows</returns>
        public int WindowCount()
        {
            return GetDriver().WindowHandles.Count;
        }

        /// <summary>
        /// Switch to the window with index
        /// </summary>
        /// <param name="index">index of the window</param>
        public void SwitchWindow(int index)
        {
            GetDriver().SwitchTo().Window(GetDriver().WindowHandles.ToArray()[index]);
        }

        /// <summary>
        /// get scripts from Network browser tub
        /// </summary>
        public object GetNetworks()
        {
            String scriptToExecute = "var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}; " +
                "var network = performance.getEntries() || {}; return network;";
            return ((IJavaScriptExecutor)GetDriver()).ExecuteScript(scriptToExecute);  
        }
    }
}
