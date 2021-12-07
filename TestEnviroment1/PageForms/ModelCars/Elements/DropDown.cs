using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace ModelCars.Elements
{
    public class DropDown : BaseElement
    {
        private const string itemsDropDownLocator = "//select[contains(@class, 'my-search-') or contains(@class, 'sds-text-field')]//option[not(contains(text(), 'Choose'))]";        
        private readonly string itemDropDownLocator = itemsDropDownLocator + "[contains(text(), '{0}')]";
        private readonly string itemsDropDownLocatorEnum =
          "//div[contains(@class, 'sds-field-group field-group-melded')]//label[text() = '{0}']/preceding-sibling::select[contains(@class, 'js-mmy-search')]//option[not(contains(text(), 'Choose'))]";
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
            Log.Info(FormatLogMsg($"selecting option by text '{text}'"));
            Element.Click();
            var dropDownItem = new Label(By.XPath(string.Format(itemDropDownLocator, text)), text);
            dropDownItem.WaitForElementIsPresent();
            dropDownItem.ClickAndWaitForLoading();
        }

        public void SelectByTextNonClick(string text)
        {
            WaitForElementIsPresent();
            Log.Info(FormatLogMsg($"selecting option by text '{text}'"));            
            var dropDownItem = new Label(By.XPath(string.Format(itemDropDownLocator, text)), text);
            dropDownItem.WaitForElementIsPresent();
            dropDownItem.ClickAndWaitForLoading();
        }               

        public List<string> GetItemsFromDropDown(string dropdown)
        {
            return GetElement().FindElements(By.XPath(string.Format(itemsDropDownLocatorEnum, dropdown))).Select(value => value.Text).ToList();
        }            
    }
}