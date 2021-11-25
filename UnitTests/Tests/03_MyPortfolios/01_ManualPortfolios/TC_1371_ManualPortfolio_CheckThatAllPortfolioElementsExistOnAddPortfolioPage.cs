using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._01_ManualPortfolios
{
    [TestClass]
    public class TC_1371_ManualPortfolio_CheckThatAllPortfolioElementsExistOnAddPortfolioPage : BaseTestUnitTests
    {
        private const int TestNumber = 1371;
        private const string CurrencySignColumn = "currencySign";
        private const string CurrencyColumn = "currency";
        private const double ExpectedBackButtonOffset = 0.1;
        private const string ExpectedCash = "300";

        private Dictionary<string, string> expectedCurrencies;
        private string createPortfolioHeaderName;

        [TestInitialize]
        public void TestInitialize()
        {
            createPortfolioHeaderName = GetTestDataAsString("PortfolioHeaderName");
            expectedCurrencies = CurrenciesInitialize();

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().OpenPortfolioCreationFormViaSelectionFlowPage(AddPortfolioTypes.Manual);
        }

        private Dictionary<string, string> CurrenciesInitialize()
        {
            var currencies = new Dictionary<string, string>();
            var tableColumns = TestContext.DataRow.Table.Columns;

            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(CurrencySignColumn))
                {
                    var index = column.ToString().Split(new[] { CurrencySignColumn }, StringSplitOptions.None).Last();
                    var currency = GetTestDataAsString($"{CurrencyColumn}{index}");
                    var currencySign = GetTestDataAsString($"{CurrencySignColumn}{index}");
                    currencies[currency] = currencySign;
                }
            }

            return currencies;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1371$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AddPortfolio")]
        [Description("Test for existing of all elements on the Add Portfolio page and its correct default state: https://tr.a1qa.com/index.php?/cases/view/19234164")]
        public override void RunTest()
        {
            LogStep(1, "Check that BACK button exists in the upper left corner");
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            var isBackButtonPresent = manualPortfolioCreationForm.IsBackButtonPresent();
            Checker.IsTrue(isBackButtonPresent, "BACK button is not displayed");

            if (isBackButtonPresent)
            {
                var backButtonLocation = manualPortfolioCreationForm.GetBackButtonLocation();
                var backButtonPaddingLeft = manualPortfolioCreationForm.GetBackButtonPaddingLeft().RegexNumbers();
                var backSectionSize = manualPortfolioCreationForm.GetBackSectionSize();
                Checker.IsTrue(backSectionSize.Width * ExpectedBackButtonOffset > backButtonPaddingLeft,
                    $"BACK button is not displayed in the upper left corner. Button location: {backButtonLocation.X + backButtonPaddingLeft}. Section width: {backSectionSize.Width}");
            }

            LogStep(2, $"Check that '{createPortfolioHeaderName}' label is displayed in portfolio information area");
            var isCreatePortfolioLabelPresent = manualPortfolioCreationForm.IsPortfolioHeaderPresent(createPortfolioHeaderName);
            Checker.IsTrue(isCreatePortfolioLabelPresent, $"'{createPortfolioHeaderName}' label is not displayed");

            var backSectionLocation = manualPortfolioCreationForm.GetBackSectionLocation();
            var createPortfolioLabelLocation = manualPortfolioCreationForm.GetPortfolioHeaderLocation(createPortfolioHeaderName);
            Checker.IsTrue(backSectionLocation.Y < createPortfolioLabelLocation.Y,
                $"'{createPortfolioHeaderName}' label is not displayed in portfolio information area");

            LogStep(3, "Check that Portfolio Name field and appropriate label are displayed");
            CheckPortfolioLabelAndFieldLocationIfExist(ManualPortfolioCreateInformation.PortfolioName.GetDescription());

            LogStep(4, "Check that 'Type:' label is displayed near radio-buttons for selecting portfolio type");
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioLabelPresent(ManualPortfolioCreateInformation.Type.GetDescription()),
                $"{ManualPortfolioCreateInformation.Type.GetDescription()} label is not displayed");

            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioTypeRadioButtonsFieldPresent(),
                "Type radio-buttons section is not displayed");

            var typeLabelLocation = manualPortfolioCreationForm.GetPortfolioLabelLocation(ManualPortfolioCreateInformation.Type.GetDescription());
            var typeRadioButtonsLocation = manualPortfolioCreationForm.GetPortfolioTypeRadioButtonsFieldLocation();
            var mainFieldSize = manualPortfolioCreationForm.GetPortfolioFieldMainSize(ManualPortfolioCreateInformation.Type.GetDescription());

            Checker.IsTrue(Math.Abs(typeLabelLocation.Y - typeRadioButtonsLocation.Y) < mainFieldSize.Height,
                $"{ManualPortfolioCreateInformation.Type} elements have different Y points. Label: {typeLabelLocation.Y}. Field: {typeRadioButtonsLocation.Y}");
            Checker.IsTrue(typeLabelLocation.X < typeRadioButtonsLocation.X,
                "Radio-buttons section to the left of the label");

            LogStep(5, "Check that 2 radio-buttons exist for Type");
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioTypePresent(PortfolioType.Investment),
                $"{PortfolioType.Investment} is not displayed");
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioTypePresent(PortfolioType.WatchOnly),
                $"{PortfolioType.WatchOnly} is not displayed");

            CheckSelectedType(PortfolioType.Investment);

            LogStep(6, "Click Watch Only radio-button");
            SelectAndCheckPortfolioType(PortfolioType.WatchOnly);

            LogStep(7, "Click Investment radio-button");
            SelectAndCheckPortfolioType(PortfolioType.Investment);

            LogStep(8, "Click Portfolio Settings link (Portfolio Settings are collapsed)");
            manualPortfolioCreationForm.ExpandPortfolioSettings(true);
            Checker.IsTrue(manualPortfolioCreationForm.GetPortfolioSettingsExpandingStatus(),
                "Portfolio Settings are collapsed");

            CheckPortfolioLabelAndDropdownLocationIfExist(PortfolioGridColumnTypes.Currency.GetDescription());
            CheckPortfolioLabelAndFieldLocationIfExist(ManualPortfolioCreateInformation.EntryCommission.GetDescription());
            CheckPortfolioLabelAndFieldLocationIfExist(ManualPortfolioCreateInformation.ExitCommission.GetDescription());
            CheckPortfolioLabelAndFieldLocationIfExist(ManualPortfolioCreateInformation.Cash.GetDescription());
            CheckPortfolioLabelAndFieldLocationIfExist(ManualPortfolioCreateInformation.Notes.GetDescription());

            LogStep(9, "Click Portfolio Settings link (Portfolio Settings are expanded)");
            manualPortfolioCreationForm.ExpandPortfolioSettings(false);
            Checker.IsFalse(manualPortfolioCreationForm.GetPortfolioSettingsExpandingStatus(), "Portfolio Settings are expanded");

            LogStep(10, 13, "Expand Portfolio Settings and check that Currency dropdown, " +
                "Entry Commission, Exit Commission and Cash fields contain necessary currencies");
            manualPortfolioCreationForm.ExpandPortfolioSettings(true);
            var addPortfoliosSteps = new AddPortfoliosSteps();
            foreach (var currency in expectedCurrencies)
            {
                var portfolioModel = new AddPortfolioManualModel
                {
                    Currency = currency.Key,
                    Cash = ExpectedCash
                };
                manualPortfolioCreationForm.FillPortfolioFields(portfolioModel);

                var actualPortfolioModel = addPortfoliosSteps.GetCurrentAddPortfolioManualModel();

                Checker.CheckEquals(portfolioModel.Currency, actualPortfolioModel.Currency, "'Currency' is not as expected");
                Checker.IsTrue(actualPortfolioModel.EntryCommission.Contains(currency.Value),
                    $"'Entry Commission' sign is not as expected. Actual: {actualPortfolioModel.EntryCommission}. Expected: {currency.Value}");
                Checker.IsTrue(actualPortfolioModel.ExitCommission.Contains(currency.Value),
                    $"'Exit Commission' sign is not as expected. Actual: {actualPortfolioModel.ExitCommission}. Expected: {currency.Value}");
                Checker.CheckEquals($"{currency.Value}{portfolioModel.Cash}", actualPortfolioModel.Cash,
                    "'Cash' is not as expected");
            }

            LogStep(14, "Check that Cancel button exists");
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioManualFlowActionsButtonPresent(PortfolioManualFlowActionsButton.Cancel),
                "Cancel button is not displayed");

            LogStep(15, "Check that Save Portfolio button exists");
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioManualFlowActionsButtonPresent(PortfolioManualFlowActionsButton.SavePortfolio),
                "Save Portfolio button is not displayed");
        }

        private void SelectAndCheckPortfolioType(PortfolioType portfolioType)
        {
            new ManualPortfolioCreationForm().SelectPortfolioType(portfolioType);
            CheckSelectedType(portfolioType);
        }

        private void CheckSelectedType(PortfolioType portfolioType)
        {
            Checker.CheckEquals(portfolioType,
                new ManualPortfolioCreationForm().GetSelectedType(),
                $"'{portfolioType.GetStringMapping()}' is not selected");
        }

        private void CheckPortfolioLabelAndFieldLocationIfExist(string labelName)
        {
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            var isLabelPresent = manualPortfolioCreationForm.IsPortfolioLabelPresent(labelName);
            var isFieldPresent = manualPortfolioCreationForm.IsPortfolioFieldPresent(labelName);
            Checker.IsTrue(isLabelPresent, $"{labelName} label is not displayed");
            Checker.IsTrue(isFieldPresent, $"{labelName} field is not displayed");

            if (isFieldPresent && isLabelPresent)
            {
                var labelLocation = manualPortfolioCreationForm.GetPortfolioLabelLocation(labelName);
                var fieldLocation = manualPortfolioCreationForm.GetPortfolioFieldLocation(labelName);
                var mainFieldSize = manualPortfolioCreationForm.GetPortfolioFieldMainSize(labelName);

                Checker.IsTrue(Math.Abs(labelLocation.Y - fieldLocation.Y) < mainFieldSize.Height,
                    $"{labelName} elements have different Y points. Label: {labelLocation.Y}. Field: {fieldLocation.Y}");
                Checker.IsTrue(labelLocation.X <= fieldLocation.X,
                    $"{labelName} field to the left of the label : {labelLocation.X} vs {fieldLocation.X}");
            }
        }

        private void CheckPortfolioLabelAndDropdownLocationIfExist(string labelName)
        {
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            var isLabelPresent = manualPortfolioCreationForm.IsPortfolioLabelPresent(labelName);
            var isFieldPresent = manualPortfolioCreationForm.IsPortfolioDropdownPresent(labelName);
            Checker.IsTrue(isLabelPresent, $"{labelName} label is not displayed");
            Checker.IsTrue(isFieldPresent, $"{labelName} dropdown is not displayed");

            if (isFieldPresent && isLabelPresent)
            {
                var labelLocation = manualPortfolioCreationForm.GetPortfolioLabelLocation(labelName);
                var fieldLocation = manualPortfolioCreationForm.GetPortfolioDropdownLocation(labelName);
                var mainFieldSize = manualPortfolioCreationForm.GetPortfolioFieldMainSize(labelName);

                Checker.IsTrue(Math.Abs(labelLocation.Y - fieldLocation.Y) < mainFieldSize.Height,
                    $"{labelName} elements have different Y points. Label: {labelLocation.Y}. Field: {fieldLocation.Y}");
                Checker.IsTrue(labelLocation.X <= fieldLocation.X,
                    $"{labelName} field to the left of the label: '{labelLocation.X} vs {fieldLocation.X}'");
            }
        }
    }
}