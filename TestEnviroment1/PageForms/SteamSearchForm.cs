using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageForms.SteamForm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestEnviroment.DropDown;

namespace PageForms.SteamSeachForm
{
    public class SteamSearchForm : Steam
    {
        private static readonly By titleLocator = By.XPath("//div[@class = 'page_content_ctn']");

        private const string sortBybox = "//div[contains(@class, 'sortbox')]";
        private const string searchResultsGrid = "//div[contains(@id, '_resultsRows')]";
        private readonly DropDown sortField = new DropDown(By.XPath($"{sortBybox}//div[contains(@class, 'dselect_container')]"), "Drop-down");
        private readonly By gridResults = By.XPath("//div[contains(@id, '_resultsRows')]");
        private readonly By resultsPrices = By.XPath("//div[contains(@id, '_resultsRows')]//div[contains(@class, 'price')]//div[contains(@class, 'price')]"); 

        public SteamSearchForm() : base(titleLocator, "Steam Main Menu Form")
        {
        }

        public void SelectSortByValueInDropDown(string text)
        {            
            sortField.SelectByText(text);
            Browser.Sleep(1000);
        }

        public void ClickOnDropDownSortBy()
        {
            sortField.ClickAndWaitForLoading();
            WaitForGridLoad();
        }

        public List<string> GetPricesForResults()
        {            
            var cbLabelList = (ReadOnlyCollection<IWebElement>)FindElements(resultsPrices);
            var cbLabelNames = cbLabelList.Select(t => t.Text.Trim()).ToList();

            return cbLabelNames.Where(x => x != string.Empty).ToList();
        }

        private void WaitForGridLoad()
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(10));
            wait.Until(d => (Browser.GetDriver().FindElements(gridResults).Count <= 0));
        }

    }
}