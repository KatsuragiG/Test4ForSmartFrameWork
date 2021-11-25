using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace WebdriverFramework.Framework.WebDriver
{
    /// <summary>
    /// Factory to initialize browser instance
    /// </summary>
    public class BrowserFactory
    {
        /// <summary>
        /// list of available browsers for execution
        /// </summary>
        public enum BrowserType
        {
            /// <summary>
            /// Chrome browser (version depends on machine when tests will be runned)
            /// </summary>
            Chrome,
            /// <summary>
            /// Firefox browser (version depends on machine when tests will be runned)
            /// </summary>
            Firefox,
            /// <summary>
            /// IEexplore browser (version depends on machine when tests will be runned)
            /// </summary>
            Iexplore,
            /// <summary>
            /// BrowserStack configuration. Should be defined in the app.config.
            /// </summary>
            BrowserStack,
            /// <summary>
            /// Chrome Mobile Emulator in chromedriver
            /// </summary>
            ChromeMobileEmulator,
            /// <summary>
            /// Edge browser (version depends on machine when tests will be runned)
            /// </summary>
            Edge
        }

        /// <summary>
        /// creates a driver of the browser
        /// if found creates a remote system variable 'server'
        /// </summary>
        /// <param name="type">type of browser</param>
        /// <returns>RemoteWebDriver</returns>
        public static RemoteWebDriver GetDriver(BrowserType type)
        {
            RemoteWebDriver driver;
            var downloadDirectory = Configuration.DownloadDirectory;
            try
            {
                Directory.CreateDirectory(downloadDirectory);
            }
            catch (Exception e)
            {
                Logger.Instance.Info($"File directory was not created {e}");
            }
            switch (type)
            {
                case BrowserType.Chrome:
                    PrepareChromeOptions(downloadDirectory, out var chromeOptions, out var chromeDriverService);
                    driver = BuildChromeDriverWithSeveralAttempts(chromeOptions, chromeDriverService);
                    break;
                case BrowserType.Firefox:
                    var firefoxOptions = PrepareFireFoxOptions(downloadDirectory);
                    driver = BuildFireFoxDriver(firefoxOptions);
                    break;
                case BrowserType.Iexplore:
                    var optionsIe = PrepareIeOptions();
                    driver = BuildIeDriver(optionsIe);
                    break;
                case BrowserType.Edge:
                    var optionsEdge = new EdgeOptions
                    {
                        PageLoadStrategy = (PageLoadStrategy)EdgePageLoadStrategy.Default,
                        UnhandledPromptBehavior = UnhandledPromptBehavior.AcceptAndNotify,
                    };
                    driver = BuildEdgeDriver(optionsEdge);
                    break;
                case BrowserType.BrowserStack:
                    driver = BuildBrowserStackDriver();
                    break;
                case BrowserType.ChromeMobileEmulator:
                    driver = BuildChromeMobileEmulatorDriver();
                    break;
                default:
                    throw new Exception("Browser was not specified. Please specify browser in the app.config or pass it from environment settings as variable with name browser");
            }

            if (Configuration.UseLocalFileDetector)
            {
                driver.FileDetector = new LocalFileDetector();
            }
            return driver;
        }

        private static EdgeDriverService CreateAndStartEdgeService()
        {
            return EdgeDriverService.CreateDefaultService("C:\\Windows\\SysWOW64\\", "msedgedriver.exe", 52296);
        }

        private static RemoteWebDriver BuildEdgeDriver(EdgeOptions optionsEdge)
        {
            var driver = Configuration.UseSeleniumGrid 
                ? new RemoteWebDriver(new Uri(Configuration.SeleniumGridUrl), optionsEdge.ToCapabilities(), TimeSpan.FromSeconds(Configuration.RemoteWebDriverWait)) 
                : new EdgeDriver(CreateAndStartEdgeService(), optionsEdge, TimeSpan.FromSeconds(Configuration.LocalWebDriverWait));

            return driver;
        }

        private static RemoteWebDriver BuildIeDriver(InternetExplorerOptions optionsIe)
        {
            var driver = Configuration.UseSeleniumGrid 
                ? new RemoteWebDriver(new Uri(Configuration.SeleniumGridUrl), optionsIe.ToCapabilities(), TimeSpan.FromSeconds(Configuration.RemoteWebDriverWait)) 
                : new InternetExplorerDriver(optionsIe);

            return driver;
        }

        private static RemoteWebDriver BuildFireFoxDriver(FirefoxOptions firefoxOptions)
        {
            RemoteWebDriver driver = null;
            int attempt = 0;

            while (++attempt <= Configuration.HowManyTimesTryToInstanceBrowser && driver == null)
            {
                try
                {
                    driver = Configuration.UseSeleniumGrid 
                        ? new RemoteWebDriver(new Uri(Configuration.SeleniumGridUrl), firefoxOptions.ToCapabilities(), TimeSpan.FromSeconds(Configuration.RemoteWebDriverWait)) 
                        : new FirefoxDriver(firefoxOptions);
                }
                catch (Exception e)
                {
                    Logger.Instance.Info($"the instance of Firefox driver was not created. The errors are {e.StackTrace}");
                }
            }

            if (driver == null)
            {
                Logger.Instance.Fail("Firefox driver was not created with several retries");
            }

            return driver;
        }

        private static RemoteWebDriver BuildChromeDriverWithSeveralAttempts(ChromeOptions chromeOptions, ChromeDriverService chromeDriverService)
        {
            RemoteWebDriver driver = null;
            int attempt = 0;

            while (++attempt <= Configuration.HowManyTimesTryToInstanceBrowser && driver == null)
            {
                try
                {
                    driver = Configuration.UseSeleniumGrid 
                        ? new RemoteWebDriver(new Uri(Configuration.SeleniumGridUrl), chromeOptions.ToCapabilities(), 
                            TimeSpan.FromSeconds(Configuration.RemoteWebDriverWait)) 
                        : new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromSeconds(Configuration.LocalWebDriverWait));
                }
                catch (Exception e)
                {
                    Logger.Instance.Info($"the instance of chrome driver was not created. The errors are {e.StackTrace}");
                }
            }

            if (driver == null)
            {
                Logger.Instance.Fail("Chrome driver was not created with several retries");
            }

            return driver;
        }

        private static RemoteWebDriver BuildBrowserStackDriver()
        {
            RemoteWebDriver driver;
            var desiredCap = new RemoteSessionSettings();
            desiredCap.AddMetadataSetting("browserstack.user", Configuration.BrowserStack.User);
            desiredCap.AddMetadataSetting("browserstack.key", Configuration.BrowserStack.Key);
            desiredCap.AddMetadataSetting("browserstack.local", Configuration.BrowserStack.Local);
            desiredCap.AddMetadataSetting("browserstack.debug", Configuration.BrowserStack.Debug);
            desiredCap.AddMetadataSetting("acceptSslCerts", "true");
            desiredCap.AddMetadataSetting("unexpectedAlertBehaviour", "accept");
            Logger.Instance.Info("==== Browserstack capabilities ====");
            Logger.Instance.Info("browserstack.user:" + Configuration.BrowserStack.User);
            Logger.Instance.Info("browserstack.key:" + Configuration.BrowserStack.Key);
            Logger.Instance.Info("browserstack.local:" + Configuration.BrowserStack.Local);
            Logger.Instance.Info("browserstack.debug:" + Configuration.BrowserStack.Debug);
            if (Configuration.MobileTesting)
            {
                try
                {
                    desiredCap.AddMetadataSetting("platform", Configuration.BrowserStack.Platform);
                    desiredCap.AddMetadataSetting("browserName", Configuration.BrowserStack.BrowserName);
                    desiredCap.AddMetadataSetting("device", Configuration.BrowserStack.Device);
                    desiredCap.AddMetadataSetting("deviceOrientation", Configuration.BrowserStack.DeviceOrientation);
                    desiredCap.AddMetadataSetting("emulator", Configuration.BrowserStack.Emulator);
                    Logger.Instance.Info("browserstack.platform:" + Configuration.BrowserStack.Platform);
                    Logger.Instance.Info("browserstack.browserName:" + Configuration.BrowserStack.BrowserName);
                    Logger.Instance.Info("browserstack.device:" + Configuration.BrowserStack.Device);
                    Logger.Instance.Info("browserstack.deviceOrientation:" + Configuration.BrowserStack.DeviceOrientation);
                    Logger.Instance.Info("browserstack.emulator:" + Configuration.BrowserStack.Emulator);
                    driver = new RemoteWebDriver(new Uri(Configuration.BrowserStack.Hub), desiredCap);
                }
                catch (Exception e)
                {
                    Logger.Instance.Info("Failed to start BrowserStack's instance. \r\n" + e.Message + "\r\n" +
                                         "Sleep for " + Configuration.PageTimeout + "s, then'll try again");
                    Thread.Sleep(TimeSpan.FromSeconds(double.Parse(Configuration.PageTimeout)));
                    desiredCap.AddMetadataSetting("platform", Configuration.BrowserStack.Platform);
                    desiredCap.AddMetadataSetting("browserName", Configuration.BrowserStack.BrowserName);
                    desiredCap.AddMetadataSetting("device", Configuration.BrowserStack.Device);
                    desiredCap.AddMetadataSetting("deviceOrientation", Configuration.BrowserStack.DeviceOrientation);
                    desiredCap.AddMetadataSetting("emulator", Configuration.BrowserStack.Emulator);
                    driver = new RemoteWebDriver(new Uri(Configuration.BrowserStack.Hub), desiredCap);
                }
            }
            else
            {
                desiredCap.AddMetadataSetting("browser", Configuration.BrowserStack.BrowserType);
                desiredCap.AddMetadataSetting("browser_version", Configuration.BrowserStack.BrowserVersion);
                desiredCap.AddMetadataSetting("os", Configuration.BrowserStack.OS);
                desiredCap.AddMetadataSetting("os_version", Configuration.BrowserStack.OSVersion);
                desiredCap.AddMetadataSetting("resolution", Configuration.BrowserStack.Resolution);
                Logger.Instance.Info("browserstack.browser:" + Configuration.BrowserStack.BrowserType);
                Logger.Instance.Info("browserstack.browser_version:" + Configuration.BrowserStack.BrowserVersion);
                Logger.Instance.Info("browserstack.os:" + Configuration.BrowserStack.OS);
                Logger.Instance.Info("browserstack.os_version:" + Configuration.BrowserStack.OSVersion);
                Logger.Instance.Info("browserstack.resolution:" + Configuration.BrowserStack.Resolution);
                driver = new RemoteWebDriver(new Uri(Configuration.BrowserStack.Hub), desiredCap, TimeSpan.FromSeconds(90));
            }
            Logger.Instance.Info("==== ========================= ====");
            return driver;
        }

        private static RemoteWebDriver BuildChromeMobileEmulatorDriver()
        {
            var mobileEmulation = new Dictionary<string, string>
                    {
                        {"deviceName", Configuration.ChromeMobileEmulator.DeviceName}
                    };
            var chromeCapabilities = new ChromeOptions();
            chromeCapabilities.AddAdditionalCapability("mobileEmulation", mobileEmulation);
            RemoteWebDriver driver = new ChromeDriver(chromeCapabilities);
            return driver;
        }

        private static InternetExplorerOptions PrepareIeOptions()
        {
            var optionsIe = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                EnsureCleanSession = true,
                EnableNativeEvents = true,
                InitialBrowserUrl = "about:blank",
                BrowserAttachTimeout = TimeSpan.FromMinutes(3),
                RequireWindowFocus = false,
                PageLoadStrategy = PageLoadStrategy.Normal,
                IgnoreZoomLevel = true,
                BrowserCommandLineArguments = "-private"
            };
            optionsIe.AddAdditionalCapability(CapabilityType.HandlesAlerts, true);
            optionsIe.AddAdditionalCapability(CapabilityType.IsJavaScriptEnabled, true);
            optionsIe.AddAdditionalCapability(CapabilityType.TakesScreenshot, true);
            return optionsIe;
        }

        private static FirefoxOptions PrepareFireFoxOptions(string downloadDirectory)
        {
            var firefoxOptions = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            //Set Firefox profile for downloading files
            firefoxOptions.SetPreference("browser.helperApps.alwaysAsk.force", false);
            firefoxOptions.SetPreference("browser.download.folderList", 2);
            firefoxOptions.SetPreference("browser.download.useDownloadDir", true);
            firefoxOptions.SetPreference("browser.download.dir", downloadDirectory);
            firefoxOptions.SetPreference("services.sync.prefs.sync.browser.download.manager.showWhenStarting", false);
            firefoxOptions.SetPreference("services.sync.prefs.sync.browser.download.dir", downloadDirectory);
            firefoxOptions.SetPreference("browser.download.manager.showWhenStarting", false);
            firefoxOptions.SetPreference("browser.download.manager.useWindow", false);
            firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip, application/x-gzip, application/msword, application/csv, text/csv, application/pdf");
            firefoxOptions.SetPreference("browser.download.manager.showAlertOnComplete", false);
            firefoxOptions.SetPreference("browser.download.manager.closeWhenDone", false);
            firefoxOptions.SetPreference("pdfjs.disabled", true);
            var profile = new FirefoxProfile {AcceptUntrustedCertificates = true, AssumeUntrustedCertificateIssuer = false};
            firefoxOptions.Profile = profile;
            return firefoxOptions;
        }

        private static void PrepareChromeOptions(string downloadDirectory, out ChromeOptions chromeOptions, out ChromeDriverService chromeDriverService)
        {
            chromeOptions = new ChromeOptions();
            chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.EnableVerboseLogging = false;
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            chromeOptions.AddUserProfilePreference("plugins.plugins_disabled", new[] { "Chrome PDF Viewer" });
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--no-first-run");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-impl-side-painting");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--test-type=ui");
            chromeOptions.AddArgument("--ignore-certificate-errors");
        }

        /// <summary>
        /// parsing string
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="browserType">type of the browser</param>
        /// <returns></returns>
        public static RemoteWebDriver SetUp(string type, out BrowserType browserType)
        {

            if (Enum.TryParse(type, true, out browserType))
            {
                return GetDriver(browserType);
            }
            Logger.Instance.Info(browserType.ToString());
            Logger.Instance.Fail("couldn't parse browser type");
            return null;
        }
    }
}