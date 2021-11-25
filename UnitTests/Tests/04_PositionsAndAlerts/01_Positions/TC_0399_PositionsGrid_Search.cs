using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0399_PositionsGrid_Search : BaseTestUnitTests
    {
        private const int TestNumber = 399;

        private readonly List<PositionsDBModel> stockModels = new List<PositionsDBModel>();
        private readonly List<PositionsDBModel> optionModels = new List<PositionsDBModel>();
        private List<string> searchTexts;
        private List<string> searchResults;
        private List<int> answersQuantity = new List<int>();
        private readonly List<int> positionsIds = new List<int>();
        private string positionTag;
        private string viewNameForAddedView;
        private int step;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("userType");
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            FillDataInStockModels();

            FillDataInOptionModels();

            viewNameForAddedView = StringUtility.RandomString(GetTestDataAsString(nameof(viewNameForAddedView)));
            positionTag = GetTestDataAsString(nameof(positionTag));

            searchTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(searchTexts));
            searchResults = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(searchResults));
            answersQuantity = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(answersQuantity))
                .Select(Parsing.ConvertToInt).ToList();

            LogStep(step, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var stockModel in stockModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, stockModel));
            }
            foreach (var optionModel in optionModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, optionModel);
            }

            positionsQueries.AddNewRowIntoUserTags(UserModels.First().TradeSmithUserId, positionTag);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, positionTag, positionsIds.First());

            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.ImportDagSiteInvestment01(true);

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());

            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.AddNewViewWithAllCheckboxesMarked(viewNameForAddedView);
            positionsTabForm.SelectView(viewNameForAddedView);
        }

        private void FillDataInOptionModels()
        {
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption1"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                Notes = GetTestDataAsString("NotesOption1")
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption2"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                PurchasePrice = GetTestDataAsString("EntryPrice2")
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption3"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Delisted}"
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption4"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Expired}"
            });
        }

        private void FillDataInStockModels()
        {
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock1"),
                TradeType = $"{(int)PositionTradeTypes.Long}"
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock2"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                Notes = GetTestDataAsString("NotesStock2")
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock3"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Delisted}"
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock4"),
                TradeType = $"{(int)PositionTradeTypes.Short}"
            });
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_399$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PositionsGrid")]
        [Description("The test checks correctness of filtration using search for Position grid https://tr.a1qa.com/index.php?/cases/view/19232691")]

        public override void RunTest()
        {
            LogStep(++step, "Type in search field any random text");
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.Search(searchTexts[0]);
            Checker.IsTrue(positionsTabForm.IsNoResultsFoundTextPresent(), "No results found text is not shown");

            LogStep(++step, "Click clear sign ");
            positionsTabForm.ClearSearchField();
            Checker.IsFalse(positionsTabForm.IsNoResultsFoundTextPresent(), "No results found text is shown");

            LogStep(++step, "Type in search field HPQ ");
            positionsTabForm.Search(stockModels[0].Symbol);
            var actualPositionsIds = positionsTabForm.GetPositionsIds();
            Checker.CheckEquals(answersQuantity[0], actualPositionsIds.Count, $"Grid does not contain two positions (step {step})");
            Checker.IsTrue(actualPositionsIds.Contains(positionsIds.First()) && actualPositionsIds.Contains(positionsIds.Last()),
                $"Grid does not contain positions HPQ* (step {step})");

            LogStep(++step, "Clear search field and Type in search field 210115p");
            var positionsSymbols = SearchPositionSymbols(searchTexts[1]);
            CheckSearchResults(step, searchTexts[1], answersQuantity[1], searchResults[0]);
            Checker.IsTrue(positionsSymbols.Contains(searchResults[1]), $"Grid does not contain positions {searchResults[1]} (step {step})");

            LogStep(++step, "Clear search field and Type in search field SPY210115P00180000");
            CheckSearchResults(step, optionModels[2].Symbol, answersQuantity[2], optionModels[2].Symbol);

            LogStep(++step, "Clear search field and Type in search field NDX201218C04800000");
            CheckSearchResults(step, optionModels[3].Symbol, answersQuantity[3], optionModels[3].Symbol);

            LogStep(++step, "Clear search field and Type in search field aPpLe");
            CheckSearchResults(step, searchTexts[3], answersQuantity[4], searchResults[2]);

            LogStep(step, "Clear search field and Type in search field unique position name from MSF.F");
            CheckSearchResults(step, positionsQueries.SelectSymbolIdNameUsingSymbol(stockModels[2].Symbol).SymbolName,
                answersQuantity[5], stockModels[2].Symbol);

            LogStep(++step, "Clear search field and Type in search field unique position name from MSFT210115P00140000");
            CheckSearchResults(step, positionsQueries.SelectSymbolIdNameUsingSymbol(optionModels[1].Symbol).SymbolName,
                answersQuantity[6], optionModels[1].Symbol);

            LogStep(++step, "Clear search field and Type in search field unique position notes from MSFT");
            CheckSearchResults(step, stockModels[1].Notes, answersQuantity[7], stockModels[1].Symbol);

            LogStep(++step, "Clear search field and Type in search field unique position notes from AAPL210115C00160000 ");
            CheckSearchResults(step, optionModels[0].Notes, answersQuantity[8], optionModels[0].Symbol);

            LogStep(++step, "Clear search field and Type in search field unique position tag from HPQ");
            CheckSearchResults(step, positionTag, answersQuantity[9], stockModels[0].Symbol);
        }

        private void CheckSearchResults(int stepNumber, string searchString, int resultsQuantity, string expectedResultTicker)
        {
            var positionsSymbols = SearchPositionSymbols(searchString);
            Checker.CheckEquals(resultsQuantity, positionsSymbols.Count, $"Grid does not contain one position (step {stepNumber})");
            Checker.IsTrue(positionsSymbols.Contains(expectedResultTicker), $"Grid does not contain position {expectedResultTicker} (step {stepNumber})");
        }

        private List<string> SearchPositionSymbols(string searchString)
        {
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClearSearchField();
            positionsTabForm.Search(searchString);
            var positionsSymbols = positionsTabForm.GetPositionColumnValuesWithoutAggregated(PositionsGridDataField.Ticker)
                .Select(t => t.Split('\r')[0]).ToList();

            return positionsSymbols;
        }
    }
}