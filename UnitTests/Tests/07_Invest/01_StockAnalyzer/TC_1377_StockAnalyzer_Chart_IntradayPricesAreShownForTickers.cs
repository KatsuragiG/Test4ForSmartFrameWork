using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Enums.Tools.StockAnalyzer;

namespace UnitTests.Tests._07_Invest._01_StockAnalyzer
{
    [TestClass]
    public class TC_1377_StockAnalyzer_Chart_IntradayPricesAreShownForTickers : BaseTestUnitTests
    {
        private int daysMaxShiftForIntraday;
        private int symbolId;
        private readonly string latestTickLabel = MarketStatisticTypes.LatestPrice.GetStringMapping();
        private string login;
        private string password;

        [TestInitialize]
        public void TestInitialize()
        {
            symbolId = GetTestDataAsInt(nameof(symbolId));
            daysMaxShiftForIntraday = GetTestDataAsInt(nameof(daysMaxShiftForIntraday));
            login = GetTestDataAsString(nameof(login));
            password = GetTestDataAsString(nameof(password));

            LogStep(0, "Precondition");
            UserModels.Add(new UserModel { Email = login, Password = password });

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenLiveLoginForm();
            var loginForm = new LoginForm();
            loginForm.AssertIsOpen();

            loginForm.LogInWithoutDbWaiting(UserModels.First());
            loginForm.CloseWalkMePopupIfExists();
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            mainMenuNavigation.OpenLiveStockAnalyzerForSymbolId(symbolId);
            new StockAnalyzerForm().AssertIsOpen();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1377$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks that Intraday Prices Are Shown For BTC/USD and AAPL on PROD https://tr.a1qa.com/index.php?/cases/view/20809746")]
        [TestCategory("StockAnalyzer"), TestCategory("Chart")]
        public override void RunTest()
        {
            LogStep(1, "Click 1D chart period button");
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.CloseWalkMePopupIfExists();
            stockAnalyzerForm.ActivateTab(StockAnalyzerTabs.ChartSettings);
            stockAnalyzerForm.Chart.SelectChartPeriod(ChartPeriod.Intraday);
            stockAnalyzerForm.CloseWalkMePopupIfExists();
            var ticker = stockAnalyzerForm.GetSymbolTreeSelectSingleValue();
            Assert.AreEqual(ChartPeriod.Intraday, stockAnalyzerForm.Chart.GetCurrentPeriodButton(), $"Chart period is not as expected for {ticker}");

            LogStep(2, "Check that price line is shown");
            Assert.IsTrue(stockAnalyzerForm.Chart.IsChartLinePresent(ChartLineTypes.Price), $"Intraday on Stock Analyzer not shown for {ticker}");

            LogStep(3, "Check that hint in any chart point contains date tick not later than 2 days.");
            var currentSymbol = stockAnalyzerForm.GetSymbolTreeSelectSingleValue();
            var chartHint = stockAnalyzerForm.Chart.MoveCursorToChartGetTooltipWithExpectedLine(ChartLineTypes.Price);
            var wordsInHint = chartHint.DateForPoint.Split(' ');
            var dateInHint = DateTime.Parse($"{wordsInHint[0]} {wordsInHint[1]} {wordsInHint[2]}");
            Checker.IsTrue(dateInHint > DateTime.Now.AddDays(daysMaxShiftForIntraday), 
                $"Stock Analyzer date tick in hint {chartHint.DateForPoint} has old value for {ticker}");

            LogStep(4, "Check that stock-analyzer position details-block contains Latest Price as wording");
            if (currentSymbol.Contains(@"/"))
            {
                Checker.CheckContains(latestTickLabel, stockAnalyzerForm.GetLatestCloseLabelWording(), $"Latest Price Label is not as expected for {ticker}");
            }
        }

        [TestCleanup]
        public new void CleanAfterTest()
        {
            IsDeleteUserViaApi = false;
            base.CleanAfterTest();
        }
    }
}