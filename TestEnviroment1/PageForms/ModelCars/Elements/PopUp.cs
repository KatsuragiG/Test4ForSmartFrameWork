using OpenQA.Selenium;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace ModelCars.Elements
{
    public class PopUp : BaseElement
    {
        public PopUp(By locator, string name) : base(locator, name)
        {
        }

        protected override string GetElementType()
        {
            return "PopUp";
        }
    }
}