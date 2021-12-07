using ModelCars.Elements;
using OpenQA.Selenium;
using PageForms.Enums.DropDownResearchEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.ModelCars.ResearchForm
{
    public class ResearchForm : TrimComparePage
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'research-landing-page']");

        private const string ResearchPageContent = "//div[contains(@class, 'section__content')]";
        private const string DropDownSection = "//div[contains(@class, '-group-melded')]";
        private const string DropDowns = "//div[contains(@class, 'field{0}')]";
        private const string TrimLocator = "//h2[contains(text(), 'trim')]";
        private const string DescriptionsForCar = "//div[@class = 'header-container']//h1";
        private const string ToolsSection = "//div[contains(@class, 'tools')]";

        private readonly Label SideBySideComparison = new Label(By.XPath(ToolsSection + "//h3[text() = 'Side-by-side comparisons']"), "Side by Side comparison label");
        private readonly Link CompareModelLink = new Link(By.XPath(ToolsSection + "//a[contains(@data-linkname, '-compare')]"), "Compare models link");
        private readonly Link TrimLink = new Link(By.XPath("//a[contains(@data-linkname, 'trim-compare')]"), "Trim Compare Link");
        private readonly Button ResearchButton = new Button(By.XPath("//button[contains(@class, '-search-button')]"), "Research button");

        public ResearchForm() : base(titleLocator, "Research Page Form")
        {
        }

        public ResearchForm(By titleLocator, string title) : base(titleLocator, title)
        {
        }

        public void ClickOnResearchButton()
        {
            ResearchButton.Click();
        }

        public void SelectValueInDropDown(string text, DropDownResearchEnums value)
        {
            var dropdown = new DropDown(By.XPath(string.Format(DropDowns, value.GetValue())), "DropDown");
            dropdown.ScrollIntoViewWithCenterAligning();
            dropdown.Click();
            dropdown.SelectByText(text);
        }

        public string SelectRandomValueInDropDown(DropDownResearchEnums dropdown)
        {
            var list = GetElementFromDropDown(dropdown);
            var rundomValueForSelection = RundomValueForDropDown(list);
            SelectValueInDropDown(rundomValueForSelection, dropdown);            
            return rundomValueForSelection;
        }

        public List<string> GetElementFromDropDown(DropDownResearchEnums dropdown)
        {
            var sectionDropDown = new DropDown(By.XPath(string.Format(DropDowns, dropdown.GetValue())), "DropDown");
            sectionDropDown.ScrollIntoViewWithCenterAligning();
            return sectionDropDown.GetItemsFromDropDown(dropdown.GetStringMapping());
        }

        public bool IsTrimLinkPresent()
        {
            return TrimLink.IsPresent();
        }

        public void ClickOnTrimCompareLink()
        {
            TrimLink.ScrollIntoViewWithCenterAligning();
            TrimLink.ClickAndWaitForLoading();
        }

        public string GetDescriptionsForCar()
        {
            return new Label(By.XPath(DescriptionsForCar), "Descriptions for car").GetText();
        }

        public bool IsCompareModelLinkPresent()
        {
            return CompareModelLink.IsPresent();
        }

        public void ClickOnCompareModelLink()
        {
            CompareModelLink.ScrollIntoViewWithCenterAligning();
            CompareModelLink.ClickAndWaitForLoading();
        }

        private string RundomValueForDropDown(List<string> items)
        {
            var rnd = new Random();
            var numberOfValue = rnd.Next(0, items.Count);
            var textElement = items.ElementAt(numberOfValue);
            return textElement;
        }
    }
}