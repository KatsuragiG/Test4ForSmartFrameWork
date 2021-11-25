using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1207_PortfolioLite_PositionsGrid_EditPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1207;

        private int positionsQuantity;
        private int editedPositionNumber;
        private PortfolioLitePositionModel changedPositionModels;
        private PortfolioLitePositionModel expectedPositionModels;
        private PortfolioLitePositionModel editablePositionModel;

        [TestInitialize]
        public void TestInitialize()
        {
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            editedPositionNumber = GetTestDataAsInt(nameof(editedPositionNumber));

            var positionsQueries = new PositionsQueries();
            var positionsModels = new List<PortfolioLitePositionModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var entryDate = GetTestDataAsString($"entryDate{i}");
                var entryPrice = GetTestDataAsString($"entryPrice{i}");
                var quantity = GetTestDataAsString($"quantities{i}");
                positionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = GetTestDataAsString($"symbolsToAdd{i}"),
                    BuyDate = string.IsNullOrEmpty(entryDate) ? null : entryDate,
                    BuyPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice,
                    Qty = string.IsNullOrEmpty(quantity) ? null : quantity,
                    IsLongType = GetTestDataAsBool($"IsLongType{i}")
                });
                positionsModels.Last().Currency = (Currency)positionsQueries.SelectSymbolCurrencyBySymbol(positionsModels.Last().Ticker);
            }

            editablePositionModel = positionsModels[editedPositionNumber - 1];

            var editedEntryDate = GetTestDataAsString("editedEntryDate");
            var editedEntryPrice = GetTestDataAsString("editedEntryPrice");
            var editedShares = GetTestDataAsString("editedShares");
            var editedIsLongType = GetTestDataAsString("editedIsLongType");
            changedPositionModels = new PortfolioLitePositionModel
            {
                BuyDate = string.IsNullOrEmpty(editedEntryDate) ? null : editedEntryDate,
                BuyPrice = string.IsNullOrEmpty(editedEntryPrice) ? null : editedEntryPrice,
                Qty = string.IsNullOrEmpty(editedShares) ? null : editedShares,
                IsLongType = string.IsNullOrEmpty(editedIsLongType)
                    ? editablePositionModel.IsLongType
                    : Parsing.ConvertToBool(editedIsLongType)
            };

            var expectedFinishEntryDate = GetTestDataAsString("expectedFinishEntryDate");
            var expectedFinishEntryPrice = GetTestDataAsString("expectedFinishEntryPrice");
            var expectedFinishQuantity = GetTestDataAsString("expectedFinishQuantity");
            var expectedFinishIsLongTradeType = GetTestDataAsBool("expectedFinishIsLongTradeType");
            var expectedSharesSign = expectedFinishIsLongTradeType ? string.Empty : Constants.MinusSign;
            expectedPositionModels = new PortfolioLitePositionModel
            {
                Ticker = editablePositionModel.Ticker,
                BuyDate = string.IsNullOrEmpty(expectedFinishEntryDate) ? Constants.NotAvailableAcronym : expectedFinishEntryDate,
                BuyPrice = string.IsNullOrEmpty(expectedFinishEntryPrice) ? Constants.NotAvailableAcronym : expectedFinishEntryPrice,
                Qty = string.IsNullOrEmpty(expectedFinishQuantity)
                    ? Constants.DefaultStringZeroDecimalValue
                    : $"{expectedSharesSign}{expectedFinishQuantity.ToFractionalString()}",
                Currency = editablePositionModel.Currency,
                Name = positionsQueries.SelectSymbolIdNameUsingSymbol(editablePositionModel.Ticker).SymbolName,
                IsLongType = expectedFinishIsLongTradeType
            };

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add positions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1207$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that user can edit position from the Positions Grid in the Portfolio Lite https://tr.a1qa.com/index.php?/cases/view/21116212")]
        public override void RunTest()
        {
            LogStep(1, "Click edit sign for defined in xls position");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            Checker.CheckEquals(positionsQuantity, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain all added positions");
            portfolioLiteMainForm.ClickActionPositionInGridByNumber(PortfolioLitePositionActionTypes.Edit, editedPositionNumber);
            Checker.IsTrue(portfolioLiteMainForm.IsTextboxPresentInGrid(PortfolioLiteColumnTypes.EntryDate, editedPositionNumber),
                $"Entry Date is not editable in grid for {editablePositionModel.Ticker}");
            Checker.IsTrue(portfolioLiteMainForm.IsTextboxPresentInGrid(PortfolioLiteColumnTypes.EntryPrice, editedPositionNumber),
                $"Entry Price is not editable in grid for {editablePositionModel.Ticker}");
            Checker.IsTrue(portfolioLiteMainForm.IsTextboxPresentInGrid(PortfolioLiteColumnTypes.Shares, editedPositionNumber),
                $"Quantity is not editable in grid for {editablePositionModel.Ticker}");

            LogStep(2, "Check that prefilled value in the editable fields match the expectations");
            var prefilledDataForEditedPosition = portfolioLiteMainForm.GetPositionDataAtEditing(editedPositionNumber);
            var expectedEntryDate = string.IsNullOrEmpty(editablePositionModel.BuyDate) ? string.Empty : editablePositionModel.BuyDate;
            Checker.CheckEquals(expectedEntryDate, prefilledDataForEditedPosition.EntryDate,
                "Entry Date is not prefilled correctly at position editing");
            var expectedPrice = string.IsNullOrEmpty(editablePositionModel.BuyDate)
                ? string.Empty
                : $"{editablePositionModel.Currency.GetDescription()}" +
                  $"{StringUtility.SetFormatFromSample(editablePositionModel.BuyPrice, prefilledDataForEditedPosition.EntryPrice)}";
            Checker.CheckEquals(expectedPrice, prefilledDataForEditedPosition.EntryPrice,
                "Entry Price is not prefilled correctly at position editing");
            var expectedShares = string.IsNullOrEmpty(editablePositionModel.Qty) ? "0" : editablePositionModel.Qty;
            Checker.CheckEquals(expectedShares, prefilledDataForEditedPosition.Shares,
                "Shares is not prefilled correctly at position editing");
            Checker.CheckEquals(editablePositionModel.IsLongType, portfolioLiteMainForm.IsBtnTradeTypeInGridActive(PositionTradeTypes.Long, editedPositionNumber),
                $"TradeType is not as original in grid for {editablePositionModel.Ticker}");

            LogStep(3, "Edit values according to xls");
            portfolioLiteMainForm.FillFieldsAtEditing(changedPositionModels, editedPositionNumber);
            prefilledDataForEditedPosition = portfolioLiteMainForm.GetPositionDataAtEditing(editedPositionNumber);
            Checker.CheckEquals(changedPositionModels.BuyDate ?? expectedEntryDate, prefilledDataForEditedPosition.EntryDate,
                "Entry Date is not shown correctly in grid after data editing before saving");
            var editedPrice = string.IsNullOrEmpty(changedPositionModels.BuyPrice)
                ? expectedPrice
                : $"{editablePositionModel.Currency.GetDescription()}{changedPositionModels.BuyPrice}";
            Checker.CheckEquals(editedPrice, prefilledDataForEditedPosition.EntryPrice,
                "Entry Price is not shown correctly in grid after data editing before saving");
            Checker.CheckEquals(changedPositionModels.Qty ?? expectedShares, prefilledDataForEditedPosition.Shares,
                "Shares is not shown correctly in grid after data editing before saving");
            Checker.CheckEquals(changedPositionModels.IsLongType, portfolioLiteMainForm.IsBtnTradeTypeInGridActive(PositionTradeTypes.Long, editedPositionNumber),
                $"TradeType is not after editing before saving in grid for {editablePositionModel.Ticker}");

            LogStep(4, "Save changes");
            portfolioLiteMainForm.ClickActionAtEditing(PortfolioLiteEditingActionTypes.Apply, editedPositionNumber);
            Checker.CheckEquals(positionsQuantity, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain expected positions after editing");

            LogStep(5, "Check in the grid that edited position has expected values");
            var actualDataForPosition = portfolioLiteMainForm.GetPositionGridDataByOrder(editedPositionNumber);
            var symbolAndName = actualDataForPosition.Ticker.Split('\r');
            Checker.CheckEquals(expectedPositionModels.Ticker, symbolAndName[0],
                $"Ticker in the grid is not matched expectation after editing: {actualDataForPosition.Ticker}");
            Checker.CheckEquals(expectedPositionModels.Name, symbolAndName[1].Replace("\n", string.Empty),
                $"Ticker name for {symbolAndName[0]} in the grid is not matched expectation");
            Checker.CheckEquals(expectedPositionModels.BuyDate, actualDataForPosition.EntryDate,
                $"Entry Date in the grid is not matched expectation after editing: {actualDataForPosition.EntryDate}");
            Checker.CheckEquals(expectedPositionModels.BuyPrice, actualDataForPosition.EntryPrice.Replace(",", ""),
                $"Entry Price in the grid is not matched expectation after editing: {actualDataForPosition.EntryPrice}");
            Checker.CheckEquals(expectedPositionModels.Qty, actualDataForPosition.Shares.Replace(",", ""),
                $"Shares in the grid is not matched expectation after editing: {actualDataForPosition.Shares}");
            portfolioLiteMainForm.ClickActionPositionInGridByNumber(PortfolioLitePositionActionTypes.Edit, editedPositionNumber);
            Checker.CheckEquals(expectedPositionModels.IsLongType, portfolioLiteMainForm.IsBtnTradeTypeInGridActive(PositionTradeTypes.Long, editedPositionNumber),
                $"TradeType is not after saving in grid for {editablePositionModel.Ticker}");
        }
    }
}