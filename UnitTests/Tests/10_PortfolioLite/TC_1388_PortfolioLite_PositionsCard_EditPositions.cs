using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Monads;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1388_PortfolioLite_PositionsCard_EditPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1388;

        private int quantityOfAddedPositions;
        private string currencySign;
        private string expectedAdjustment;
        private bool isPriceRetrievedViaGetQuote;
        private PortfolioLitePositionModel createdPositionModel;
        private PortfolioLitePositionModel changedPositionModel;
        private PortfolioLiteCardModel expectedPositionCardModel;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));

            PrepareCreatedPositionModel();

            PrepareEditedPositionModel();

            PrepareFinalPositionModel();

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add position");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPosition(createdPositionModel);
            portfolioLiteMainForm.ExitFrame();
        }

        private void PrepareFinalPositionModel()
        {
            var expectedFinishEntryDate = GetTestDataAsString("expectedFinishEntryDate");
            var expectedFinishEntryPrice = GetTestDataAsString("expectedFinishEntryPrice");
            var expectedFinishQuantity = GetTestDataAsString("expectedFinishQuantity");
            var expectedFinishIsLongTradeType = GetTestDataAsBool("expectedFinishIsLongTradeType");
            var expectedSharesSign = expectedFinishIsLongTradeType ? string.Empty : Constants.MinusSign;
            var hdSymbolStatisticsModel = positionDataQueries.SelectSymbolStatisticsForSymbol(createdPositionModel.Ticker);
            currencySign = createdPositionModel.Currency.GetDescription();
            expectedPositionCardModel = new PortfolioLiteCardModel
            {
                Ticker = createdPositionModel.Ticker,
                EntryDate = string.IsNullOrEmpty(expectedFinishEntryDate) ? Constants.NotAvailableAcronym : expectedFinishEntryDate,
                EntryPrice = string.IsNullOrEmpty(expectedFinishEntryPrice) ? Constants.NotAvailableAcronym : expectedFinishEntryPrice,
                Shares = string.IsNullOrEmpty(expectedFinishQuantity)
                    ? Constants.DefaultStringZeroDecimalValue
                    : $"{expectedSharesSign}{expectedFinishQuantity.ToFractionalString()}",
                Name = positionsQueries.SelectSymbolIdNameUsingSymbol(createdPositionModel.Ticker).SymbolName,
                LatestClose = $"{currencySign}{hdSymbolStatisticsModel.LatestClose}"
            };
            if (isPriceRetrievedViaGetQuote && expectedPositionCardModel.EntryPrice == Constants.NotAvailableAcronym)
            {
                expectedPositionCardModel.EntryPrice = $"{currencySign}{GetTradeAdjClosePrice(expectedPositionCardModel.EntryDate)}";
            }
            expectedAdjustment = changedPositionModel.IsLongType != null && (bool)changedPositionModel.IsLongType
                ? false.ToString()
                : true.ToString();
        }

        private void PrepareEditedPositionModel()
        {
            var editedEntryDate = GetTestDataAsString("editedEntryDate");
            var editedEntryPrice = GetTestDataAsString("editedEntryPrice");
            var editedShares = GetTestDataAsString("editedShares");
            var editedIsLongType = GetTestDataAsString("editedIsLongType");
            var editedIsGetQuoteClicked = GetTestDataAsString("editedIsGetQuoteClicked");
            changedPositionModel = new PortfolioLitePositionModel
            {
                BuyDate = string.IsNullOrEmpty(editedEntryDate) ? null : editedEntryDate,
                BuyPrice = string.IsNullOrEmpty(editedEntryPrice) ? null : editedEntryPrice,
                Qty = string.IsNullOrEmpty(editedShares) ? null : editedShares,
                IsLongType = string.IsNullOrEmpty(editedIsLongType)
                    ? createdPositionModel.IsLongType
                    : Parsing.ConvertToBool(editedIsLongType),
                IsGetQuoteClicked = !string.IsNullOrEmpty(editedIsGetQuoteClicked) && Parsing.ConvertToBool(editedIsGetQuoteClicked),
            };
            isPriceRetrievedViaGetQuote = string.IsNullOrEmpty(editedEntryPrice)
                && (bool)changedPositionModel.IsGetQuoteClicked;
        }

        private void PrepareCreatedPositionModel()
        {
            var entryDate = GetTestDataAsString("entryDate");
            var entryPrice = GetTestDataAsString("entryPrice");
            var quantity = GetTestDataAsString("quantities");
            createdPositionModel = new PortfolioLitePositionModel
            {
                Ticker = GetTestDataAsString("symbolsToAdd"),
                BuyDate = string.IsNullOrEmpty(entryDate) ? null : entryDate,
                BuyPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice,
                Qty = string.IsNullOrEmpty(quantity) ? null : quantity,
                IsLongType = GetTestDataAsBool("isLongType")
            };
            createdPositionModel.Currency = (Currency)positionsQueries.SelectSymbolCurrencyBySymbol(createdPositionModel.Ticker);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1388$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that user can edit position from the Positions Card in the Portfolio Lite https://tr.a1qa.com/index.php?/cases/view/21784296")]
        public override void RunTest()
        {
            LogStep(1, "Check that added position is listed in grid");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            Assert.AreEqual(quantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain added position");

            LogStep(2, "Click link for position in grid");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(quantityOfAddedPositions);

            LogStep(3, "Activate Position details tab");
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            var positionDetailsTab = portfolioLiteCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PortfolioLiteCardTabs.PositionDetails);

            LogStep(4, "Check that Editing positions is available");
            Checker.IsTrue(positionDetailsTab.IsEditPresent(), "Editing is not available");

            LogStep(5, "Click Edit");
            positionDetailsTab.EditPositionCard();
            Checker.IsTrue(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.EntryDate),
                $"Entry Date is not editable on position card for {createdPositionModel.Ticker}");
            Checker.IsTrue(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.EntryPrice),
                $"Entry Price is not editable on position card for {createdPositionModel.Ticker}");
            Checker.IsTrue(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.Shares),
                $"Shares is not editable on position card for {createdPositionModel.Ticker}");

            LogStep(6, "Check that prefilled value in the editable fields match the expectations");
            var expectedEntryDate = string.IsNullOrEmpty(createdPositionModel.BuyDate) ? string.Empty : createdPositionModel.BuyDate;
            Checker.CheckEquals(expectedEntryDate, positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate),
                "Entry Date is not prefilled correctly at position editing");

            DetectAutofilledPrices(positionDetailsTab, out string actualPrice, out string expectedPrice);
            Checker.CheckEquals(expectedPrice, actualPrice, "Entry Price is not prefilled correctly at position editing");

            var expectedShares = string.IsNullOrEmpty(createdPositionModel.Qty) ? Constants.DefaultStringZeroIntValue : createdPositionModel.Qty;
            Checker.CheckEquals(expectedShares, positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.Shares),
                "Shares is not prefilled correctly at position editing");
            Checker.CheckEquals(createdPositionModel.IsLongType, positionDetailsTab.GetTradeTypeInEdit() == PositionTradeTypes.Long,
                $"TradeType is not as original at position editing for {createdPositionModel.Ticker}");

            LogStep(7, "Edit values according to xls");
            EditValuesOnPositionCard(positionDetailsTab);

            Checker.CheckEquals(changedPositionModel.BuyDate ?? expectedEntryDate,
                positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate),
                "Entry Date is not shown correctly on position card after data editing before saving");

            DetectEditedPrices(positionDetailsTab, out actualPrice, expectedPrice, out string editedPrice);
            Checker.CheckEquals(editedPrice, actualPrice,
                "Entry Price is not shown correctly on position card after data editing before saving");
            Checker.CheckEquals(changedPositionModel.Qty ?? expectedShares, positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.Shares),
                "Shares is not shown correctly on position card after data editing before saving");
            Checker.CheckEquals(changedPositionModel.IsLongType, positionDetailsTab.GetTradeTypeInEdit() == PositionTradeTypes.Long,
                $"TradeType is not after editing before saving on position card for {expectedPositionCardModel.Ticker}");

            LogStep(8, "Save changes");
            portfolioLiteCardForm.ClickSave();
            Checker.IsFalse(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.EntryDate),
                $"Entry Date is editable on position card for {createdPositionModel.Ticker}");
            Checker.IsFalse(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.EntryPrice),
                $"Entry Price is editable on position card for {createdPositionModel.Ticker}");
            Checker.IsFalse(positionDetailsTab.IsPositionDetailsFieldEnabledForEditing(PositionDetailsFieldTypes.Shares),
                $"Shares is editable on position card for {createdPositionModel.Ticker}");

            LogStep(9, "Check saved values");
            var actualDataForPosition = portfolioLiteCardForm.GetCardData(expectedPositionCardModel);
            Checker.CheckEquals(expectedPositionCardModel.Ticker, actualDataForPosition.Ticker,
                $"Ticker on position card is not matched expectation after editing: {actualDataForPosition.Ticker}");
            Checker.CheckEquals(expectedPositionCardModel.Name, actualDataForPosition.Name,
                $"Ticker name for {actualDataForPosition.Ticker} on position card is not matched expectation for {expectedPositionCardModel.Ticker}");
            Checker.CheckEquals(expectedPositionCardModel.EntryDate, actualDataForPosition.EntryDate,
                $"Entry Date in the grid is not matched expectation after editing for {expectedPositionCardModel.Ticker}");

            Checker.CheckEquals(StringUtility.SetFormatFromSample(expectedPositionCardModel.EntryPrice, actualDataForPosition.EntryPrice),
                StringUtility.ReplaceAllCurrencySigns(actualDataForPosition.EntryPrice.Replace(",", "")),
                $"Entry Price on position card is not matched expectation after editing for {expectedPositionCardModel.Ticker}");
            Checker.CheckEquals(expectedPositionCardModel.Shares, actualDataForPosition.Shares.Replace(",", ""),
                $"Shares on position card is not matched expectation after editing for {expectedPositionCardModel.Ticker}");
            Checker.CheckEquals(expectedPositionCardModel.LatestClose, actualDataForPosition.LatestClose.Replace(",", ""),
                $"Latest Close on position card is not matched expectation after editing for {expectedPositionCardModel.Ticker}");
            Checker.CheckEquals(changedPositionModel.IsLongType, positionDetailsTab.IsTradeTypeLong(),
                $"TradeType is not after editing on position card for {expectedPositionCardModel.Ticker}");

            LogStep(10, "Check in DB that created position has expected values");
            var positionId = portfolioLiteCardForm.GetPositionIdFromElement();
            var positionFromDb = positionsQueries.SelectAllPositionData(positionId);
            Checker.CheckEquals(expectedPositionCardModel.EntryDate, DateTime.Parse(positionFromDb.PurchaseDate).ToString(Constants.ShortDateFormat),
                "Entry Date in DB is not matched expectation");
            var expectedDbPrice = Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(expectedPositionCardModel.EntryPrice)).ToString("#0.00000000");
            Checker.CheckEquals(expectedDbPrice, positionFromDb.SplitsAdj,
                "Entry Price in DB is not matched expectation");
            Checker.CheckEquals(expectedAdjustment, positionFromDb.IgnoreDividend,
                "Portfolio Lite Position adjustment in DB is not matched expectation");
            Checker.CheckEquals(Parsing.ConvertToDouble(expectedPositionCardModel.Shares).ToString("#0.00000000").DeleteMathSigns(),
                positionFromDb.Shares,
                "Shares in DB is not matched expectation");
        }

        private void DetectEditedPrices(PositionDetailsTabPositionCardForm positionDetailsTab, out string actualPrice, string expectedPrice, out string editedPrice)
        {
            actualPrice = positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryPrice);
            editedPrice = string.IsNullOrEmpty(changedPositionModel.BuyPrice)
                ? expectedPrice
                : $"{currencySign}{changedPositionModel.BuyPrice}";
            editedPrice = isPriceRetrievedViaGetQuote
                ? expectedPositionCardModel.EntryPrice
                : editedPrice;
        }

        private void DetectAutofilledPrices(PositionDetailsTabPositionCardForm positionDetailsTab, out string actualPrice, out string expectedPrice)
        {
            actualPrice = positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryPrice);
            var prefilledPriceAutomatically = !string.IsNullOrEmpty(createdPositionModel.BuyDate) && string.IsNullOrEmpty(createdPositionModel.BuyPrice)
                ? GetTradeAdjClosePrice(createdPositionModel.BuyDate).ToString(CultureInfo.InvariantCulture)
                : createdPositionModel.BuyPrice;
            expectedPrice = string.IsNullOrEmpty(createdPositionModel.BuyDate) && string.IsNullOrEmpty(createdPositionModel.BuyPrice)
                ? string.Empty
                : $"{currencySign}{StringUtility.SetFormatFromSample(prefilledPriceAutomatically, actualPrice)}";
        }

        private void EditValuesOnPositionCard(PositionDetailsTabPositionCardForm positionDetailsTab)
        {
            changedPositionModel.IsLongType.Do(p => positionDetailsTab.SelectTradeType(p != null && (bool)p ? PositionTradeTypes.Long : PositionTradeTypes.Short));
            changedPositionModel.BuyDate.Do(p => positionDetailsTab.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, p));
            if (isPriceRetrievedViaGetQuote)
            {
                positionDetailsTab.ClickGetQuote();
            }
            changedPositionModel.BuyPrice.Do(p => positionDetailsTab.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryPrice, p));
            changedPositionModel.Qty.Do(p => positionDetailsTab.SetValueInTextBoxField(PositionDetailsFieldTypes.Shares, p));
        }

        private decimal GetTradeAdjClosePrice(string date)
        {
            return positionDataQueries
                .SelectHdAdjPriceForSymbolIdDate(expectedPositionCardModel.Ticker, date)
                .GetTradeSplitOnlyAdjClose();
        }
    }
}