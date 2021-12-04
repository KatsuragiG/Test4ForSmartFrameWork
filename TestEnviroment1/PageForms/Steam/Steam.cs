using OpenQA.Selenium;
using System;
using WebdriverFramework.Framework.WebDriver;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.SteamForm
{
    public class Steam : BaseForm
    {
        private static readonly By titleLocator = By.XPath("//div[contains(@class, 'responsive_page_frame')]");

        protected const string searchBox = "//div[contains(@class, 'searchbox')]";
        protected const string searchField = searchBox + "//input";
        protected readonly Button searchButton = new Button(By.XPath("//div[contains(@class, 'searchbox')]//a//img"), "Search button");

        public Steam() : base(titleLocator, "Steam Main Menu Form")
        {
        }

        public Steam(By titleLocator, string title) : base(titleLocator, title)
        {
        }

        public void ClickOnSearchButton()
        {
            searchButton.Click();
        }

        public bool IsSearchBoxPresent()
        {            
            return new Label(By.XPath(searchBox), "Search").IsExists();
        }

        public void SetTextInSearchField(string sometext)
        {                      
            var searchfield = GetSearchTextBoxField();
            searchfield.SetText(sometext);            
        }

        private TextBox GetSearchTextBoxField()
        {
            return new TextBox(By.XPath($"{searchField}"), "Search field textbox");
        }

        public bool IsSteamPageLoaded()
        {
            try
            {
                new Steam().AssertIsOpen();
                return true;
            }
            catch (Exception)
            {
                Log.Info("NOT Edit Template Page Loaded");
                return false;
            }
        }
    }
}