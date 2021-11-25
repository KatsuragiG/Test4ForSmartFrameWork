using OpenQA.Selenium;
using WebdriverFramework.Framework.WebDriver;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// Class for Js actions to web elements
    /// </summary>
    public class JsActions
    {
        /// <summary>
        /// Get Background Color Of Pseudo Element ::before ViaJs
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public string GetBackgroundColorOfPseudoElementViaJs(IWebElement webElement)
        {
            return GetBackgroundColorOfPseudoElementViaJs(webElement, "::before");
        }

        /// <summary>
        /// Get Background Color Of Pseudo Element ViaJs
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="pseudoElement"></param>
        /// <returns></returns>
        public string GetBackgroundColorOfPseudoElementViaJs(IWebElement webElement, string pseudoElement)
        {
            return GetPropertyValueOfPseudoElementViaJs(webElement, "background-color", pseudoElement);
        }

        /// <summary>
        /// Get Color Of Pseudo Element ::before Via Js
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public string GetColorOfPseudoElementViaJs(IWebElement webElement)
        {
            return GetColorOfPseudoElementViaJs(webElement, "::before");
        }

        /// <summary>
        /// Get Color Of Pseudo Element Via Js
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="pseudoElement"></param>
        /// <returns></returns>
        public string GetColorOfPseudoElementViaJs(IWebElement webElement, string pseudoElement)
        {
            return GetPropertyValueOfPseudoElementViaJs(webElement, "color", pseudoElement);
        }

        /// <summary>
        /// Get Page Loading State via Js
        /// </summary>
        /// <returns></returns>
        public object GetPageLoadingState()
        {
            return ((IJavaScriptExecutor)Browser.Instance.GetDriver())
                .ExecuteScript("return document['readyState'] ? 'complete' == document.readyState : true");
        }

        private string GetPropertyValueOfPseudoElementViaJs(IWebElement webElement, string propertyValue, string pseudoElement)
        {
            return ((IJavaScriptExecutor)Browser.Instance.GetDriver())
                .ExecuteScript($"return window.getComputedStyle(arguments[0], '{pseudoElement}').getPropertyValue('{propertyValue}');", webElement)
                .ToString();
        }

        /// <summary>
        /// Get item from site Local Storage in Browser
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetItemFromLocalStorage(string itemName)
        {
            return ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript($"return window.localStorage.getItem('{itemName}');").ToString();
        }

        /// <summary>
        /// Scroll Into View Via Js
        /// </summary>
        /// <param name="webElement"></param>
        public void ScrollIntoViewViaJs(IWebElement webElement)
        {
            ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
        }

        /// <summary>
        /// Scroll Right Via Js
        /// </summary>
        /// <param name="webElement"></param>
        public void ScrollRightViaJs(IWebElement webElement)
        {
            ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript("arguments[0].scrollLeft += 250;", webElement);
        }

        /// <summary>
        /// Clear all items from Session storage
        /// </summary>
        /// <returns></returns>
        public void ClearSessionStorage()
        {
            ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript("window.sessionStorage.clear();");
        }

        /// <summary>
        /// Clear all items from Local storage
        /// </summary>
        /// <returns></returns>
        public void ClearLocalStorage()
        {
            ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript("window.localStorage.clear();");
        }

        /// <summary>
        /// click via JS.
        /// </summary>
        public void ClickViaJs(IWebElement webElement)
        {
            ((IJavaScriptExecutor)Browser.Instance.GetDriver()).ExecuteScript("arguments[0].click();", webElement);
        }

        /// <summary>
        /// focus via JS.
        /// </summary>
        public void FocusViaJs(IWebElement webElement)
        {
            var jsExecutor = ((IJavaScriptExecutor)Browser.Instance.GetDriver());
            jsExecutor.ExecuteScript("window.focus();");
            jsExecutor.ExecuteScript("arguments[0].focus();", webElement);
        }

        /// <summary>
        /// click via dispatchEvent.
        /// </summary>
        public void ClickInvisibleViaJs(IWebElement webElement)
        {
            var jsExecutor = ((IJavaScriptExecutor)Browser.Instance.GetDriver());
            jsExecutor.ExecuteScript("arguments[0].dispatchEvent(new MouseEvent('click', {view: window, bubbles:true, cancelable: true}))", webElement);            
        }
    }
}