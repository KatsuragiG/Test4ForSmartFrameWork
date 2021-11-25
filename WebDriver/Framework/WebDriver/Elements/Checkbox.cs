using OpenQA.Selenium;

namespace WebdriverFramework.Framework.WebDriver.Elements
{
    /// <summary>
    /// class describes the interface element "CheckBox"
    /// </summary>
    public class Checkbox: BaseElement
    {
        /// <summary>
        /// constructor with two parameters
        /// </summary>
        /// <param name="locator">locator By of the checkBox</param>
        /// <param name="name">name of the checkBox</param>
        public Checkbox(By locator, string name) : base(locator, name)
        { }

        /// <summary>
        /// gets the type of the checkBox 
        /// </summary>
        /// <returns>type of the checkBox</returns>
        protected override string GetElementType()
        {
            return "Checkbox";
        }

        /// <summary>
        /// set value​​(to check whether it is necessary to change the current)
        /// </summary>
        /// <param name="state">value(true or false)</param>
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
        /// uncheck the checkBox
        /// </summary>
        public void Uncheck()
        {
            Check(false);
        }

        /// <summary>
        /// check the checkBox
        /// </summary>
        public void Check()
        {
            Check(true);
        }

        /// <summary>
        /// set checkbox to state
        /// </summary>
        /// <param name="state"></param>
        public void CheckJs(bool state)
        {
            WaitForElementExists();
            Log.Info(string.Format("setting state '{0}' for checkbox " + Name, state));
            // get current state of checkbox
            var currentState = (bool)Browser.GetDriver().ExecuteScript("return arguments[0].checked", Element);
            if (currentState != state)
            {
                Browser.GetDriver().ExecuteScript("arguments[0].click();", Element);
                Browser.WaitForPageToLoad();
            }
            currentState = (bool)Browser.GetDriver().ExecuteScript("return arguments[0].checked", Element);
            Log.Info(string.Format("current state is '{0}' for checkbox " + Name, currentState));
        }
    }
}
