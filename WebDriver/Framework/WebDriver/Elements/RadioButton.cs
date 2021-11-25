using OpenQA.Selenium;

namespace WebdriverFramework.Framework.WebDriver.Elements
{
    /// <summary>
    /// class describes the interface element "radio button"
    /// </summary>
    public class RadioButton: BaseElement
    {
        /// <summary>
        /// constructor with two parameters
        /// </summary>
        /// <param name="locator">locator By of the radio button</param>
        /// <param name="name">name of the radio button</param>
        public RadioButton(By locator, string name) : base(locator, name)
        { }

        /// <summary>
        /// gets the type of the radio button 
        /// </summary>
        /// <returns>type of the radio button</returns>
        protected override string GetElementType()
        {
            return "RadioButton";
        }

        /// <summary>
        /// set value (with checking)
        /// </summary>
        /// <param name="state">state(true or false)</param>
        public void Check(bool state)
        {
            WaitForElementIsPresent();
            Info($"setting state '{state}'");
            if ((state && !Element.Selected) || (!state && Element.Selected))
            {
                Element.Click();
            }
        }

        /// <summary>
        /// check radio button is true
        /// </summary>
        public void Check()
        {
            Check(true);
        }
    }
}
