﻿using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System;
using WebdriverFramework.Framework.WebDriver.Elements;
using System.Collections.ObjectModel;

namespace TestEnviroment.Dropdown
{
    public class DropDown : BaseElement
    {
        private const string dropDownValues = "//div[@class = 'dropcontainer']//ul//li//a[text() = '{0}']";

        public DropDown(By locator, string name) : base(locator, name)
        {
        }

        protected override string GetElementType()
        {
            return "DropDown";
        }

        public void SelectByText(string text)
        {
            WaitForElementIsPresent();
            Log.Info(FormatLogMsg($"selecting sortby by '{text}'"));
            Element.Click();
            var dropDownItem = new Label(By.XPath(string.Format(dropDownValues, text)), text);
            dropDownItem.WaitForElementIsPresent();
            dropDownItem.ClickAndWaitForLoading();
        }

        //public List<string> GetDropDownLabelNames()
        //{
        //    var cbLabelList = (ReadOnlyCollection<IWebElement>)FindElements(resultsPrices);
        //    var cbLabelNames = cbLabelList.Select(t => t.Text.Trim()).ToList();

        //    return cbLabelNames.Where(x => x != string.Empty).ToList();
        //}                     
    }
}