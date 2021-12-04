using ModelCars.Elements;
using OpenQA.Selenium;
using PageForms.ModelCars.NavigationMenu;
using System;
using System.Collections.Generic;
using WebdriverFramework.Framework.WebDriver;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.ModelCars.ResearchForm
{
    public class ResearchForm : NavigationMenuForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'research-landing-page']");

        private const string ResearchPageContent = "//div[contains(@class, 'section__content')]";
        private const string DropDownSection = "//div[contains(@class, '-group-melded')]";

        private DropDown MakeDropDown = new DropDown(By.XPath("//div[contains(@class, 'field1')]"), "Make dropdown");

        private Button ResearchButton = new Button(By.XPath("//button[contains(@class, '-search-button')]"), "Research button");

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

        public void SelectValueInMakeDropDown(string text)
        {
            MakeDropDown.ScrollIntoViewWithCenterAligning();
            MakeDropDown.SelectByText(text);
        }

        public List<string> GetElementFromMakeDropDown()
        {
           return MakeDropDown.GetItemsFromMakeDropDown();
        }

        //public bool IsSteamPageLoaded()
        //{
        //    try
        //    {
        //        new Steam().AssertIsOpen();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        Log.Info("NOT Edit Template Page Loaded");
        //        return false;
        //    }
        //}
    }
}