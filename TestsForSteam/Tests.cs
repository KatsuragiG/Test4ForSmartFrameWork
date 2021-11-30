using NUnit.Framework;
using WebdriverFramework.Framework.WebDriver;
using PageForms.SteamForm;
using TestEnviroment.BrowserStart;
using Assert = NUnit.Framework.Assert;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PageForms.SteamSeachForm;

namespace TestsForSteam
{
    public class Tests : BaseTest
    {             
        private const int quantityOfGamesForWither = 10;
        private const int quantityOfGamesForFallout = 20;
        private int step = 1;
        private int game = 1;
        private string url;

        private readonly IDictionary<string, string> OrderByValue = new Dictionary<string, string>
        {
            {"english", "Lowest Price"},
            {"russian", "возрастанию цены"}
        };

        private readonly IDictionary<int, string> GameValue = new Dictionary<int, string>
        {
            {1, "Wither"},
            {2, "Fallout"}
        };

        private readonly IDictionary<int, string> PricesNullValues = new Dictionary<int, string>
        {
            {1, "Free"},
            {2, "Free To Play"},
            {3, "Free Demo"},
            {4, "Demo"},
            {5, "Free to Play" }
        };

        [SetUp]
        public void SetUp()
        {
            new BrowserStart().StartBrowserOnStartPage();
            url = Browser.GetDriver().Url.Split('=').Last();
            Browser.WaitForPageToLoad();            
        }

        [Test]
        public override void RunTest()
        {
            LogStep(1, 3, "Search the game and check Order by prices in grid");            
            StepsToDo(GameValue[game++], OrderByValue[url], quantityOfGamesForWither);            

            LogStep(4, 6, "Search the game and check Order by prices in grid");
            StepsToDo(GameValue[game++], OrderByValue[url], quantityOfGamesForFallout);
        }

        private void StepsToDo(string game, string orderValue, int quantity)
        {
            LogStep(step++, "Open Steam page and search the game");
            var stemForm = new Steam();
            stemForm.AssertIsOpen();
            Assert.IsTrue(stemForm.IsSearchBoxPresent(), "Search field is not present");
            stemForm.SetTextInSearchField(game);
            stemForm.ClickOnSearchButton();

            LogStep(step++, $"Open search page and select {orderValue} in drop-down");
            var searchForm = new SteamSearchForm();
            searchForm.AssertIsOpen();
            searchForm.SelectSortByValueInDropDown(orderValue);
            var prices = searchForm.GetPricesForResults();
            Assert.IsTrue(prices.Any(), $"There are no prices in results for {game}");

            LogStep(step++, "Catch the prices, convert format and check that prices is order by asc");
            var newPrices = new List<string>();
            ConvertPrice(prices, newPrices);
            Assert.IsTrue(newPrices.Any(), $"There are no prices in list after Convert for {game}");
            List<double> newPricesInt = new List<double>();
            newPricesInt = newPrices.ConvertAll(double.Parse);
            OrderByAsc(newPricesInt, quantity);
        }

        private List<string> ConvertPrice(List<string> oldPrice, List<string> newPrice)
        {
            for (int i = 0; i < oldPrice.Count; i++)
            {
                if (PricesNullValues.Values.ToList().Contains(oldPrice[i]))
                {
                    newPrice.Add("0");
                }
                else if (Regex.IsMatch(oldPrice[i], "\\$*\\$."))
                {                    
                    var values = Regex.Split(oldPrice[i].Replace("$", ""), @"\r\n").Last();
                    if (PricesNullValues.Values.ToList().Contains(values))
                    {
                        values = "0";
                    }
                    newPrice.Add(values);
                    
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
                //object orderValue = (list[i] <= list[i + 1]);
                //Checker.IsTrue(orderValue, "Values is not order by asc");
            }
        }

        [TearDown]
        public void Exit()
        {
            Browser.Quit();
        }
    }
}