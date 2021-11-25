using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.Sorting;
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
    public class TC_1392_PortfolioLite_Grid_CheckSorting : BaseTestUnitTests
    {
        private const int TestNumber = 1392;

        private int step;
        private int quantityOfAddedPositions;
        private readonly List<PortfolioLitePositionModel> positionsModels = new List<PortfolioLitePositionModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));
            for (int i = 1; i <= quantityOfAddedPositions; i++)
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
            }

            LogStep(step++, "Preconditions. Create user with subscription to PortfolioLite. Add position");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            Checker.CheckEquals(quantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain added position");
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1392$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite"), TestCategory("Sorting")]
        [Description("Test checks that grid sorting works as expected https://tr.a1qa.com/index.php?/cases/view/21116197")]
        public override void RunTest()
        {
            LogStep(step++, "Check that default sorting is Ticker ASC.");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();

            CheckTickerColumnSorting(portfolioLiteMainForm);
            CheckDateColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.EntryDate);
            CheckMoneyAndDoubleValuesColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.EntryPrice);
            CheckMoneyAndDoubleValuesColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.Shares);
            CheckMoneyAndDoubleValuesColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.LatestClose);
            CheckDoubleColumnSortingWithLeadingNa(portfolioLiteMainForm, PortfolioLiteColumnTypes.Value);
            CheckMoneyAndDoubleValuesColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.DailyGain);
            CheckTotalGainColumnSorting(portfolioLiteMainForm, PortfolioLiteColumnTypes.TotalGain);
        }

        private void CheckTickerColumnSorting(PortfolioLiteMainForm portfolioLiteMainForm)
        {
            Checker.CheckEquals(SortingStatus.Asc, portfolioLiteMainForm.GetSortingDirectionForColumn(PortfolioLiteColumnTypes.Ticker),
                $"Sorting for column  {PortfolioLiteColumnTypes.Ticker.GetStringMapping()} is not Ticker {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            var values = portfolioLiteMainForm.GetColumnValues(PortfolioLiteColumnTypes.Ticker);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsInOrder(values, Sorting.SortStringListAsc(values)),
                $"Column {PortfolioLiteColumnTypes.Ticker.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Click on 'Ticker' column. Check that sorting sing is DESC.");
            portfolioLiteMainForm.ClickColumnInGrid(PortfolioLiteColumnTypes.Ticker);
            Checker.CheckEquals(SortingStatus.Desc, portfolioLiteMainForm.GetSortingDirectionForColumn(PortfolioLiteColumnTypes.Ticker),
                $"Sorting for column  {PortfolioLiteColumnTypes.Ticker.GetStringMapping()} is not Ticker {SortingStatus.Desc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            values = portfolioLiteMainForm.GetColumnValues(PortfolioLiteColumnTypes.Ticker);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsInOrder(values, Sorting.SortStringListDesc(values)),
                $"Column {PortfolioLiteColumnTypes.Ticker.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Desc.GetStringMapping()}");
        }

        private void CheckDateColumnSorting(PortfolioLiteMainForm portfolioLiteMainForm, PortfolioLiteColumnTypes column)
        {
            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is ASC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Asc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column {column.GetStringMapping()} is not Ticker {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            var values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(OrderChecker.CheckOrderOfDateValuesInColumnWithDate(values, SortingStatus.Asc.ToString()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is DESC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Desc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Desc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(OrderChecker.CheckOrderOfDateValuesInColumnWithDate(values, SortingStatus.Desc.ToString()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Desc.GetStringMapping()}");
        }

        private void CheckMoneyAndDoubleValuesColumnSorting(PortfolioLiteMainForm portfolioLiteMainForm, PortfolioLiteColumnTypes column)
        {
            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is ASC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Asc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            var values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingAscIntList(values.Select(Sorting.GetOrderNumberOfColumWithNegativeNaNullPositive).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is DESC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Desc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Desc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (DESC) matched the expectations.");
            values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingDescIntList(values.Select(Sorting.GetOrderNumberOfColumWithNegativeNaNullPositive).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Desc.GetStringMapping()}");
        }

        private void CheckDoubleColumnSortingWithLeadingNa(PortfolioLiteMainForm portfolioLiteMainForm, PortfolioLiteColumnTypes column)
        {
            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is ASC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Asc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            var values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingAscIntList(values.Select(Sorting.GetOrderNumberOfColumWithNaNegativeNullPositive).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is DESC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Desc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Desc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (DESC) matched the expectations.");
            values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingDescIntList(values.Select(Sorting.GetOrderNumberOfColumWithNaNegativeNullPositive).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Desc.GetStringMapping()}");
        }

        private void CheckTotalGainColumnSorting(PortfolioLiteMainForm portfolioLiteMainForm, PortfolioLiteColumnTypes column)
        {
            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is ASC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Asc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (ASC) matched the expectations.");
            var values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingAscIntList(values.Select(Sorting.GetOrderNumberOfColumWithNegativeNullPositiveIgnoringNa).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Asc.GetStringMapping()}");

            LogStep(step++, $"Click on '{column.GetStringMapping()}' column. Check that sorting sing is DESC.");
            portfolioLiteMainForm.ClickColumnInGrid(column);
            Checker.CheckEquals(SortingStatus.Desc, portfolioLiteMainForm.GetSortingDirectionForColumn(column),
                $"Sorting for column  {column.GetStringMapping()} is not Ticker {SortingStatus.Desc.GetStringMapping()}");

            LogStep(step++, "Make sure sorting (DESC) matched the expectations.");
            values = portfolioLiteMainForm.GetColumnValues(column);
            Checker.IsTrue(Sorting.CheckSortingDescIntList(values.Select(Sorting.GetOrderNumberOfColumWithNegativeNullPositiveIgnoringNa).ToList()),
                $"Column {column.GetStringMapping()} sorting is not matched the expectations for {SortingStatus.Desc.GetStringMapping()}");
        }
    }
}