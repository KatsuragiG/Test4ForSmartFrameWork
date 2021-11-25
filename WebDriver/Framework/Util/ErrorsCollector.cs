using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebdriverFramework.Framework.WebDriver;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// Errors Collectors from execution level
    /// </summary>
    public static class ErrorsCollector
    {
        private enum Useless
        {
            [EnumExtensions.StringMappingAttribute("Error in parsing value for")]
            ErrorInParsingValueFor
        }

        private enum Usefull
        {
            [EnumExtensions.StringMappingAttribute("error")] Error,
            [EnumExtensions.StringMappingAttribute("Failed to load")] FailedToLoad,
            [EnumExtensions.StringMappingAttribute("download failed")] DownloadFailed


            //Failed to load resource: the server responded with a status of 404 (Not Found)
            //Failed to load resource: the server responded with a status of 500 (Internal Server Error)
        }
        /// <summary>
        /// get errors from browser console
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static List<string> GetBrowserErrors(IWebDriver driver)
        {
            if (((RemoteWebDriver)driver).Capabilities.GetCapability("browserName").Equals("Chrome".ToLower()))
            {
                try
                {
                    var logStore = driver.Manage().Logs.GetLog("browser");
                    var errors = logStore.Where(item => IsUsefull(item) && !IsUseless(item));
                    return errors.Select(entry => entry.Message).ToList();
                }
                catch (NullReferenceException e)
                {
                    Logger.Instance.Info($"Error at getting browser console with stack '{e}'");
                }
                catch (Exception e)
                {
                    Logger.Instance.Warn($"Could not generate logs with stack '{e}'");
                }
            }
            return new List<string>();
        }
        
        private static bool IsUsefull(LogEntry line)
        {
            return
                EnumExtensions.EnumUtility.GetValues<Usefull>()
                    .Any(item => line.Message.ToLower().Contains(item.GetStringMapping().ToLower()));
        }

        private static bool IsUseless(LogEntry line)
        {
            return
                EnumExtensions.EnumUtility.GetValues<Useless>()
                    .Any(item => line.Message.ToLower().Contains(item.GetStringMapping().ToLower()));
        }
        
    }
}
