using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Newsletters;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Timings;
using AutomatedTests.Elements;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.Tools.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Models;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._07_StockRating
{
    [TestClass]
    public class TC_1398_Invest_StockRating_AllExpectedElementsArePresent : BaseTestUnitTests
    {
        private const int TestNumber = 1398;

        private string ticker;
        private string pageHeader;
        private string billionairesPopupWording;
        private string newslettersPopupWording;
        private string strategiesPopupWording;
        private bool isBillionairesClickable;
        private bool isSectorAvailable;
        private bool isLikeFolioAvailable;
        private bool isHealthAvailable;
        private bool isTrendAvailable;
        private bool isTimingAvailable;
        private bool isRsiAvailable;
        private SpeedometerModel stockAnalyzerSpeedometerModel;
        private List<string> expectedRankingTypes;
        private List<string> expectedPageTabNames;
        private List<string> expectedPageSectionsNames;
        private List<string> expectedRatingFieldTypes;
        private readonly List<string> expectedPopupColumnHeaders = new List<string>();
        private readonly List<string> expectedStrategiesItems = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");
            ticker = GetTestDataAsString(nameof(ticker));
            pageHeader = GetTestDataAsString(nameof(pageHeader));
            billionairesPopupWording = GetTestDataAsString(nameof(billionairesPopupWording));
            newslettersPopupWording = GetTestDataAsString(nameof(newslettersPopupWording));
            strategiesPopupWording = GetTestDataAsString(nameof(strategiesPopupWording));

            isBillionairesClickable = GetTestDataAsBool(nameof(isBillionairesClickable));
            isSectorAvailable = GetTestDataAsBool(nameof(isSectorAvailable));
            isLikeFolioAvailable = GetTestDataAsBool(nameof(isLikeFolioAvailable));
            isHealthAvailable = GetTestDataAsBool(nameof(isHealthAvailable));
            isTrendAvailable = GetTestDataAsBool(nameof(isTrendAvailable));
            isTimingAvailable = GetTestDataAsBool(nameof(isTimingAvailable));
            isRsiAvailable = GetTestDataAsBool(nameof(isRsiAvailable));

            expectedPageTabNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPageTabNames));
            expectedPageSectionsNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPageSectionsNames));

            expectedStrategiesItems.AddRange(Constants.AvailableStrategyTypes.Select(strategy => strategy.GetStringMapping()));
            expectedRankingTypes = EnumUtils.GetValues<GlobalRatingTypes>()
                .Select(t => t.GetStringMapping().ToUpperInvariant()).ToList();
            expectedRatingFieldTypes = EnumUtils.GetValues<StockRatingFieldTypes>()
                .Select(t => t.GetStringMapping()).ToList();

            var expectedPopupHeaders = new List<GeneralTablesHeaders>
            {
                GeneralTablesHeaders.Publisher,
                GeneralTablesHeaders.Portfolio,
                GeneralTablesHeaders.RefDate,
                GeneralTablesHeaders.ChangeSinceRefDate
            };
            expectedPopupColumnHeaders.AddRange(expectedPopupHeaders.Select(t => t.GetStringMapping()));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().SetSymbol(ticker);

            var stockAnalyzerForm = new StockAnalyzerForm();
            Checker.IsTrue(stockAnalyzerForm.IsCarouselShown(), "Carousel is not shown");
            stockAnalyzerForm.SelectCarouselItem(AnalyzerCarouselPageTypes.StockRating);
            stockAnalyzerSpeedometerModel = stockAnalyzerForm.Speedometer.GetSpeedometerModel();
            stockAnalyzerForm.ClickDetailedOverview();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1398$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/22154473 The test checks Stock Rating page - that all elements are shown.")]
        [TestCategory("StockRating")]
        public override void RunTest()
        {
            LogStep(1, "Check that stock rating form is shown");
            var stockRatingForm = new StockRatingForm();
            stockRatingForm.AssertIsOpen();

            LogStep(2, $"Check that page header is 'Stock Rating overview for {ticker}'");
            Checker.CheckEquals(string.Format(pageHeader, ticker), stockRatingForm.GetPageHeader(),
                "Page header is not as expected");

            LogStep(3, "Check that speedometer is shown");
            Checker.IsTrue(stockRatingForm.Speedometer.IsExists(), "Speedometer is not shown");

            LogStep(4, "Check that speedometer has expected labels");
            var actualRankingTypes = stockRatingForm.Speedometer.GetAllSectorsDescriptions();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedRankingTypes, actualRankingTypes),
                $"Speedometer has unexpected labels inside:\n {GetExpectedResultsString(expectedRankingTypes)}\r\n{GetActualResultsString(actualRankingTypes)} for {ticker}");

            LogStep(5, "Check that speedometer has result under arrow according to FinalRank score ");
            var dbRankData = new SymbolsQueries().SelectRankDataForSymbol(ticker);
            Checker.IsTrue(dbRankData != null, $"Rank Db Data is not received from DB for {ticker}");

            var stockRatingSpeedometerModel = stockRatingForm.Speedometer.GetSpeedometerModel();
            Checker.IsTrue(stockRatingSpeedometerModel != null,
                $"Stock Rating Speedometer Model is not received from UI for {ticker}");
            Checker.CheckEquals(((GlobalRatingTypes)dbRankData.GlobalRank).GetStringMapping().ToUpperInvariant(),
                stockRatingSpeedometerModel.FinalStatus, $"Global Rating is not as expected for {ticker}");
            Checker.CheckEquals(stockAnalyzerSpeedometerModel, stockRatingSpeedometerModel, $"Speedometers are not equals for {ticker}");
            Checker.IsTrue(stockRatingSpeedometerModel.IsArrowAngleCorrespondsFinalStatus(), 
                $"Arrow angle {stockRatingSpeedometerModel.ArrowAngle} is not corresponds status {stockRatingSpeedometerModel.FinalStatus} for  {ticker}");

            LogStep(6, "Check that Stock Rating Details tab is shown");
            var actualTabsNames = stockRatingForm.GetTabsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPageTabNames, actualTabsNames),
                $"Stock Rating has unexpected tabs:\n {GetExpectedResultsString(expectedPageTabNames)}\r\n{GetActualResultsString(actualTabsNames)} for {ticker}");

            LogStep(7, "Check that 2 sections are shown: Fundamentals and Technicals");
            var actualSectionsNames = stockRatingForm.GetDataSectionsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPageSectionsNames, actualSectionsNames),
                 $"Stock Rating has unexpected sections:\n {GetExpectedResultsString(expectedPageSectionsNames)}\r\n{GetActualResultsString(actualSectionsNames)} for {ticker}");

            LogStep(8, "Check that 11 labels are shown");
            var actualFieldNames = stockRatingForm.GetFieldNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedRatingFieldTypes, actualFieldNames),
                 $"Stock Rating has unexpected fields:\n {GetExpectedResultsString(expectedRatingFieldTypes)}\r\n{GetActualResultsString(actualFieldNames)} for {ticker}");

            LogStep(9, "Check that if isBillionairesPopup = true - billionaires value are clickable. Close popup");
            var isFieldClickable = stockRatingForm.IsFieldValueClickable(StockRatingFieldTypes.Billionaires);
            Checker.CheckEquals(isBillionairesClickable, isFieldClickable,
                $"Billionaires clickability is not as expected for {ticker}");
            if (isBillionairesClickable && isFieldClickable)
            {
                stockRatingForm.ClickFieldValue(StockRatingFieldTypes.Billionaires);
                CheckBillionairePopup();
            }

            LogStep(10, "If Newsletters value is clickable - Open popup, check it and Close popup");
            isFieldClickable = stockRatingForm.IsFieldValueClickable(StockRatingFieldTypes.Newsletters);
            if (isFieldClickable)
            {
                stockRatingForm.ClickFieldValue(StockRatingFieldTypes.Newsletters);
                CheckNewslettersPopup();
            }

            LogStep(11, "Check if Strategies value is clickable and close popup after checking");
            isFieldClickable = stockRatingForm.IsFieldValueClickable(StockRatingFieldTypes.Strategies);
            if (isFieldClickable)
            {
                var strategiesQuantity = stockRatingForm.GetFieldValue(StockRatingFieldTypes.Strategies);
                stockRatingForm.ClickFieldValue(StockRatingFieldTypes.Strategies);
                CheckStrategiesPopup(Parsing.ConvertToInt(strategiesQuantity));
            }

            LogStep(12, "Check that all items have correct result from (Strong Bearish, Bearish, Neutral, Bullish, Strong Bullish) according to DB");
            CheckRatingsForAllFields(dbRankData);

            LogStep(13, "Check that EPS (TTM) has correct value from db");
            var symbolId = Parsing.ConvertToInt(new PositionsQueries().SelectSymbolIdNameUsingSymbol(ticker).SymbolId);
            var currencySign = ((Currency)new PositionsQueries().SelectSymbolCurrencyBySymbolId(symbolId)).GetDescription();
            var expectedEpsValue = GetFormattedEpsValue(dbRankData.EpsAndBasicAndTTM, currencySign);
            Checker.CheckEquals(expectedEpsValue, stockRatingForm.GetFieldValue(StockRatingFieldTypes.Eps),
                $"Eps value is not as expected for {ticker}");

            LogStep(14, "Check that ticker health has correct value from DB");
            CheckHealthValue();

            LogStep(15, "Check that Trend has correct value from DB");
            var expectedDbTrend = string.IsNullOrEmpty(dbRankData.TrendType)
                ? Constants.NotAvailableAcronym
                : ((HealthTrendZones)Parsing.ConvertToInt(dbRankData.TrendType)).GetDescription();
            var actualValue = stockRatingForm.GetFieldValue(StockRatingFieldTypes.Trend);
            Checker.CheckEquals(isHealthAvailable, !string.IsNullOrEmpty(actualValue),
                $"Trend is not as available due subscription for {ticker}");
            if (!string.IsNullOrEmpty(actualValue))
            {
                Checker.CheckEquals(expectedDbTrend, actualValue == Constants.NotAvailableAcronym
                        ? Constants.NotAvailableAcronym
                        : actualValue.ParseAsEnumFromDescription<HealthTrendZones>().GetDescription(),
                    $"Trend value is not as expected for {ticker}");
            }

            LogStep(16, "Check that Sector has correct value from DB");
            var dbBullBear = dbRankData.SectorIndicator == null
                ? Constants.NotAvailableAcronym
                : ((BullBearIndicatorStatuses)Parsing.ConvertToInt(dbRankData.SectorIndicator)).ToString();
            actualValue = stockRatingForm.GetFieldValue(StockRatingFieldTypes.Sector);
            Checker.CheckEquals(isSectorAvailable, !string.IsNullOrEmpty(actualValue),
                $"Sector is not as expected due to subscriptions for {ticker}");
            if (!string.IsNullOrEmpty(actualValue))
            {
                Checker.IsTrue(actualValue.Contains(dbBullBear),
                    $"Sector value is not as expected for {ticker}: '{actualValue}' on UI, {dbBullBear} in DB");
            }

            LogStep(17, "Check that Timing has correct value from DB");
            var timingDbModel = new TimingsQueries().SelectNearestTimingModelBySymbolId(symbolId);
            var expectedTiming = timingDbModel != null
                ? timingDbModel.GetTimingDirectionTypes().GetStringMapping()
                : Constants.NotAvailableAcronym;
            actualValue = stockRatingForm.GetFieldValue(StockRatingFieldTypes.Timing);
            Checker.CheckEquals(isTimingAvailable, !string.IsNullOrEmpty(actualValue),
                $"Timing is not as expected due to subscriptions for {ticker}");
            if (!string.IsNullOrEmpty(actualValue))
            {
                Checker.CheckEquals(expectedTiming, actualValue,
                    $"Timing value is not as expected for {ticker}");
            }

            LogStep(18, "Check that LikeFolio has correct value from DB");
            var expectedLikeFolio = dbRankData.LikeFolioStatus != null
                ? ((LikeFolioSentimentTypes)Parsing.ConvertToInt(dbRankData.LikeFolioStatus)).ToString()
                : Constants.NotAvailableAcronym;
            actualValue = stockRatingForm.GetFieldValue(StockRatingFieldTypes.LikeFolio);
            Checker.CheckEquals(isLikeFolioAvailable, !string.IsNullOrEmpty(actualValue),
                $"LikeFolio is not as expected due to subscriptions for {ticker}");
            if (!string.IsNullOrEmpty(actualValue))
            {
                Checker.IsTrue(actualValue.Contains(expectedLikeFolio),
                    $"LikeFolio value is not as expected for {ticker}: '{actualValue}' on UI, {expectedLikeFolio} in DB");
            }

            LogStep(19, "Check that F-Score has correct value from DB");
            var dbFscore = dbRankData.FScoreValue ?? Constants.NotAvailableAcronym;
            Checker.CheckEquals(dbFscore, stockRatingForm.GetFieldValue(StockRatingFieldTypes.FScore),
                $"F-Score value is not as expected for {ticker}");

            LogStep(20, "Check that RSI has correct value from DB");
            var dbRsi = dbRankData.RsiValue ?? Constants.NotAvailableAcronym;
            actualValue = stockRatingForm.GetFieldValue(StockRatingFieldTypes.Rsi);
            Checker.CheckEquals(isRsiAvailable, !string.IsNullOrEmpty(actualValue),
                $"RSI is not as expected due to subscriptions for {ticker}");
            if (!string.IsNullOrEmpty(actualValue))
            {
                Checker.CheckEquals(dbRsi.ToFractionalString(), actualValue,
                    $"RSI value is not as expected for {ticker}");
            }
        }

        private void CheckHealthValue()
        {
            var dbLongAdjDsi = new SymbolsQueries().SelectAnalyzedDataforSymbol(ticker).LongAdjDsi;
            var dbHealth = dbLongAdjDsi == null
                ? HealthZoneTypes.NotAvailable.ToString()
                : ((HealthZoneTypes)Parsing.ConvertToInt(dbLongAdjDsi)).ToString();
            var actualValue = new StockRatingForm().GetFieldValue(StockRatingFieldTypes.Health);
            Checker.CheckEquals(isHealthAvailable, !string.IsNullOrEmpty(actualValue),
                $"Health value is not as available due subscription for {ticker}");
            if (actualValue == ChartLineTypes.YellowZone.GetStringMapping())
            {
                Checker.IsTrue(dbHealth.In(
                        HealthZoneTypes.YellowDown.ToString(), HealthZoneTypes.YellowSide.ToString(), HealthZoneTypes.YellowUp.ToString()),
                    $"Health value is not as expected for {ticker} in Yellow zone");
            }
            else if (!string.IsNullOrEmpty(actualValue))
            {
                var uiHealth = Dictionaries.TypeOfHealthTypeReverse.GetValueByPartOfTheKeyOrDefault(actualValue,
                    defaultValue: HealthZoneTypes.NotAvailable).ToString();
                Checker.CheckEquals(dbHealth, uiHealth, $"Health value is not as expected for {ticker}");
            }
        }

        private string GetFormattedEpsValue(string epsAndBasicAndTTM, string currencySign)
        {
            string expectedEpsValue;
            if (string.IsNullOrEmpty(epsAndBasicAndTTM))
            {
                expectedEpsValue = Constants.NotAvailableAcronym;
            }
            else
            {
                expectedEpsValue = epsAndBasicAndTTM.Contains(Constants.MinusSign)
                ? $"{Constants.MinusSign}{currencySign}{epsAndBasicAndTTM.ToFractionalString().DeleteMathSigns()}"
                : $"{currencySign}{epsAndBasicAndTTM.ToFractionalString()}";
            }

            return expectedEpsValue;
        }

        private void CheckRatingsForAllFields(RankDbDataModel dbRankData)
        {
            var expectedRating = ((GlobalRatingTypes)dbRankData.BillionaireRank).GetStringMapping();
            var stockRatingForm = new StockRatingForm();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Billionaires),
                $"Billionaires rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.NewsletterRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Newsletters),
                $"Newsletters rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.EpsRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Eps),
                $"Eps rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.LongAdjHealthRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Health),
                $"Health rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.HealthTrendRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Trend),
                $"Trend rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.LongAdjSectorRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Sector),
                $"Sector rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.TimingRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Timing),
                $"Timing rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.StrategyRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Strategies),
                $"Strategies rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.LikeFolioRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.LikeFolio),
                $"LikeFolio rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.RsiRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.Rsi),
                $"RSI rating is not as expected for {ticker}");
            expectedRating = ((GlobalRatingTypes)dbRankData.FScoreRank).GetStringMapping();
            Checker.CheckEquals(expectedRating, stockRatingForm.GetFieldRating(StockRatingFieldTypes.FScore),
                $"F-Score rating is not as expected for {ticker}");
        }

        private void CheckBillionairePopup()
        {
            var billionairesPopup = new BillionairesPopup();
            billionairesPopup.AssertIsOpen();

            Checker.CheckEquals(billionairesPopupWording, billionairesPopup.GetContentInfoText(),
                $"Billionaires description is not as expected for {ticker}");

            var gridTable = billionairesPopup.PopupTable;

            var rowsQuantity = CheckHeadersAndRowsQuantityAndGetQuantity(gridTable);

            var expectedColumnData = Enumerable.Repeat(GurusMenuItems.BillionairesClub.GetStringMapping(), rowsQuantity).ToList();
            var actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.Publisher);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedColumnData, actualColumnData),
                $"Billionaires publishers values is not as expected\n {GetExpectedResultsString(expectedColumnData)}\r\n{GetActualResultsString(actualColumnData)} for {ticker}");

            expectedColumnData = new NewsLettersQueries().SelectBillionairePublisherNames();
            actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.Portfolio);
            foreach (var billionaire in actualColumnData)
            {
                Checker.IsTrue(expectedColumnData.Contains(billionaire),
                    $"Billionaires portfolio {billionaire} is not as expected in: \n {GetExpectedResultsString(expectedColumnData)} for {ticker}");
            }

            CheckRefDateAndChangesColumns(gridTable);

            billionairesPopup.ClickCrossButton();
        }

        private void CheckNewslettersPopup()
        {
            var newslettersPopup = new NewslettersPopup();
            newslettersPopup.AssertIsOpen();

            Checker.CheckEquals(string.Format(newslettersPopupWording, ticker), newslettersPopup.GetContentInfoText(),
                $"Newsletters description is not as expected for {ticker}");

            var gridTable = newslettersPopup.PopupTable;
            CheckHeadersAndRowsQuantityAndGetQuantity(gridTable);

            var actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.Publisher);
            foreach (var actualPublisher in actualColumnData)
            {
                Checker.IsTrue(Constants.NewslettersPublishersNames.Contains(actualPublisher),
                    $"'{actualPublisher}' is not included into Newsletters  for {ticker}: '" +
                    $"{Constants.NewslettersPublishersNames.Aggregate(string.Empty, (current, value) => current + "\n" + value)}");
            }

            actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.Portfolio);
            foreach (var portfolioName in actualColumnData)
            {
                Checker.IsTrue(!string.IsNullOrEmpty(portfolioName),
                    $"Newsletters portfolio {portfolioName} is not as expected for {ticker}");
            }

            CheckRefDateAndChangesColumns(gridTable);

            newslettersPopup.ClickCrossButton();
        }

        private void CheckStrategiesPopup(int expectedStrategiesQuantity)
        {
            var strategiesPopup = new StrategiesPopup();
            strategiesPopup.AssertIsOpen();

            Checker.CheckEquals(string.Format(strategiesPopupWording, ticker), strategiesPopup.GetContentInfoText(),
                "Strategies description is not as expected for {ticker}");

            var actualStrategies = strategiesPopup.GetListViewItems();
            Checker.IsTrue(actualStrategies.Any(), $"{ticker} has not strategies");
            Checker.CheckEquals(expectedStrategiesQuantity, actualStrategies.Count,
                $"Unexpected strategies quantity for {ticker}");
            var invalidStrategies = actualStrategies.Where(e => !expectedStrategiesItems.Contains(e)).ToList();
            Checker.IsFalse(invalidStrategies.Any(),
                $"{ticker} has invalid strategies: {string.Join(", ", invalidStrategies)}");

            strategiesPopup.ClickCrossButton();
        }

        private int CheckHeadersAndRowsQuantityAndGetQuantity(EmbeddedTableElement gridTable)
        {
            Checker.IsTrue(gridTable.IsExists(), $"Popup does not contain table for {ticker}");

            var rowsQuantity = gridTable.GetModalTableRowsQuantity();
            Checker.IsTrue(rowsQuantity > 0, $"Popup table does not contain rows for {ticker}");

            var actualHeaders = gridTable.GetTableColumnNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPopupColumnHeaders, actualHeaders),
                $"Popup headers is not as expected\n {GetExpectedResultsString(expectedPopupColumnHeaders)}\r\n{GetActualResultsString(actualHeaders)} for {ticker}");

            return rowsQuantity;
        }

        private void CheckRefDateAndChangesColumns(EmbeddedTableElement gridTable)
        {
            var actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.RefDate);
            foreach (var date in actualColumnData)
            {
                Checker.IsTrue(!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out _),
                    $"Popup Ref Date {date} is not as expected for {ticker}");
            }

            actualColumnData = gridTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.ChangeSinceRefDate);
            foreach (var priceChange in actualColumnData)
            {
                Checker.IsTrue(Constants.ChangeSinceRefDatePatternRegex.IsMatch(priceChange),
                    $"Popup Change Since Ref Date {priceChange} is not as expected for {ticker}");
            }
        }
    }
}