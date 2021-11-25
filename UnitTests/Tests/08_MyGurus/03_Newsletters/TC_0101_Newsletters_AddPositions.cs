using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Newsletters;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Helpers;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._08_MyGurus._03_Newsletters
{
    [TestClass]
    public class TC_0101_Newsletters_AddPositions : BaseTestUnitTests
    {
        private const int TestNumber = 101;

        private PortfolioDBModel portfolioModel;
        private AddToPortfolioSelectType addToPortfolioKind;
        private string portfolioNameToAdd;
        private string textAfterSaving;
        private bool isWithSsi;
        private readonly List<PositionsGridDataField> columnsToAddInView = new List<PositionsGridDataField>
        {
            PositionsGridDataField.EntryDate, PositionsGridDataField.EntryPrice, PositionsGridDataField.TradeType,
            PositionsGridDataField.Notes, PositionsGridDataField.Vq
        };
        private readonly List<PositionsGridDataField> columnsToCollectDataOnGrid = new List<PositionsGridDataField>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            isWithSsi = !ListsComparator.AreTwoListsEqualsNotInOrder(userProductSubscriptions, new List<ProductSubscriptions>
                { ProductSubscriptions.TradeStopsBasic, ProductSubscriptions.CryptoStopsBasic });
            if (isWithSsi)
            {
                columnsToAddInView.Add(PositionsGridDataField.Health);
            }

            columnsToCollectDataOnGrid.AddRange(columnsToAddInView);
            columnsToCollectDataOnGrid.AddRange(new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker, PositionsGridDataField.Name, PositionsGridDataField.Status
            });

            portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")}",
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            addToPortfolioKind = GetTestDataParsedAsEnumFromStringMapping<AddToPortfolioSelectType>(nameof(addToPortfolioKind));
            portfolioNameToAdd = addToPortfolioKind == AddToPortfolioSelectType.New ? StringUtility.RandomString(GetTestDataAsString("PortfolioName")) : portfolioModel.Name;
            textAfterSaving = GetTestDataAsString(nameof(textAfterSaving));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyGurus);
            new GurusMenuForm().ClickGurusMenuItem(GurusMenuItems.Newsletters);
            new MainMenuNavigation().OpenCustomPublisherGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_101$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232916 The test checks adding all possible positions from newsletters (custom publisher).")]
        [TestCategory("Smoke"), TestCategory("NewsLettersPage"), TestCategory("PositionsGrid")]
        public override void RunTest()
        {
            LogStep(1, "Make sure that positions grid is not empty");
            var selectedPublisherForm = new SelectedPublisherForm();
            selectedPublisherForm.SelectPublisherPortfolio(Constants.CustomPublisherPortfolioName);
            Assert.IsTrue(selectedPublisherForm.IsGridPresent(), "Custom publisher grid is not shown");
            new NewslettersSteps().SortGridByColumn(NewslettersGridColumnTypes.Ticker);
            var allPositionsQuantity = selectedPublisherForm.GetNumberOfPositionsInGrid();
            Assert.AreNotEqual(0, allPositionsQuantity, "Custom publisher grid does not contains any position");

            LogStep(2, "Remember data for all position");
            var allPositionsData = selectedPublisherForm.GetAllPositionsDataWithOrWithoutSsi(isWithSsi);
            var addablePositionsData = selectedPublisherForm.GetPositionsDataAddedToPortfolio(allPositionsData);

            LogStep(3, "Select all positions. Remember positions quantity to save");
            selectedPublisherForm.SelectAllItemsInGrid();
            for (int i = 0; i < allPositionsQuantity; i++)
            {
                Checker.CheckEquals(addablePositionsData.Contains(allPositionsData[i]), selectedPublisherForm.IsItemCheckboxByOrderEnabled(i + 1),
                    $"Ticker # {i + 1} has unexpected checkbox state");
            }
            Assert.IsTrue(selectedPublisherForm.IsAddToPortfolioButtonEnabled(), "Add To Portfolio is not shown");
            var addablePositionsQuantity = selectedPublisherForm.GetSelectedItemsNumberFromFooter();
            Checker.CheckEquals(addablePositionsData.Count, addablePositionsQuantity, "Addable positions count is not same as in footer");

            LogStep(4, "Select Portfolios type (new or existed) and portfolio Name according to test data");
            selectedPublisherForm.SelectAddToPortfolioItem(addToPortfolioKind);
            Checker.CheckEquals(addToPortfolioKind, selectedPublisherForm.GetAddToPortfolioItem(), "Expected portfolio type is not selected");
            if (addToPortfolioKind == AddToPortfolioSelectType.Existed)
            {
                selectedPublisherForm.SelectPortfolioToAdd(portfolioNameToAdd);
                Checker.CheckEquals(portfolioNameToAdd, selectedPublisherForm.GetSelectedPortfolioToAdd(), "Expected portfolio name is not typed for existed portfolio");
            }
            else
            {
                selectedPublisherForm.SetPortfolioName(portfolioNameToAdd);
                Checker.CheckEquals(portfolioNameToAdd, selectedPublisherForm.GetPortfolioNameForAdding(), "Expected portfolio name is not typed for new portfolio");
            }

            LogStep(5, "Click Add");
            selectedPublisherForm.ClickAddToPortfolioButton();
            Checker.CheckEquals(textAfterSaving, selectedPublisherForm.GetPortfolioActionLabelText(), "Text for saving result is not as expected");

            LogStep(6, "Open positions grid for Portfolio from step 4");
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioNameToAdd);
            var positionsTabForm = new PositionsTabForm();
            var addedPositionsQuantity = positionsTabForm.GetNumberOfRowsInGrid();
            Checker.CheckEquals(addablePositionsQuantity, addedPositionsQuantity, "Saved positions quantity is not as expected");

            LogStep(7, "Make sure data match expectation for all possible to add position");
            positionsTabForm.AddANewViewWithCheckboxesMarked(StringUtility.RandomString("View#######"), columnsToAddInView.Select(t => t.GetStringMapping()).ToList());
            var positionsData = positionsTabForm.GetPositionDataForAllPositions(columnsToCollectDataOnGrid);
            foreach (var addablePositionData in addablePositionsData)
            {
                var mappedModel = positionsData.FirstOrDefault(u => u.Ticker == addablePositionData.Ticker && u.EntryDate == addablePositionData.RefDate);
                if (mappedModel == null)
                {
                    Checker.Fail($"Position {addablePositionData.Ticker} from Publisher not found in Positions grid");
                }
                else
                {
                    Checker.CheckEquals(addablePositionData.Name, mappedModel.Name, "Positions names are not equals");

                    var gbpCurrencyCorrection = addablePositionData.RefPrice.EndsWith(Currency.GBX.GetDescription()) ? 100 : 1;
                    var refPrice = Parsing.ConvertToDouble(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(addablePositionData.RefPrice).Value) / gbpCurrencyCorrection;
                    var entryPrice = (Parsing.ConvertToDouble(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(mappedModel.EntryPrice).Value)).ToString("N8").TrimEnd('0').TrimEnd('.');
                    Checker.CheckEquals(StringUtility.SetFormatFromSample(refPrice.ToString(CultureInfo.InvariantCulture), entryPrice), entryPrice.Replace(",", string.Empty),
                        $"Entry Prices are not equals for {addablePositionData.Ticker}");

                    Checker.CheckEquals(addablePositionData.LS, mappedModel.TradeType, $"Trade Type are not equals for {addablePositionData.Ticker}");
                    if (addablePositionData.Advice == Constants.NotAvailableAcronym)
                    {
                        Checker.CheckEquals($"{EnumDisplayNamesHelper.Get(PublisherTypes.TestPublisher)}: {Constants.CustomPublisherPortfolioName}.",
                            mappedModel.Notes, $"Notes are not default for {addablePositionData.Ticker}");
                    }
                    else
                    {
                        Checker.CheckEquals($"{EnumDisplayNamesHelper.Get(PublisherTypes.TestPublisher)}: {Constants.CustomPublisherPortfolioName}. Advice: {addablePositionData.Advice}",
                            mappedModel.Notes, $"Notes are not equals for {addablePositionData.Ticker}");
                    }

                    Checker.CheckEquals(addablePositionData.VqPercent, mappedModel.Vq, $"Notes are not equals for {addablePositionData.Ticker}");
                    if (addablePositionData.Alert != Constants.NotAvailableAcronym)
                    {
                        Checker.IsTrue(mappedModel.Status.In(
                                ((long)StatusPositionColumnGridStates.ManualPositionsWithTriggeredAlerts).ToString(),
                                ((long)StatusPositionColumnGridStates.ManualPositionsWithoutTriggeredAlerts).ToString()
                            ), $"Alert Status {mappedModel.Status} are not equals for {addablePositionData.Ticker}");
                    }

                    if (isWithSsi)
                    {
                        Checker.CheckEquals((HealthZoneTypes)Parsing.ConvertToInt(addablePositionData.HealthState), mappedModel.Health,
                            $"Health for {addablePositionData.Ticker} is not as expected");
                    }
                }
            }
        }
    }
}