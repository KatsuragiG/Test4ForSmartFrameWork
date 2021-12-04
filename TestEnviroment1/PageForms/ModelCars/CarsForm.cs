using OpenQA.Selenium;
using System;
using WebdriverFramework.Framework.WebDriver;
using WebdriverFramework.Framework.WebDriver.Elements;
using PageForms.ModelCars.NavigationMenu;

namespace PageForms.ModelCars.CarsForm
{
    public class CarsForm : NavigationMenuForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'home_page']");     

        public CarsForm() : base(titleLocator, "Steam Main Menu Form")
        {
        }

        public CarsForm(By titleLocator, string title) : base(titleLocator, title)
        {
        }

        //public bool IsSearchBoxPresent()
        //{            
        //    return new Label(By.XPath(searchBox), "Search").IsExists();
        //}

        //public void SetTextInSearchField(string sometext)
        //{                      
        //    var searchfield = GetSearchTextBoxField();
        //    searchfield.SetText(sometext);            
        //}

        //private TextBox GetSearchTextBoxField()
        //{
        //    return new TextBox(By.XPath($"{searchField}"), "Search field textbox");
        //}

        public bool IsCarsPageLoaded()
        {
            try
            {
                new CarsForm().AssertIsOpen();
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