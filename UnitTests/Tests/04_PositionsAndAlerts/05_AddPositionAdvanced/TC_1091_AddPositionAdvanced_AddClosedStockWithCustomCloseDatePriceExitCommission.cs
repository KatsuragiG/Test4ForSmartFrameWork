using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.AddPosition;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._05_AddPositionAdvanced
{
    [TestClass]
    public class TC_1091_AddPositionAdvanced_AddClosedStockWithCustomCloseDatePriceExitCommission : BaseTestUnitTests
    {
        private const int TestNumber = 1091;

        private readonly List<PortfolioModel> portfoliosModels = new List<PortfolioModel>();
        private int positionsQuantity;
        private readonly List<AddPositionAdvancedModel> positionsModels = new List<AddPositionAdvancedModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioName = GetTestDataAsString("PortfolioName");
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                Currency = GetTestDataAsString("Currency1")
            });
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType2"),
                Currency = GetTestDataAsString("Currency2")
            });
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            var entryDate = GetTestDataAsString("EntryDate");
            var entryPrice = GetTestDataAsString("EntryPrice");
            var entryCommission = GetTestDataAsDecimal("EntryCommission");
            var exitDate = GetTestDataAsString("ExitDate");
            var exitPrice = GetTestDataAsString("ExitPrice");
            var exitCommission = GetTestDataAsString("ExitCommission");
            var shares = GetTestDataAsString("Shares");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new AddPositionAdvancedModel
                {
                    AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"AssetType{i}"),
                    Ticker = GetTestDataAsString($"Symbol{i}"),
                    EntryDate = entryDate,
                    EntryPrice = entryPrice,
                    EntryCommission = entryCommission,
                    IsLongTradeType = GetTestDataAsBool($"TradeType{i}"),
                    IsOpenStatusType = false,
                    ExitDate = exitDate,
                    ExitPrice = exitPrice,
                    ExitCommission = exitCommission,
                    IsAdjustByDividends = GetTestDataAsBool($"Adjust{i}"),
                    Notes = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Portfolio = i % 2 == 0 ? portfoliosModels[1].Name : portfoliosModels[0].Name
                });
                if (positionsModels.Last().AssetType == PositionAssetTypes.Option)
                {
                    positionsModels.Last().Contracts = shares; 
                }
                else
                {
                    positionsModels.Last().Shares = shares;
                }
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            new PortfolioGridsSteps().LoginCreatePortfoliosViaDbGetPortfoliosIds(UserModels.First(), portfoliosModels);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1091$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test ability adding a Closed Stock position with custom Entry Date, Entry Price And Exit Commission into manual portfolio Investment " +
            "and Watch Only. https://tr.a1qa.com/index.php?/cases/view/19232062")]
        [TestCategory("Smoke"), TestCategory("AddPositionPage"), TestCategory("ClosedPositionCard")]
        public override void RunTest()
        {
            LogStep(1, "Open Add Position Advanced page ");
            var positionsQueries = new PositionsQueries();
            var portfoliosQueries = new PortfoliosQueries();
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenAddPositionAdvanced();

            foreach (var positionModel in positionsModels)
            {
                LogStep(2, 3, "Enter and select from autocomplete symbol with $ currency.Select / Enter:- Entry Price: 1028.05;- Entry Date: 01 / 01 / 2017;" +
                    "- Entry Commission: 250;- Shares value;- Trade Type = *Long *;-Status: = *Closed *;-Close Date: Entry Date < Close Date;- Close Price: 3200;" +
                    "- Exit Commission: any positive value;- Adjust alerts by dividends: No");
                var addPositionAdvancedSteps = new AddPositionAdvancedSteps();
                addPositionAdvancedSteps.FillRequiredFieldsOnAddPositionAdvanced(positionModel);
                Assert.IsTrue(positionModel.AssetType.HasValue, "positionModel.AssetType == null");
                var currentAddPositionAdvancedModel = addPositionAdvancedSteps.GetCurrentAddPositionAdvancedClosedModel((PositionAssetTypes)positionModel.AssetType);

                LogStep(4, "Make sure currency sign match expectation for the fields:- Entry Price;-Entry Commission;-Close Price;-Exit Commission;");
                var currencySign = portfoliosModels[0].Currency.ParseAsEnumFromStringMapping<Currency>().GetDescription();
                Checker.IsTrue(currentAddPositionAdvancedModel.EntryPrice.Contains(currencySign), "Currency is not as expected for Entry Price");
                Checker.IsTrue(new AddPositionAdvancedForm().GetValueFromTextBoxField(AddPositionAdvancedFields.EntryCommission).Contains(currencySign),
                    "Currency is not as expected for Entry Commission");
                Checker.IsTrue(currentAddPositionAdvancedModel.ExitPrice.Contains(currencySign), "Currency is not as expected for Close Price");
                Checker.IsTrue(currentAddPositionAdvancedModel.ExitCommission.Contains(currencySign), "Currency is not as expected for Exit Commission");

                LogStep(5, "Click 'Save and Close' button.");
                addPositionAdvancedSteps.ClickSaveButton();
                var positionId = positionsQueries.SelectLastAddedPositionId(Parsing.ConvertToInt(
                    portfoliosQueries.SelectPortfolioDataByPortfolioNameUserModel(positionModel.Portfolio, UserModels.First()).PortfolioId));

                LogStep(6, 7, "Make sure data match expectation:- Symbol;-Shares;-Entry Date;-Entry Price;-L / S;-Entry Commission;- Exit Date;- Exit Price;- Exit Commission;" +
                    "- Adjust alerts by dividends. Make sure currency sign match expectation for the fields:- Entry Price;-Entry Commission;-Close Price;-Exit Commission");
                var positionCardForm = new PositionCardForm();
                var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
                var expectedTicker = positionModel.AssetType == PositionAssetTypes.Option ? currentAddPositionAdvancedModel.OptionVariant : positionModel.Ticker;
                var expectedSharesField = positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType();
                var expectedSharesValue = positionModel.AssetType == PositionAssetTypes.Option ? currentAddPositionAdvancedModel.Contracts : currentAddPositionAdvancedModel.Shares;
                Checker.CheckEquals(expectedTicker, new PositionCardForm().GetSymbol(), "Symbol is not as expected on Position Card");
                Checker.CheckEquals(positionModel.EntryDate, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                    $"Entry Date is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(currentAddPositionAdvancedModel.EntryPrice)), Constants.DefaultDecimalRounding),
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice))),
                    $"Entry Price is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(expectedSharesValue, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(expectedSharesField).Replace("-", string.Empty),
                    $"Shares is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(Parsing.ConvertToDouble(currentAddPositionAdvancedModel.EntryCommission.ToString()),
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission))),
                    $"Entry Commission is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(currentAddPositionAdvancedModel.IsLongTradeType, positionDetailsTabPositionCardForm.IsTradeTypeLong(), "Trade Type is not as expected on Position Card");
                Checker.CheckEquals(currentAddPositionAdvancedModel.ExitDate, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitDate),
                    $"Exit Date is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(currentAddPositionAdvancedModel.ExitPrice)), Constants.DefaultDecimalRounding),
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitPrice))),
                    $"Exit Price is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(currentAddPositionAdvancedModel.ExitCommission)),
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitCommission))),
                    $"Exit Commission is not as expected on Position Card for {expectedTicker}");
                Checker.CheckEquals(positionModel.IsAdjustByDividends,
                    positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.AdjustAlertsByDividends) == AdjustmentType.Adjusted.GetStringMapping(),
                    $"Adjust dividends '{positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.AdjustAlertsByDividends)}' " +
                        $"is not as expected on Position Card for {expectedTicker}");

                LogStep(8, "In DB: make sure data match expectations");
                var positionData = positionsQueries.SelectAllPositionData(positionId);
                Checker.CheckEquals(expectedTicker, new PositionsQueries().SelectSymbolBySymbolId(Parsing.ConvertToInt(positionData.SymbolId)), "Symbol is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(currentAddPositionAdvancedModel.EntryDate), Parsing.ConvertToShortDateString(positionData.PurchaseDate),
                    $"Entry Date is not as expected in DB for {expectedTicker}");
                Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(currentAddPositionAdvancedModel.EntryPrice)),
                    Parsing.ConvertToDouble(positionData.PurchasePrice), "Entry Price is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToDouble(expectedSharesValue), Parsing.ConvertToDouble(positionData.Shares), "Shares is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToDouble(currentAddPositionAdvancedModel.EntryCommission.ToString()), Parsing.ConvertToDouble(positionData.OpenFee),
                    $"Entry Commission is not as expected in DB for {expectedTicker}");
                Checker.CheckEquals(Parsing.ConvertToDouble(Constants.DecimalNumberWithIntegerPossibilityRegex.Match(currentAddPositionAdvancedModel.ExitPrice.Replace(",", string.Empty)).Value).ToString("N"),
                    Parsing.ConvertToDouble(positionData.ClosePrice).ToString("N"), "Exit Price is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(currentAddPositionAdvancedModel.ExitDate), Parsing.ConvertToShortDateString(positionData.CloseDate),
                    $"Exit Date is not as expected in DB for {expectedTicker}");
                var exitCommissionDb = currentAddPositionAdvancedModel.ExitCommission.Replace(",", string.Empty);
                Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(exitCommissionDb) == Constants.DefaultStringZeroIntValue
                        ? "0.00"
                        : Constants.DecimalNumberWithIntegerPossibilityRegex.Match(exitCommissionDb).Value),
                    Parsing.ConvertToDouble(positionData.CloseFee), "Exit Commission is not as expected in DB for {expectedTicker}");
                Checker.CheckEquals(currentAddPositionAdvancedModel.IsLongTradeType == true ? (int)PositionTradeTypes.Long : (int)PositionTradeTypes.Short,
                    Parsing.ConvertToInt(positionData.TradeType), "Trade Type is not as expected in DB for {expectedTicker}");
                Checker.CheckEquals(!positionModel.IsAdjustByDividends, Parsing.ConvertToBool(positionData.IgnoreDividend),
                    $"Adjust alerts by dividends is not as expected in DB for {expectedTicker}");

                mainMenuNavigation.OpenAddPositionAdvanced();
            }
        }
    }
}