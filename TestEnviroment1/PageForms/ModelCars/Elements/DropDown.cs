using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System;
using WebdriverFramework.Framework.WebDriver.Elements;
using WebdriverFramework.Framework.WebDriver;

namespace ModelCars.Elements
{
    public class DropDown : BaseElement
    {
        private const int MaximumArrowsDepthValue = 5;
        private const string ClassPropertyForOpenedDropdown = "rotated";
        private const string SelectedItemsMultipleSelectionLocator = ".//span[contains(@class, 'multi-value-label')]";

       
        private readonly string itemTreeSelectMenu = "//div[contains(@class, 'vue-treeselect__menu')][./div[contains(@class, '__list')]]";
        private readonly string itemTreeSelectIndentLevelLocator = "//div[contains(@class, 'indent-level')][./div/div/label[@title='{0}' or text()='{0}']]";        

        private const string MakeDropDown = "//div[contains(@class, 'field1')]";
        private const string ModelDropDown = "//div[contains(@class, 'field2')]";
        private const string YearDropDown = "//div[contains(@class, 'field3')]";

        private const string itemsDropDownLocator = "//select[contains(@class, 'my-search-')]//option[not(contains(text(), 'Choose'))]";        
        private readonly string itemDropDownLocator = itemsDropDownLocator + "[contains(text(), '{0}')]";
        //div[contains(@class, 'sds-field-group field-group-melded')]//label[text() = 'Make']/preceding-sibling::select[contains(@class, 'js-mmy-search')]//option[not(contains(text(), 'Choose'))]
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

        //public void SelectInTreeSelectByText(string text)
        //{
        //    WaitForElementIsPresent();
        //    Log.Info(FormatLogMsg($"selecting option by text '{text}'"));
        //    Element.Click();
        //    var dropDownItem = GetDropDownItemInTreeSelect(text);
        //    dropDownItem.WaitForElementIsPresent();
        //    dropDownItem.ClickAndWaitForLoading();
        //}

        //public Label GetDropDownItemInTreeSelect(string text)
        //{
        //    return new Label(By.XPath(string.Format(itemTreeSelectLocator, text)), text);
        //}

        //public bool IsTreeSelectDropdownOpen()
        //{
        //    return GetTreeSelectSvgControlArrowElement().GetClass().Contains(ClassPropertyForOpenedDropdown);
        //}

        //public void SetTreeSelectItemOpenState(bool condition)
        //{
        //    if (IsTreeSelectDropdownOpen() != condition)
        //    {
        //        GetTreeSelectControlArrowElement().Click();
        //    }
        //}

        //public bool IsTreeSelectItemByTextPresent(string itemName)
        //{
        //    WaitForElementIsPresent();
        //    SetTreeSelectItemOpenState(true);
        //    var isElementPresent = GetTreeSelectItemLabel(itemName).IsPresent();
        //    SetTreeSelectItemOpenState(false);
        //    return isElementPresent;
        //}

        //public void SwitchAllCategoriesInTreeSelect()
        //{
        //    SwitchAllCategoriesInTreeSelect(true, MaximumArrowsDepthValue);
        //}

        //public void SwitchAllCategoriesInTreeSelect(bool switchCondition)
        //{
        //    SwitchAllCategoriesInTreeSelect(switchCondition, MaximumArrowsDepthValue);
        //}

        //public void SwitchAllCategoriesInTreeSelect(bool switchCondition, int depthValue)
        //{
        //    SetTreeSelectItemOpenState(true);
        //    for (var indentLevel = 0; indentLevel < depthValue; indentLevel++)
        //    {
        //        var treeSelectMenuLabel = GetTreeSelectMenu();
        //        try
        //        {
        //            var arrowValues = treeSelectMenuLabel.GetElement().FindElements(By.XPath(itemTreeSelectOptionArrowLocator));
        //            var listOfArrowElements = arrowValues.Where(e => switchCondition != e.GetClass().Contains(ClassPropertyForOpenedDropdown)).ToList();
        //            listOfArrowElements.Reverse();
        //            InvertCollapsedItemsInTreeSelect(listOfArrowElements);
        //        }
        //        catch (NoSuchElementException)
        //        {
        //            Log.Info($"Indent-level [{indentLevel}]: No matching items found to open them");
        //        }
        //    }
        //    SetTreeSelectItemOpenState(false);
        //}

        //public List<string> GetTreeSelectChildrenValuesForTreeSelectItem(string parentItem)
        //{
        //    SetTreeSelectItemOpenState(true);
        //    var treeSelectChildrenValues = new List<string>();
        //    if (string.IsNullOrEmpty(parentItem))
        //    {
        //        treeSelectChildrenValues = GetElement().FindElements(By.XPath(itemTreeSelectEmptyIndentLevelLocator))
        //            .Where(e => !string.IsNullOrEmpty(e.Text))
        //            .Select(element => element.Text.Trim().SetUnixNewLines().Split('\n').FirstOrDefault())
        //            .ToList();
        //    }
        //    else
        //    {
        //        var parentLevel = GetTreeSelectItemIndentLevel(parentItem);
        //        var treeSelectItem = GetTreeSelectItemLabel(parentItem);

        //        try
        //        {
        //            treeSelectChildrenValues = treeSelectItem.GetElement().FindElements(By.XPath(
        //                $".{string.Format(itemTreeSelectValuesByIndentLevelLocator, parentLevel + 1)}"))
        //                .Select(element => element.Text.Trim().SetUnixNewLines().Split('\n').FirstOrDefault()).ToList();
        //        }
        //        catch (NoSuchElementException)
        //        {
        //            Log.Info($"{parentItem} tree select item doesn't have children items");
        //        }
        //    }

        //    SetTreeSelectItemOpenState(false);
        //    return treeSelectChildrenValues;
        //}

        //public void SelectByNumber(string number, string locator)
        //{
        //    WaitForElementIsPresent();
        //    Log.Info(FormatLogMsg($"selecting option by value '{number}'"));
        //    Element.Click();
        //    var byDropDownItem = By.XPath(string.Format(locator, number));
        //    new Label(byDropDownItem, "Selected by Number Item in Dropdown").WaitForElementIsPresent();
        //    Element.FindElement(byDropDownItem).Click();
        //}

        public List<string> GetItemsFromMakeDropDown()
        {
            return GetElement().FindElements(By.XPath(MakeDropDown + itemsDropDownLocator)).Select(value => value.Text).ToList();
        }

        public List<string> GetItemsFromModelDropDown()
        {
            return GetElement().FindElements(By.XPath(MakeDropDown + itemsDropDownLocator)).Select(value => value.Text).ToList();
        }

        public List<string> GetItemsFromYearDropDown()
        {
            return GetElement().FindElements(By.XPath(MakeDropDown + itemsDropDownLocator)).Select(value => value.Text).ToList();
        }

        private Label GetTreeSelectItemLabel(string itemName)
        {
            return new Label(By.XPath(string.Format(itemTreeSelectIndentLevelLocator, itemName)), itemName);
        }               

        private Label GetTreeSelectMenu()
        {
            WaitForElementIsPresentAndGetIt();
            return new Label(By.XPath(itemTreeSelectMenu), "Tree select menu");
        }        
    }
}