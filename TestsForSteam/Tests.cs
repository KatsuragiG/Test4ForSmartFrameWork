using NUnit.Framework;
using WebdriverFramework.Framework.WebDriver;
using PageForms.SteamForm;
using PageForms.BrowserStart;
using Assert = NUnit.Framework.Assert;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestsForSteam
{
    public class Tests : BaseTest
    {
        private string gameWither = "Wither";
        private string gameFallout = "Fallout";
        private string orderByLatestPrice = "Lowest Price";
        private int quantityOfGamesForWither = 10;
        private int quantityOfGamesForFallout = 20;        

        [SetUp]
        public void SetUp()
        {
            new BrowserStart().StartBrowserOnStartPage();
            Browser.WaitForPageToLoad();
        }

        [Test]
        public override void RunTest()
        {
            LogStep(1, "Open Steam page and search");
            var stemForm = new Steam();
            stemForm.AssertIsOpen();
            Assert.IsTrue(stemForm.IsSearchBoxPresent(), "Search field is not present");
            stemForm.SetTextInSearchField(gameWither);
            stemForm.ClickOnSearchButton();

            LogStep(2, "Open search page and select Lowest Price in drop-down");
            var searchForm = new SteamSearchForm();
            searchForm.AssertIsOpen();            
            searchForm.SelectSortByValueInDropDown(orderByLatestPrice);
            var prices = searchForm.GetPricesForResults();
            Assert.IsTrue(prices.Any(), "There are no prices in results");

            LogStep(3, "Catch the prices and convert format and check that prices is order by asc");
            var newPrices = new List<string>();
            ConvertPrice(prices, newPrices);
            Assert.IsTrue(newPrices.Any(), "There are no prices in list");
            List<double> newPricesInt = newPrices.ConvertAll(double.Parse);
            OrderByAsc(newPricesInt, quantityOfGamesForWither);            

            LogStep(4, "Search the second game");
            Assert.IsTrue(stemForm.IsSearchBoxPresent(), "Search field is not present");
            stemForm.SetTextInSearchField(gameFallout);
            stemForm.ClickOnSearchButton();

            LogStep(5, "Order games for Lowest Price in drop-down");
            searchForm.SelectSortByValueInDropDown(orderByLatestPrice);
            prices = searchForm.GetPricesForResults();
            Assert.IsTrue(prices.Any(), "There are no prices in results");

            LogStep(6, "Catch the prices and convert format");
            newPrices = new List<string>();
            ConvertPrice(prices, newPrices);
            Assert.IsTrue(newPrices.Any(), "There are no prices in list");
            newPricesInt = newPrices.ConvertAll(double.Parse);
            OrderByAsc(newPricesInt, quantityOfGamesForFallout);
        }

        private List<string> ConvertPrice(List<string> oldPrice, List<string> newPrice)
        {
            for (int i = 0; i < oldPrice.Count; i++)
            {
                if (oldPrice[i].Contains("Free") || oldPrice[i].Contains("Free to Play") || oldPrice[i].Contains("Free Demo"))
                {
                    newPrice.Add("0");
                }
                else if (Regex.IsMatch(oldPrice[i], "\\$*\\$."))
                {                    
                    newPrice.Add(Regex.Split(oldPrice[i].Replace("$", ""), @"\r\n").Last());
                }
                else
                {
                    newPrice.Add(oldPrice[i].Replace("$", ""));
                }
            }            
            return newPrice;
        }

        private void OrderByAsc(List<double> list, int quantityOfGames)
        {
            for (int i = 0; i < quantityOfGames - 1; i++)
            {
                Assert.IsTrue(list[i] <= list[i + 1], "Values is not order by asc");
            }
        }

        [TearDown]
        public void Exit()
        {
            Browser.Quit();
        }
    }
}