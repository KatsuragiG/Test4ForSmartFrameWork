using System.Collections.Generic;
using OpenQA.Selenium;

namespace WebdriverFramework.Framework.WebDriver.Elements
{
    /// <summary>
    /// class describes the interface element "label"
    /// </summary>
    public class Label : BaseElement
    {
        /// <summary>
        /// constructor with two parameters
        /// </summary>
        /// <param name="locator">locator By of the label</param>
        /// <param name="name">name of the label</param>
        public Label(By locator, string name) : base(locator, name)
        { }

        /// <summary>
        /// gets the type of the label 
        /// </summary>
        /// <returns>type of the label</returns>
        protected override string GetElementType()
        {
            return "Label";
        }

        /// <summary>
        /// gets all labels
        /// </summary>
        /// <param name="locator">locator By of labels</param>
        /// <param name="name">name of labels</param>
        /// <returns>all labels on the page</returns>
        public Label[] GetAllLabels(string locator, string name)
        {
            int count = Browser.GetDriver().FindElements(By.XPath(locator)).Count;
            var results = new Label[count];
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    results[i] = new Label(By.XPath(locator + "[" + (i + 1) + "]"), name + (i + 1));
                }
            }
            return results;
        }

        /// <summary>
        /// gets all labels
        /// </summary>
        /// <param name="locator">locator By of labels</param>
        /// <returns>all labels on the page</returns>
        public Label[] GetAllLabels(string locator)
        {

            int count = Browser.GetDriver().FindElements(By.XPath(locator)).Count;
            var results = new Label[count];
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    results[i] = new Label(By.XPath(locator + "[" + (i + 1) + "]"), "");
                }
            }
            return results;
        }

        /// <summary>
        /// gets all label's text as collection
        /// </summary>
        /// <returns>collection of strings</returns>
        public List<string> GetAllLabelsText()
        {
            var elements = Browser.GetDriver().FindElements(Locator);
            var result = new List<string>();
            foreach (var element in elements)
            {
                try
                {
                    result.Add(element.Text);
                }
                catch (StaleElementReferenceException e)
                {
                    Logger.Instance.Info($"StaleElementReferenceException error '{e}'");
                }
            }

            return result;
        }

        /// <summary>
        /// gets attribute href
        /// </summary>
        /// <returns>attribute href of the label</returns>
        public string GetHref()
        {
            return GetAttribute("href");
        }

        /// <summary>
        /// gets attribute checked
        /// </summary>
        /// <returns>attribute checked of the label</returns>
        public string GetChecked()
        {
            return GetAttribute("checked");
        }
    }
}
