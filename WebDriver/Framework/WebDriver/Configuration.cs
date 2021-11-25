using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace WebdriverFramework.Framework.WebDriver
{
    /// <summary>
    /// class with settings for the solution
    /// </summary>
    public class Configuration
    {
        protected Configuration()
        {
        }

        /// <summary>
        /// get from app.config Remote WebDriver default waiting
        /// </summary>
        public static int RemoteWebDriverWait => int.Parse(GetValue(nameof(RemoteWebDriverWait)));

        /// <summary>
        /// get from app.config Remote WebDriver default waiting
        /// </summary>
        public static int LocalWebDriverWait => int.Parse(GetValue("page_timeout"));

        /// <summary>
        /// get from app.config field Login
        /// </summary>
        public static string Login => GetValue(nameof(Login));

        /// <summary>
        /// get from app.config field Password
        /// </summary>
        public static string Password => GetValue(nameof(Password));

        /// <summary>
        /// get from app.config field LoginURL
        /// </summary>
        public static string LoginUrl => GetValue("LoginURL");

        /// <summary>
        /// get from app.config field LoginURL
        /// </summary>
        public static string AdminUrl => GetValue("AdminLoginURL");

        /// <summary>
        /// get from app.config field Selenium Grid URL
        /// </summary>
        public static string SeleniumGridUrl => GetValue(nameof(SeleniumGridUrl));

        /// <summary>
        /// get from app.config field UseSeleniumGrid
        /// </summary>
        public static bool UseSeleniumGrid => bool.Parse(GetValue(nameof(UseSeleniumGrid)));

        /// <summary>
        /// get from app.config field UseLocalFileDetector
        /// </summary>
        public static bool UseLocalFileDetector => bool.Parse(GetValue(nameof(UseLocalFileDetector)));

        /// <summary>
        /// get from app.config field HowManyTimesTryToInstanceBrowser
        /// </summary>
        public static int HowManyTimesTryToInstanceBrowser => int.Parse(GetValue(nameof(HowManyTimesTryToInstanceBrowser)));

        /// <summary>
        /// get from app.config field element_timeout
        /// </summary>
        public static string ElementTimeout => GetValue("element_timeout");

        /// <summary>
        /// get from app.config field email_wait_timeout
        /// defines timeout for waiting emails
        /// </summary>
        public static string EmailWaitTimeout => GetValue("email_wait_timeout");


        /// <summary>
        /// get from app.config field page_timeout
        /// </summary>
        public static string PageTimeout => GetValue("page_timeout");

        /// <summary>
        /// get from app.config field page_timeout
        /// </summary>
        public static string QueryTimeout => GetValue("db_query_timeout");

        /// <summary>
        /// get from app.config field login_request_timeout_ms to validate login process in DB
        /// </summary>
        public static string LoginRequestTimeOut => GetValue("login_request_timeout_ms");

        /// <summary>
        /// get from app.config field Delay
        /// </summary>
        public static string Delay => GetValue(nameof(Delay));

        /// <summary>
        /// get from app.config field UseSystemBrowserParameter
        /// </summary>
        public static bool UseSystemBrowserParameter => bool.Parse(GetValue(nameof(UseSystemBrowserParameter)));

        /// <summary>
        /// get from app.config field Browser
        /// </summary>
        public static string Browser => UseSystemBrowserParameter ? Environment.GetEnvironmentVariable("BROWSER") : GetValue("browser");

        /// <summary>
        ///  get boolean value for the field MobileTesting from environment or from app.config
        /// </summary>
        public static bool MobileTesting => bool.Parse(GetValue(nameof(MobileTesting)));

        /// <summary>
        /// get from app.config field file_downloading_timeout_ms
        /// </summary>
        public static string FileDownloadingTimeoutMs => GetValue("file_downloading_timeout_ms");

        /// <summary>
        /// described preferences for database
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// get from app.config field ProviderInvariantName
            /// </summary>
            public static string ProviderInvariantName => GetDatabaseValue("provider_invariant_name");

            /// <summary>
            /// get from app.config field ConnectionString
            /// </summary>
            public static string ConnectionString => GetDatabaseValue("connection_string");

            /// <summary>
            /// get alternative string from app.config field ConnectionString
            /// </summary>
            public static string ConnectionString2 => GetDatabaseValue("connection_string2");
        }

        private static string GetDatabaseValue(string key)
        {
            return GetValue("database." + key);
        }

        /// <summary>
        /// described preferences of the launching with browserstack
        /// </summary>
        public static class BrowserStack
        {
            /// <summary>
            /// browserstack hub
            /// </summary>
            public static string Hub => GetBrowserStackValue("hub");

            /// <summary>
            /// user to connect to browserstack environment
            /// </summary>
            public static string User => GetBrowserStackValue("user");

            /// <summary>
            /// key to connect to browserstack environment
            /// </summary>
            public static string Key => GetBrowserStackValue("key");

            /// <summary>
            /// browser name according to the browserstack specification
            /// </summary>
            public static string BrowserType => GetBrowserStackValue("browser");

            /// <summary>
            /// browser version according to the browserstack specification
            /// </summary>
            public static string BrowserVersion => GetBrowserStackValue("browser_version");

            /// <summary>
            /// OS name according to the browserstack specification
            /// </summary>
            public static string OS => GetBrowserStackValue("os");

            /// <summary>
            /// OS version according to the browserstack specification
            /// </summary>
            public static string OSVersion => GetBrowserStackValue("os_version");

            /// <summary>
            /// screen resolution according to the browserstack specification
            /// </summary>
            public static string Resolution => GetBrowserStackValue("resolution");

            /// <summary>
            /// value run or not (true/false) local browserstack 
            /// </summary>
            public static string Local => GetBrowserStackValue("local");

            /// <summary>
            /// value run or not (true/false) debug browserstack 
            /// </summary>
            public static string Debug => GetBrowserStackValue("debug");
            //======Mobile browserstack testing=======

            /// <summary>
            /// browserName (for mobile testing) according to the browserstack specification
            /// </summary>
            public static string BrowserName => GetBrowserStackValue("browserName");

            /// <summary>
            /// platform name (for mobile testing) according to the browserstack specification
            /// </summary>
            public static string Platform => GetBrowserStackValue("platform");

            /// <summary>
            /// device name (for mobile testing) according to the browserstack specification
            /// </summary>
            public static string Device => GetBrowserStackValue("device");

            /// <summary>
            /// device orientation (for mobile testing) according to the browserstack specification
            /// </summary>
            public static string DeviceOrientation => GetBrowserStackValue("deviceOrientation");

            /// <summary>
            /// emulator usage (for mobile testing) according to the browserstack specification
            /// value must be true/false
            /// </summary>
            public static bool Emulator => bool.Parse(GetBrowserStackValue("emulator"));

            /////=====================================
        }

        private static string GetBrowserStackValue(string key)
        {
            return GetValue("browserstack." + key);
        }

        /// <summary>
        /// described preferences of the launching with chrome mobile emulator
        /// </summary>
        public static class ChromeMobileEmulator
        {
            /// <summary>
            /// device name
            /// </summary>
            public static string DeviceName => GetChromeMobileEmulator("deviceName");
        }

        private static string GetChromeMobileEmulator(string key)
        {
            return GetValue("chromemobileemulator." + key);
        }

        /// <summary>
        /// get from app.config field and converts into string
        /// </summary>
        protected static string GetValue(string key)
        {
            return GetEnviromentVar(key, ConfigurationManager.AppSettings.Get(key));
        }

        /// <summary>
        /// returns value of environment variable
        /// </summary>
        /// <param name="var">variable's name</param>
        /// <param name="defaultValue">default value, will be returned if env var was not setted</param>
        /// <returns>value of environment variable</returns>
        public static string GetEnviromentVar(string var, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(var) ?? defaultValue;
        }
        /// <summary>
        /// get DownloadDirectory browser depended
        /// </summary>
        public static string DownloadDirectory
        {
            get
            {
                if (Browser == "Edge" || Browser == "Iexplore")
                {
                    string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    return Path.Combine(pathUser, "downloads");
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "downloads", Thread.CurrentThread.ManagedThreadId.ToString());
            }
        }
    }
}