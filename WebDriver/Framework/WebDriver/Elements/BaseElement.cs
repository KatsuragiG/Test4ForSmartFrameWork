using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace WebdriverFramework.Framework.WebDriver.Elements
{
    /// <summary>
    /// Abstract class for any elements in the application
    /// All classes that described elements such as Button, TextBox and etc.
    /// should inherit of this class
    /// </summary>
    public abstract class BaseElement : BaseEntity
    {
        /// <summary>
        /// Name of elements (usually used for logging)
        /// </summary>
        protected readonly string Name;
        /// <summary>
        /// locator to element
        /// </summary>
        protected By Locator;
        /// <summary>
        /// IWebElement instance
        /// </summary>
        protected IWebElement Element;

        /// <summary>
        /// Timeout ToDelay Char Typing in Ms
        /// </summary>
        private const int TimeoutToDelayCharMs = 35;

        /// <summary>
        /// constructor with two parameters
        /// </summary>
        /// <param name="locator">locator By of the element</param>
        /// <param name="name">name of the element</param>
        protected BaseElement(By locator, string name)
        {
            Locator = locator;
            Name = name == "" ? GetText() : name;
        }

        /// <summary>
        /// return IWeb-Element
        /// </summary>
        /// <returns>IWeb-Element</returns>
        public IWebElement GetElement()
        {
            try
            {
                Element = Browser.GetDriver().FindElement(Locator);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Element not found");
            }
            return Element;
        }
        /// <summary>
        /// returns elements, currently displayed on page
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public List<IWebElement> GetDisplayedElements(By locator)
        {
            var resultList = new List<IWebElement>();
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));
                IWebElement element;
                element = wait.Until(d =>
                {
                    element = d.FindElement(locator);
                    return element.Displayed ? element : null;
                });
                resultList.AddRange(Browser.GetDriver().FindElements(locator).Where(e => e.Displayed));
            }
            catch (TimeoutException ex)
            {
                Browser.GetDriver().Navigate().Refresh();
                if (resultList.Count == 0)
                {
                    Logger.Instance.Fail(ex.Message + "\n" + ("Elements not found; timeout extended"));
                }
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Elements not found");
            }
            return resultList;
        }

        /// <summary>
        /// get locator By
        /// </summary>
        /// <returns>locator By of the element</returns>
        public By GetLocator()
        {
            return Locator;
        }

        /// <summary>
        /// method gets the name of element
        /// </summary>
        /// <returns>name of the element</returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// formats the value for logging "element type - name - log splitter - the message"
        /// </summary>
        /// <param name="message">message for format</param>
        /// <returns>a formatted value for logging "element type - name - log splitter - the message"</returns>
        protected override string FormatLogMsg(string message)
        {
            return $"{GetElementType()} '{Name}' {Logger.LogDelimiter} {message}";
        }


        /// <summary>
        /// assertion of the presence of the element on the page
        /// </summary>
        public void AssertIsPresent()
        {
            if (!IsPresent())
            {
                Log.Fail(FormatLogMsg("is not present"));
            }
            else
            {
                Log.Info(FormatLogMsg("is present"));
            }
        }

        /// <summary>
        ///  wait until element is absent
        /// </summary>
        /// <returns>true if absent</returns>
        public bool IsAbsent()
        {
            return IsAbsent(5);
        }

        /// <summary>
        ///  wait until element is absent
        /// </summary>
        /// <param name="seconds">count of seconds to wait for the absence of an element on the page</param>
        /// <returns>true if absent</returns>
        public bool IsAbsent(int seconds)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Convert.ToDouble(seconds)));
            try
            {
                wait.Until(waiting =>
                {
                    var elements = Browser.GetDriver().FindElements(Locator);
                    Browser.Refresh();
                    Browser.WaitForPageToLoad();
                    if (elements.Count == 0)
                    {
                        return true;
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Logger.Instance.Debug("Element is present: " + GetLocator() + "\r\n" + e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        ///  assertion of the absence of the element on the page
        /// </summary>
        public void AssertIsAbsent()
        {
            if (IsPresent())
            {
                Log.Fail(FormatLogMsg("is present"));
            }
            else
            {
                Log.Info(FormatLogMsg("is absent"));
            }
        }

        /// <summary>
        ///  assertion of the disable element on the page
        /// </summary>
        public void AssertIsDisable()
        {
            if (!IsDisabled())
            {
                Log.Fail(FormatLogMsg("is enabled"));
            }
            else
            {
                Log.Info(FormatLogMsg("is disabled"));
            }
        }

        /// <summary>
        /// check that the element is enabled (performed by a class member)
        /// </summary>
        /// <returns>true if element is enabled</returns>
        public bool IsEnabled()
        {
            return !GetClass().ToLower().Contains("disabled") && Element.Enabled;
        }

        /// <summary>
        /// workaround for AJAX 
        /// </summary>
        /// <returns>element if element is enabled else null</returns>
        public void WaitForIsEnabled()
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));

                Element = wait.Until(d =>
                {
                    try
                    {
                        Element = d.FindElement(Locator);
                        return Element.Enabled ? Element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (InvalidElementStateException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Logger.Instance.Debug(e.Message + "\n" + FormatLogMsg(" stale element"));
                        return null;
                    }
                });
            }
            catch (TimeoutException ex)
            {
                Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not enabled"));
            }
            catch (Exception exec)
            {
                Logger.Instance.Fail(exec.Message + "\n" + FormatLogMsg(" bad situation"));
            }
        }

        /// <summary>
        /// workaround for AJAX
        /// </summary>
        /// <returns>element if element is enabled else null</returns>
        public void WaitForIsDisabled()
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));
                Element = wait.Until(d =>
                {
                    Element = d.FindElement(Locator);
                    return Element.Enabled ? null : Element;
                });
            }
            catch (TimeoutException ex)
            {
                Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Disabled"));
            }
        }

        /// <summary>
        /// wait until element is presence
        /// </summary>
        /// <returns>element if element is displayed else null</returns>
        public void WaitForElementIsPresent()
        {
            WaitForElementIsPresent(1);
        }

        /// <summary>
        /// wait until element is presence
        /// </summary>
        /// <returns>element if element is displayed else null</returns>
        public void WaitForElementIsPresent(int multyplyerForBrowserTimeout)
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), Browser.GetElementTimeoutInSeconds(multyplyerForBrowserTimeout));
                Element = wait.Until(d =>
                {
                    try
                    {
                        try
                        {
                            var elements = d.FindElements(Locator);
                            return elements.FirstOrDefault(webElement => webElement.Displayed);
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Logger.Instance.Debug(e.StackTrace);
                        return null;
                    }
                });
            }
            catch (TimeoutException ex)
            {
                Browser.GetDriver().Navigate().Refresh();
                if (!IsPresent())
                {
                    Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Present" + " by locator: " + Locator));
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                if (!IsPresent())
                {
                    Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Present" + " by locator: " + Locator));
                }
            }
        }

        /// <summary>
        /// wait until element exists
        /// initialize Element var if element exist
        /// </summary>
        public void WaitForElementExists()
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));
                Element = wait.Until(d =>
                {
                    try
                    {
                        var elements = d.FindElements(Locator);
                        var count = elements.Count;
                        if (count > 0)
                        {
                            Logger.Instance.Debug(count + " elements was found by locator " + GetLocator());
                            Element = elements[0];
                            return Element;
                        }
                        return null;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Logger.Instance.Debug(e.StackTrace);
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                if (!IsPresent())
                {
                    Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Exists" + " by locator: " + Locator));
                }
            }
        }

        /// <summary>
        ///  wait until element is presence updating each time the page
        /// </summary>
        /// <param name="url">url for update</param>
        /// <returns>element if element is displayed else null</returns>
        public void WaitForIsElementPresentReloadingPage(string url)
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));
                Element = wait.Until(d =>
                {
                    Browser.NavigateTo(url);
                    Element = d.FindElement(Locator);
                    return Element.Displayed ? Element : null;
                });
            }
            catch (TimeoutException ex)
            {
                Browser.GetDriver().Navigate().Refresh();
                if (!IsPresent())
                {
                    Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Present"));
                }
            }
        }

        /// <summary>
        /// wait until element is absent
        /// </summary>
        public void WaitForIsAbsent()
        {
            try
            {
                if (IsPresent(3))
                {
                    var wait = new WebDriverWait(Browser.GetDriver(),
                        TimeSpan.FromSeconds(Browser.TimeoutForElementWaiting));
                    wait.Until(d => (Browser.GetDriver().FindElements(Locator).Count <= 0));
                }
            }
            catch (StaleElementReferenceException)
            {
                Log.Info(FormatLogMsg(" is already absent"));
            }
            catch (WebDriverException)
            {
                Log.Info(FormatLogMsg(" is already absent"));
            }
            catch (InvalidOperationException)
            {
                Log.Info(FormatLogMsg(" is already absent"));
            }
        }

        /// <summary>
        /// wait until element present and get it
        /// initialize Element var if element present
        /// </summary>
        /// <returns>IWeb-Element</returns>
        public IWebElement WaitForElementIsPresentAndGetIt()
        {
            WaitForElementIsPresent();
            return Element;
        }

        /// <summary>
        /// wait until element exists and get it
        /// initialize Element var if element exist
        /// </summary>
        /// <returns>IWeb-Element</returns>
        public IWebElement WaitForElementExistsAndGetIt()
        {
            WaitForElementExists();
            return Element;
        }

        /// <summary>
        /// checks the presence of an element on the page
        /// </summary>
        /// <returns>true if element is displayed</returns>
        public bool IsPresent()
        {
            return IsPresent(5);
        }

        /// <summary>
        /// check if element exists on the page
        /// </summary>
        /// <returns>true if exists</returns>
        public bool IsExists()
        {
            return IsExists(5);
        }

        /// <summary>
        /// check if element active on the page by finding 'active' in class attribute
        /// </summary>
        /// <returns>true if active</returns>
        public bool IsActive()
        {
            return GetClass().Contains("active");
        }

        /// <summary>
        /// get value of data-index attribute of the element
        /// </summary>
        /// <returns>data-index attribute value</returns>
        public string GetDataIndex()
        {
            return GetAttribute("data-index");
        }

        /// <summary>
        /// gets attribute disabled
        /// </summary>
        /// <returns>attribute disabled of the element. Empty string if attribute is absent</returns>
        public string GetDisabledAttribute()
        {
            return GetAttribute("disabled") ?? String.Empty;
        }

        /// <summary>
        /// gets attribute disabled
        /// </summary>
        /// <returns>attribute disabled of the element. Empty string if attribute is absent</returns>
        public string GetDisabledAttributeInvisible()
        {
            return GetAttributeInvisible("disabled") ?? String.Empty;
        }

        /// <summary>
        /// gets Css Value Cursor
        /// </summary>
        /// <returns>Cursor css value for enabling action. Empty string if attribute is absent</returns>
        public string GetCssValueCursor()
        {
            return WaitForElementIsPresentAndGetIt().GetCssValue("cursor") ?? String.Empty;
        }

        /// <summary>
        /// check if element active on the page by finding 'checked' in class attribute
        /// </summary>
        /// <returns>true if checked</returns>
        public bool IsChecked()
        {
            return GetClass().Contains("checked");
        }

        /// <summary>
        /// check if element active on the page by finding 'selected' in class attribute
        /// </summary>
        /// <returns>true if selected</returns>
        public bool IsSelected()
        {
            return GetClass().Contains("selected");
        }

        /// <summary>
        /// check for is element present on the page
        /// </summary>
        /// <param name="sec">count of seconds to wait for the absence of an element on the page</param>
        /// <returns>true if element is present</returns>
        public bool IsPresent(int sec)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Convert.ToDouble(sec)));
            try
            {
                wait.Until(waiting =>
                {
                    var elements = Browser.GetDriver().FindElements(Locator);
                    var result = false;
                    try
                    {
                        foreach (var webElement in elements.Where(webElement => webElement.Displayed))
                        {
                            Element = webElement;
                            result = true;
                        }
                    }
                    catch (StaleElementReferenceException)
                    {
                        result = false;
                    }
                    return result;
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Logger.Instance.Debug("Element is not present: " + GetLocator() + "\r\n" + e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        /// check for is element exists on the page
        /// </summary>
        /// <param name="sec">wait in seconds until element is not exists</param>
        /// <returns>true if exists</returns>
        public bool IsExists(int sec)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Convert.ToDouble(sec)));
            try
            {
                wait.Until(waiting =>
                {
                    var elements = Browser.GetDriver().FindElements(Locator);
                    if (elements.Count > 0)
                    {
                        Element = elements[0];
                        return true;
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Logger.Instance.Debug("Element is not exists: " + GetLocator() + "\r\n" + e.StackTrace);
                return false;

            }
            return true;
        }


        /// <summary>
        /// check for is element disabled on the page
        /// </summary>
        /// <returns></returns>
        public bool IsDisabled()
        {
            try
            {
                Element = Browser.GetDriver().FindElement(Locator);
            }
            catch (Exception)
            {
                Log.Info(FormatLogMsg(" is not present"));
                return false;
            }
            return !Element.Enabled;
        }

        /// <summary>
        /// click on the element
        /// </summary>
        public void Click()
        {
            WaitForElementIsPresent();
            Info("Clicking");
            try
            {
                if (Browser.CurrentBrowser == BrowserFactory.BrowserType.Iexplore)
                {
                    new Actions(Browser.GetDriver()).MoveToElement(Element).Click(Element).Perform();
                }
                else
                {
                    Element.Click();
                }
            }
            catch (InvalidOperationException ex)
            {
                Warn(ex.Message);
                Fatal(" is not available for click ");
            }

        }

        /// <summary>
        ///click on an item and wait for the page is loaded
        /// </summary>
        public void ClickAndWaitForLoading()
        {
            try
            {
                Info("Perform click and wait for page to load");
                Click();
                Browser.WaitForPageToLoad();
            }
            catch (WebDriverException exc)
            {
                Info("An exception accured while we were trying to click by " + Name + "One attemp yet...\r\n" + exc.Message);
                WaitForElementIsPresent();
                Info("Perform click and wait for page to load");
                Click();
                Browser.WaitForPageToLoad();
            }
        }

        /// <summary>
        /// extended click through Enter
        /// </summary>
        public void ClickExt()
        {
            WaitForElementIsPresent();
            Info("extended Clicking");
            Browser.GetDriver().FindElement(Locator).SendKeys(Keys.Enter);
        }

        /// <summary>
        /// returns count of elements using findElements method of selenium
        /// </summary>
        /// <param name="locator">locator to element</param>
        /// <returns></returns>
        public int GetElementsCount(By locator)
        {
            Browser.WaitForPageToLoad();
            return Browser.GetDriver().FindElements(locator).Count;
        }

        /// <summary>
        /// click on an item ext click through Enter and wait for the page is loaded.
        /// </summary>
        public void ClickExtAndWait()
        {
            try
            {
                ClickExt();
                Browser.WaitForPageToLoad();
            }
            catch (WebDriverException exc)
            {
                Warn(exc.Message);
                Browser.GetDriver().Navigate().Refresh();
                Browser.GetDriver().FindElement(By.XPath("//")).SendKeys(Keys.Enter);

                ClickExt();
                Browser.WaitForPageToLoad();
            }
        }

        /// <summary>
        /// click via Action.
        /// </summary>
        public void ClickViaAction()
        {
            WaitForElementIsPresent();
            AssertIsPresent();
            ClickViaActionWithoutWaiting();
        }

        /// <summary>
        /// click via Action.
        /// </summary>
        public void ClickViaActionWithoutWaiting()
        {
            Info("Clicking");
            var action = new Actions(Browser.GetDriver());
            action.Click(GetElement());
            action.Perform();
        }

        /// <summary>
        /// click via JS.
        /// </summary>
        public void ClickViaJs()
        {
            WaitForElementIsPresent();
            Info("Clicking");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].click();", GetElement());
        }

        /// <summary>
        /// click via JS.
        /// </summary>
        public void FocusViaJs()
        {
            WaitForElementIsPresent();
            Info("Clicking");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].focus();", GetElement());
        }

        /// <summary>
        /// scroll element into view
        /// </summary>
        public void ScrollIntoView()
        {
            WaitForElementIsPresent();
            Info("Scrolling into view");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView(true);", GetElement());
        }

        /// <summary>
        /// scroll element into view with center aligning
        /// </summary>
        public void ScrollIntoViewWithCenterAligning()
        {
            WaitForElementIsPresent();
            Info("Scrolling into view");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'center'});", GetElement());
        }

        /// <summary>
        /// scroll element to right by Right Arrow Key
        /// </summary>
        public void ScrollHorizontalToEndFromKeyboard()
        {
            WaitForElementIsPresent();
            Info("Scrolling to the right end sending Keys.Right");
            var currentXcoordinate = Element.Location.X;
            int previousXcoordinate;
            do
            {
                new Actions(Browser.GetDriver()).MoveToElement(Element).SendKeys(Keys.Right).Perform();
                previousXcoordinate = currentXcoordinate;
                currentXcoordinate = Element.Location.X;
            } while (previousXcoordinate != currentXcoordinate);
        }

        /// <summary>
        /// scroll element to left by Left Arrow Key
        /// </summary>
        public void ScrollHorizontalToBeginFromKeyboard()
        {
            WaitForElementIsPresent();
            Info("Scrolling to the left end sending Keys.Left");
            var currentXcoordinate = Element.Location.X;
            int previousXcoordinate;
            do
            {
                new Actions(Browser.GetDriver()).MoveToElement(Element).SendKeys(Keys.Left).Perform();                
                previousXcoordinate = currentXcoordinate;
                currentXcoordinate = Element.Location.X;
            } while (previousXcoordinate != currentXcoordinate);
        }

        /// <summary>
        /// click via JS.
        /// </summary>
        public void ClickInvisible()
        {
            WaitForElementExists();
            Info("Clicking");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].click();", GetElement());
        }

        /// <summary>
        /// click on an item js click and wait for the page is loaded.
        /// </summary>
        public void ClickViaJsAndWait()
        {
            try
            {
                ClickViaJs();
                Browser.WaitForPageToLoad();
            }
            catch (WebDriverException exc)
            {
                Logger.Instance.Debug(exc.Message);
                Browser.GetDriver().Navigate().Refresh();
                Browser.GetDriver().FindElement(By.XPath("//")).SendKeys(Keys.Enter);
                ClickViaJs();
                Browser.WaitForPageToLoad();
            }
        }

        /// <summary>
        /// move the cursor to the element and click him
        /// </summary>
        public void ClickWithMouseOver()
        {
            WaitForElementIsPresent();
            Info("Clicking with mouse over");
            new Actions(Browser.GetDriver()).MoveToElement(Element).Click(Element).Perform();
        }

        /// <summary>
        /// click and look forward to the emergence of a new window
        /// </summary>
        public void ClickAndWaitForNewWindow()
        {
            int count = Browser.WindowCount();
            Click();
            Info("Select next window");
            Browser.WaitForNewWindow(count);
            Browser.SwitchWindow(count);
            Browser.WindowMaximise();
        }

        /// <summary>
        /// click and look forward to closing the current window
        /// </summary>
        public void ClickAndWaitForWindowClose()
        {
            int count = Browser.WindowCount();
            Click();
            Info("Select previous window");
            Browser.WaitForNewWindow(count - 2);
            Browser.SwitchWindow(count - 2);
        }

        /// <summary>
        /// double click
        /// </summary>
        public void DoubleClick()
        {
            WaitForElementIsPresent();
            Info("Double clicking");
            new Actions(Browser.GetDriver()).DoubleClick(Element).Perform();
        }

        /// <summary>
        /// send keys
        /// </summary>
        /// <param name="key"></param>
        public void SendKeys(string key)
        {
            Info($"Typing '{key}'");
            WaitForElementIsPresent();
            Browser.GetDriver().FindElement(Locator).SendKeys(key);
        }

        /// <summary>
        /// send keys without element waiting
        /// </summary>
        /// <param name="key"></param>
        public void SendKeysWithoutPresent(string key)
        {
            Info($"Typing '{key}'");
            Browser.GetDriver().FindElement(Locator).SendKeys(key);
        }

        /// <summary>
        /// sets the value of the title attribute
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <param name="attValue">attribute value</param>
        public void SetAttribute(string attName, string attValue)
        {
            Browser.GetDriver().ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", Element, attName, attValue);
        }

        /// <summary>
        /// gets the value of the class attribute
        /// </summary>
        /// <returns>the class value of the title attribute of the element</returns>
        public string GetClass()
        {
            return GetAttribute("class");
        }

        /// <summary>
        /// gets the value of the class attribute
        /// </summary>
        /// <returns>the class value of the title attribute of the element</returns>
        public string GetClassInvisible()
        {
            return GetAttributeInvisible("class");
        }

        /// <summary>
        /// gets the value of the class attribute aria-invalid
        /// </summary>
        /// <returns>the class value of the title attribute of the element</returns>
        public string GetAriaInvalidAttribute()
        {
            return GetAttribute("aria-invalid");
        }

        /// <summary>
        /// gets the value of the class attribute aria-label
        /// </summary>
        /// <returns>the class value of the title attribute of the element</returns>
        public string GetAriaLabelAttribute()
        {
            return GetAttribute("aria-label");
        }

        /// <summary>
        /// gets the value of the title attribute
        /// </summary>
        /// <param name="attr">attribute of the element</param>
        /// <returns>the value of the title attribute of the element</returns>
        public string GetAttribute(string attr)
        {
            return WaitForElementIsPresentAndGetIt().GetAttribute(attr);
        }

        /// <summary>
        /// gets the CSS value of the property
        /// </summary>
        /// <param name="propertyName">property name of the element</param>
        /// <returns>the CSS value of the property of the element</returns>
        public string GetCssValue(string propertyName)
        {
            return WaitForElementIsPresentAndGetIt().GetCssValue(propertyName);
        }

        /// <summary>
        /// get attribute value from element that is not displayed
        /// </summary>
        /// <param name="attr">attribute</param>
        /// <returns>value of attribute</returns>
        public string GetAttributeInvisible(string attr)
        {
            return WaitForElementExistsAndGetIt().GetAttribute(attr);
        }

        /// <summary>
        /// gets the value
        /// </summary>
        /// <returns>the value of the element</returns>
        public string GetValue()
        {
            return GetAttribute("value");
        }

        /// <summary>
        /// gets the value
        /// </summary>
        /// <returns>the value of the element</returns>
        public string GetValueInvisible()
        {
            return WaitForElementExistsAndGetIt().GetAttribute("value");
        }

        /// <summary>
        /// get the text of the element
        /// </summary>
        /// <returns>the text of the element</returns>
        public string GetText()
        {
            return WaitForElementIsPresentAndGetIt().Text;
        }

        /// <summary>
        /// get the text of the element Without Waiting
        /// </summary>
        /// <returns>the text of the element</returns>
        public string GetTextWithoutWaiting()
        {
            return Element.Text;
        }

        /// <summary>
        /// get the textContent of the element
        /// </summary>
        /// <returns>the textContent of the element</returns>
        public string GetTextContent()
        {
            return GetAttribute("textContent");
        }

        /// <summary>
        /// get the border color of the element by CSS value
        /// </summary>
        /// <returns>the border color of the element by CSS value</returns>
        public string GetBorderColorByCssValue()
        {
            return GetCssValue("border-color");
        }

        /// <summary>
        /// get the border Top color of the element by CSS value
        /// </summary>
        /// <returns>the border color of the element by CSS value</returns>
        public string GetBorderTopColorByCssValue()
        {
            return GetCssValue("border-top-color");
        }

        /// <summary>
        /// get the title of the element
        /// </summary>
        /// <returns>the textContent of the element</returns>
        public string GetTitleAttrubute()
        {
            return GetAttribute("title");
        }

        /// <summary>
        /// get the style of the element
        /// </summary>
        /// <returns>the textContent of the element</returns>
        public string GetStyleAttrubute()
        {
            return GetAttribute("style");
        }

        /// <summary>
        /// get the sortvalue of the element
        /// </summary>
        /// <returns>the textContent of the element</returns>
        public string GetSortValueAttrubute()
        {
            return GetAttribute("sortvalue");
        }

        /// <summary>
        /// get the color of the element by CSS value
        /// </summary>
        /// <returns>the color of the element by CSS value</returns>
        public string GetColorByCssValue()
        {
            return GetCssValue("color");
        }

        /// <summary>
        /// get the padding-left value of the element by CSS value
        /// </summary>
        /// <returns>the padding-left value of the element by CSS value</returns>
        public string GetPaddingLeftByCssValue()
        {
            return GetCssValue("padding-left");
        }

        /// <summary>
        /// focus on the element and send key ""
        /// </summary>
        public void FocusWithKeys()
        {
            Focus();
            try
            {
                Element.SendKeys("");
            }
            catch (Exception)
            {
                Info("Focused");
            }
        }

        /// <summary>
        /// focuses the element
        /// </summary>
        public void Focus()
        {
            Info("Focusing");
            WaitForElementIsPresent();
            new Actions(Browser.GetDriver()).MoveToElement(Element).Build().Perform();
        }

        /// <summary>
        /// focuses the element with mouse shift
        /// </summary>
        public void FocusWithMouseShift(int pixelX, int pixelY)
        {
            Info("Focusing");
            WaitForElementIsPresent();
            new Actions(Browser.GetDriver()).MoveToElement(Element, pixelX, pixelY).Build().Perform();
        }

        /// <summary>
        /// move the cursor to the element 
        /// </summary>
        public void MouseOver()
        {
            WaitForElementIsPresent();
            Info("Mouse over");
            new Actions(Browser.GetDriver()).MoveToElement(Element).Perform();
        }

        /// <summary>
        /// scroll the page
        /// </summary>
        public void ScrollThePage(int x, int y)
        {
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("window.scrollBy(" + x + "," + y + ");");
        }

        /// <summary>
        /// scroll the element to the position
        /// </summary>
        public void ScrollTo(int x, int y)
        {
            WaitForElementIsPresent();
            Info("Scrolling into view");
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].scrollTo(" + x + "," + y + ");", GetElement());
        }

        /// <summary>
        /// abstract method for get the type of the element 
        /// </summary>
        protected abstract string GetElementType();

        /// <summary>
        /// right click
        /// </summary>
        public void ClickRight()
        {
            WaitForElementIsPresent();
            Info("Right clicking");
            var action = new Actions(Browser.GetDriver());
            action.ContextClick(Element);
            action.Perform();
        }

        /// <summary>
        /// verify that the drop-down element is minimized (performed by a class member)
        /// </summary>
        /// <returns>true if collapsed</returns>
        public bool IsCollapsed()
        {
            return GetClass().ToLower().Contains("collapse");
        }

        /// <summary>
        /// set value via javascript <b>document.getElementById('{0}').value='{1}' </b>
        /// </summary>
        /// <param name="elementId">Element Id</param>
        /// <param name="value">Value</param>
        public void SetValueViaJs(string elementId, string value)
        {
            try
            {
                ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript(
                    $"document.getElementById('{elementId}').value='{value}'", Element);
            }
            catch (Exception r)
            {
                Logger.Instance.Warn(r.Message);
            }
        }

        /// <summary>
        /// set innerHtml via javascript <b>arguments[0].innerHTML='{0}' </b>
        /// </summary>
        /// <param name="value">value</param>
        public void SetInnerHtml(string value)
        {
            WaitForElementIsPresent();
            AssertIsPresent();
            Element.Click();
            Info("Ввод текста '" + value + "'");

            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].innerHTML=\"\";", Element);
            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].innerHTML=\"" + value + "\";", Element);
        }

        /// <summary>
        /// set value via javascript <b>arguments[0].value='{0}' </b>
        /// </summary>
        /// <param name="value"></param>
        public void SetValueViaJs(string value)
        {
            WaitForElementIsPresent();
            Element.Click();
            Info("Ввод текста '" + value + "'");

            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].value=\"\";", Element);

            ((IJavaScriptExecutor)Browser.GetDriver()).ExecuteScript("arguments[0].value=\"" + value + "\";", Element);
        }

        /// <summary>
        /// enum to set expected condition for explicit wait
        /// </summary>
        public enum ExpectedConditions
        {
            /// <summary>
            /// condition when element exists in the html source code
            /// </summary>
            ElementExists,
            /// <summary>
            /// condition when element exists in the html source code and visible now
            /// </summary>
            ElementIsVisible
        }

        /// <summary>
        /// set explicit wait
        /// </summary>
        /// <param name="condition">Expected condition for explicit wait</param>
        /// <param name="seconds">Explicit wait timeout</param>
        public void ExplicitWait(ExpectedConditions condition, int seconds)
        {
            try
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(seconds));
                if (condition.ToString().Equals("ElementExists", StringComparison.OrdinalIgnoreCase))
                {
                    Element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Locator));
                }
                else if (condition.ToString().Equals("ElementIsVisible", StringComparison.OrdinalIgnoreCase))
                {
                    Element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Locator));
                }
                else
                {
                    Logger.Instance.Info($"Unexpected ExpectedConditions value: {condition}");
                }
            }
            catch (TimeoutException ex)
            {
                Browser.GetDriver().Navigate().Refresh();
                if (!IsPresent())
                {
                    Logger.Instance.Fail(ex.Message + "\n" + FormatLogMsg(" is not Present"));
                }
            }
        }

        /// <summary>
        /// select all text by Ctrl+A
        /// </summary>
        public void SelectAllTextByCtrlA()
        {
            WaitForElementIsPresentAndGetIt().SendKeys(Convert.ToString('\u0001'));
        }

        /// <summary>
        /// clear field by selecting all chars and pressing Delete
        /// </summary>
        public void ClearFieldBySelectingAndDeleting()
        {
            WaitForElementIsPresent();
            SelectAllTextByCtrlA();
            Element.SendKeys(Keys.Delete);
        }

        /// <summary>
        /// clear field by pressing Backspace
        /// </summary>
        public void ClearFieldByCharsDeleting(int stringLength)
        {
            WaitForElementIsPresent();
            for (int i = 0; i < stringLength; i++)
            {
                Element.SendKeys(Keys.Backspace);
            }
        }

        /// <summary>
        /// clear field by selecting all chars and pressing Delete and pressing Backspace
        /// </summary>
        public void ClearFieldBySelectingPressingDelete(string text)
        {
            if (Browser.CurrentBrowser == BrowserFactory.BrowserType.Firefox)
            {
                new Actions(Browser.GetDriver()).MoveToElement(GetElement()).DoubleClick().Perform();
                ClearFieldByCharsDeleting(2 * text.Length);
            }
            else
            {
                ClearFieldByCharsDeleting(2 * text.Length);
                ClearFieldBySelectingAndDeleting();
            }
        }

        /// <summary>
        /// clear field and set text with delay
        /// </summary>
        /// <param name="text">text for set</param>
        public void SetTextWithDelayInCharTyping(string text)
        {
            List<string> list = ConvertTextToListOfTypedString(text);
            foreach (var letter in list)
            {
                Info($"Setting '{letter}' with delay");
                foreach (char t in letter)
                {
                    Element.SendKeys(t.ToString());
                    Browser.Sleep(TimeoutToDelayCharMs);
                }
            }
        }

        /// <summary>
        /// Convert Text To List Of Typed String
        /// </summary>
        /// <param name="text">text for set</param>
        protected List<string> ConvertTextToListOfTypedString(string text)
        {
            WaitForElementIsPresent();
            WaitForIsEnabled();
            Element.Clear();
            var charArray = text.ToCharArray();
            var list = new List<string>();
            string s = "";
            for (int index = 0; index < charArray.Length; index++)
            {
                s += charArray[index];
                if (s.ToCharArray().Length != 250 && index != charArray.Length - 1)
                {
                    continue;
                }

                list.Add(s);
                s = "";
            }
            return list;
        }
    }
}
