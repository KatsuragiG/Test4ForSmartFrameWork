using OpenQA.Selenium;
using System;
using PageForms.ModelCars.NavigationMenu;

namespace PageForms.ModelCars.CarsForm
{
    public class CarsForm : NavigationMenuForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'home_page']");     

        public CarsForm() : base(titleLocator, "Cars Main Menu Form")
        {
        }

        public CarsForm(By titleLocator, string title) : base(titleLocator, title)
        {
        }        

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