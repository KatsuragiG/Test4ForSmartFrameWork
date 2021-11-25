using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms.Settings.Position;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._09_UsersMenu._01_Settings._04_Positions
{
    [TestClass]
    public class TC_0453_Settings_Positions_SelectPortfolioCurrencyAndGoToPositionPage : BaseTestUnitTests
    {
        private const int TestNumber = 453;

        private int portfolioNamesQuantity;
        private readonly List<string> portfolioNamesList = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioNamesQuantity = GetTestDataAsInt(nameof(portfolioNamesQuantity));
            for (int i = 0; i < portfolioNamesQuantity; i++)
            {
                portfolioNamesList.Add(StringUtility.RandomString(GetTestDataAsString("PortfolioName")));
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            new SettingsSteps().LoginNavigateToSettingsPositionGetForm(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_453$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness of default currency of Portfolio which was selected on Settings page. https://tr.a1qa.com/index.php?/cases/view/19232666")]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SettingsPagePositionsTab"), TestCategory("AddPortfolio")]
        public override void RunTest()
        {
            CheckCurrency(Currency.CAD, portfolioNamesList[0]);
            CheckCurrency(Currency.USD, portfolioNamesList[1]);
            CheckCurrency(Currency.EUR, portfolioNamesList[2]);
            CheckCurrency(Currency.GBP, portfolioNamesList[3]);
            CheckCurrency(Currency.AUD, portfolioNamesList[4]);
            CheckCurrency(Currency.BTC, portfolioNamesList[5]);
        }

        private void CheckCurrency(Currency currency, string portfolioName)
        {
            LogStep(1, $"Select Portfolio Currency '{currency}' on drop-down and click 'Save'.");
            new PositionSettingForm().SetCurrency(currency.ToString());
            var settingsMainForm = new SettingsMainForm();
            settingsMainForm.SaveSettings();

            LogStep(2, "Open 'Portfolios' page. Click 'Add Portfolio' -> 'Manual'.");
            var addPortfoliosSteps = new AddPortfoliosSteps();
            addPortfoliosSteps.OpenPortfolioCreationFormViaSelectionFlowPageExpandPortfolioSettings(AddPortfolioTypes.Manual);

            LogStep(3, $"Make sure that currency '{currency}' shown by default equal to chosen and saved on step #1.");
            var addPortfolioManualTab = addPortfoliosSteps.GetCurrentAddPortfolioManualModel();
            Checker.CheckEquals(currency.ToString(), addPortfolioManualTab.Currency, "Currency is not shown by default equal to chosen and saved on step #1.");

            LogStep(4, "Fill in required fields with any valid data:- Name: any text;- Other values by default and click 'Save'.");
            addPortfoliosSteps.FillPortfolioNameClickSave(portfolioName);
            var portoflioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPortfolios();

            LogStep(5, $"Make sure that currency '{currency}' on 'Currency' column equal to chosen and saved on step #1.");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.OpenPortfolioTabWithPortfolioId(portoflioId);
            var portfolioRowOrder = portfoliosForm.GetPortfoliosIds().FindIndex(a => a == portoflioId);
            Checker.CheckEquals(currency.ToString(),
                portfoliosForm.GetPortfoliosGridRow(portfolioRowOrder + 1, new List<PortfolioGridColumnTypes> { PortfolioGridColumnTypes.Currency }).Currency,
                "Currency on 'Currency' column is not equal to chosen and saved on step #1.");
            mainMenuNavigation.OpenSettings();
            settingsMainForm.ClickSettingsItem(SettingsSectionTypes.Positions);
        }
    }
}