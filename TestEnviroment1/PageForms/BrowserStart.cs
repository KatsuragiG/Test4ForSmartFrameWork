﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace PageForms.BrowserStart
{
    public class BrowserStart
    {
        private readonly Checker checker;
        private readonly Logger _logger;

        public BrowserStart()
        {
            checker = new Checker(Logger.Instance, Browser.Instance);
            _logger = Logger.Instance;
        }       

        public void StartBrowserOnStartPage()
        {
            Browser.Instance.Quit();
            Browser.Instance.InitDriver();
            Browser.Instance.NavigateToStartPage();
            Browser.Instance.WindowMaximise();
        }
    }
}