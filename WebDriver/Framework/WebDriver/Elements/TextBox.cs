using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace WebdriverFramework.Framework.WebDriver.Elements
{
    /// <summary>
    /// class describes the interface element "textbox"
    /// </summary>
    public class TextBox : BaseElement
    {
        /// <summary>
        /// constructor with two parameters
        /// </summary>
        /// <param name="locator">locator By of the textbox</param>
        /// <param name="name">name of the textbox</param>
        public TextBox(By locator, string name) : base(locator, name)
        { }

        /// <summary>
        /// gets the type of the textbox 
        /// </summary>
        /// <returns>type of the textbox</returns>
        protected override string GetElementType()
        {
            return "TextBox";
        }

        /// <summary>
        /// set text without clear
        /// </summary>
        /// <param name="text">text for set</param>
        public void Type(string text)
        {
            WaitForElementIsPresent();
            Info($"Typing '{text}'");
            Element.SendKeys(text);
        }

        /// <summary>
        /// clear field and set text
        /// </summary>
        /// <param name="text">text for set</param>
        public void SetText(string text)
        {
            List<string> list = ConvertTextToListOfTypedString(text);
            foreach (var letter in list)
            {
                Info($"Setting '{letter}'");
                Element.Click();
                Element.SendKeys(letter);
            }
        }

        /// <summary>
        /// check that the element is readonly (performed by a class member)
        /// </summary>
        /// <returns>true if element is enabled</returns>
        public bool IsReadOnly()
        {
            WaitForElementIsPresent();
            string elementClass = Element.GetAttribute("readonly");
            var result = false;
            try
            {
                result = elementClass.ToLower().Contains("readonly");
            }
            catch (Exception e)
            {
                Logger.Instance.Info($"There are no readonly attribute for {Element} with error {e}");
            }
            return Element.Enabled && !result;
        }

        /// <summary>
        /// clear field and set text
        /// </summary>
        /// <param name="text">text for set</param>
        public void SetTextWithAction(string text)
        {
            WaitForElementIsPresent();
            WaitForIsEnabled();
            Element.Clear();
            new Actions(Browser.GetDriver()).MoveToElement(Element).Click().SendKeys(Element, text).Perform();
        }

        /// <summary>
        /// clear field and set text
        /// </summary>
        public void ClearField()
        {
            WaitForElementIsPresent();
            Element.Clear();
        }

        /// <summary>
        /// type text into textbox without wait before element is dispayed
        /// </summary>
        /// <param name="text">typed text</param>
        public void TypeInvisible(string text)
        {
            WaitForElementExists();
            Info($"Type '{text}'");
            Browser.GetDriver().ExecuteScript("arguments[0].value=" + text, Element);
        }

        /// <summary>
        /// submit form
        /// </summary>
        public void Submit()
        {
            WaitForElementIsPresent();
            Info("submitting");
            Element.Submit();
        }

        /// <summary>
        /// get value for numeric stepper
        /// </summary>
        public string GetValueByJs()
        {
            try
            {
                return ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("return document.getElementsByClassName('app-input app-input-incremental__input')[0].value;").ToString();
            }
            catch (Exception r)
            {
                Logger.Instance.Warn(r.Message);
                return string.Empty;
            }
        }
    }
}