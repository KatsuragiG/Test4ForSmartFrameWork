using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_0922_AlertsGrid_DataInTheExportedFileMatchedTheGrid : BaseTestUnitTests
    {
        private const int TestNumber = 922;

        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private int expectedNumberOfColumns;
        private int positionsQuantity;
        private string fileName;
        private string viewNameForAddedView;
        private List<AlertsGridColumnsDataField> expectedColumns;
        private int year = 1900;

        [TestInitialize]
        public void TestInitialize()
        {
            positionsQuantity = GetTestDataAsInt("PositionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = ((int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}")).ToString(CultureInfo.InvariantCulture),
                    CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
                });
            }
            positionsModels.First().Notes = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField);
            expectedNumberOfColumns = GetTestDataAsInt("expectedNumberOfColumns");
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            expectedColumns =  userType == ProductSubscriptionTypes.TSBasic 
                ? Instance.GetListOfAllBasicAlertColumns()
                : Instance.GetListOfAllPremiumAlertColumns();
            expectedColumns.Add(AlertsGridColumnsDataField.Name);

            fileName = GetTestDataAsString("fileName");
            viewNameForAddedView = StringUtility.RandomString("########");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            DoPrecondition();

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenAlertsGrid();
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.AddNewViewWithAllCheckboxesMarked(viewNameForAddedView);
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            alertsTabForm.ScrollToLastRow();
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.AlertState, SortingStatus.Desc);
        }

        private void DoPrecondition()
        {
            var portfolioId = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            var positionsIds = positionsModels.Select(positionModel => PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel)).ToList();
            var stocks = new List<int> { positionsIds[0], positionsIds[1] };
            var longOptions = new List<int> { positionsIds[2] };

            foreach (var stock in stocks)
            {
                //VolMA
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentOfAverageVolume}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    Period = "1",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.AboveBelowMovingAverage}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    PriceType = $"{(int)PriceType.Close}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    Period = "1",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    AveragePrice = "140",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture),
                    NumTriggered = (SRandom.Instance.Next()).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.MovingAverageCrosses}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    Operation = $"{(int)OperationType.Below}",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    PeriodType2 = $"{(int)DatePeriodTypes.Day}",
                    ThresholdValue = "1",
                    Period = "1",
                    Period2 = "2",
                    AveragePrice = "140",
                    AveragePrice2 = "140",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                //Time
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.CalendarDaysAfterEntry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.TradingDaysAfterEntry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture),
                    NumTriggered = (SRandom.Instance.Next()).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.ProfitableClosesAfterEntry}",
                    PriceType = $"{(int)ClosesAfterEntryTypes.Opens}",
                    ThresholdValue = "1",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.SpecificDate}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdDate = $"{DateTime.Now}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    ExtremumDate = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                //Price
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageGainLoss}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = Constants.TsDefaultPercent.ToString(),
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.DollarGainLoss}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.FixedPriceAboveBelow}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    PeriodType = $"{(int)DatePeriodTypes.Month}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                //Fundamentals
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.MarketCap}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture),
                    NumTriggered = (SRandom.Instance.Next()).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.EnterpriseValue}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.EnterpriseValueRevenue}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString()
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.EnterpriseValueEbitda}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.PriceBook}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.PriceEarnings}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Target}",
                    ColumnName = $"{(int)FundamentalAlertTypes.Peg}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });

                //ts
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.VqTrailingStop}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(stock, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageTrailingStop}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "43",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
            }

            foreach (var longOption in longOptions)
            {
                //VolMA
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentOfAverageVolume}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    Period = "1",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.AboveBelowMovingAverage}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    PriceType = $"{(int)PriceType.Close}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    Period = "1",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    AveragePrice = "140",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.MovingAverageCrosses}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    Operation = $"{(int)OperationType.Below}",
                    PeriodType = $"{(int)DatePeriodTypes.Day}",
                    PeriodType2 = $"{(int)DatePeriodTypes.Day}",
                    ThresholdValue = "1",
                    Period = "1",
                    Period2 = "2",
                    AveragePrice = "140",
                    AveragePrice2 = "140",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                //Time
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.CalendarDaysAfterEntry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.TradingDaysAfterEntry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.ProfitableClosesAfterEntry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    PriceType = $"{(int)ClosesAfterEntryTypes.Opens}",
                    ThresholdValue = "1",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.SpecificDate}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdDate = $"{DateTime.Now}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    Operation = $"{(int)DaysAfterTypes.Only}",
                    ExtremumDate = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });

                //ts
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageTrailingStop}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = Constants.TsDefaultPercent.ToString(),
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });

                //Price
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageGainLoss}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = Constants.TsDefaultPercent.ToString(),
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.DollarGainLoss}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.FixedPriceAboveBelow}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Below}",
                    PriceType = $"{(int)PriceType.Close}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });

                //Time Value / Expiry
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageTimeValue}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    InitialTimeValue = "104",
                    LatestTimeValue = "1",
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.DollarTimeValue}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    InitialTimeValue = "104",
                    LatestTimeValue = "1",
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.Expiry}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    InitialTimeValue = "104",
                    LatestTimeValue = "1",
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                //Underlying stock
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.PercentageTrailingStopUnderlying}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
                PositionsAlertsSetUp.AddAlertViaDB(longOption, new AlertsDbModel
                {
                    TriggerTypeId = $"{(int)AlertTypes.UnStockVolatilityQuotinent}",
                    IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                    ThresholdValue = "1",
                    Operation = $"{(int)OperationType.Above}",
                    FirstTimeTriggered = $"01/01/{year++}",
                    LastTriggered = DateTime.Now.ToShortDateString(),
                    CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
                });
            }
            PositionsAlertsSetUp.AddAlertViaDB(positionsIds[4], new AlertsDbModel
            {
                TriggerTypeId = $"{(int)AlertTypes.NakedPutCostBasis}",
                IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.TriggeredAlert}",
                ThresholdValue = "1",
                CostBasisValue = "1",
                Operation = $"{(int)OperationType.Above}",
                PriceType = $"{(int)PriceType.Close}",
                FirstTimeTriggered = $"01/01/{year++}",
                LastTriggered = DateTime.Now.ToShortDateString(),
                CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
            });
            PositionsAlertsSetUp.AddAlertViaDB(positionsIds[5], new AlertsDbModel
            {
                TriggerTypeId = $"{(int)AlertTypes.CoveredCallCostBasis}",
                IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                ThresholdValue = "1",
                CostBasisValue = "1",
                CurrentValue = "1",
                ThresholdValueMoney = "1",
                Operation = $"{(int)OperationType.Above}",
                PriceType = $"{(int)PriceType.Close}",
                FirstTimeTriggered = $"01/01/{year++}",
                LastTriggered = DateTime.Now.ToShortDateString()
            });
            PositionsAlertsSetUp.AddAlertViaDB(stocks[0], new AlertsDbModel
            {
                TriggerTypeId = $"{(int)AlertTypes.TwoVq}",
                IsTriggered = $"{(int)StatusOfAlertOnPositionGridStates.UntriggeredAlert}",
                ThresholdValue = "1",
                FirstTimeTriggered = $"01/01/{year++}",
                LastTriggered = DateTime.Now.ToShortDateString(),
                CurrentValue = (SRandom.Instance.NextDouble() * 100).ToString(CultureInfo.InvariantCulture)
            });
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_922$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks matching data between Alerts grid and exported csv file. https://tr.a1qa.com/index.php?/cases/view/19232189")]
        [TestCategory("Smoke"), TestCategory("AlertsGrid"), TestCategory("Export")]
        public override void RunTest()
        {
            LogStep(1, "Remember data for Positions grid");
            var alertsTabForm = new AlertsTabForm();
            var alertsData = alertsTabForm.GetAlertsInformation(expectedColumns);
            Assert.IsTrue(alertsData.Count > 0, "There is no rows in the alerts grid");

            LogStep(2, "Click export");
            alertsTabForm.ClickGridActionButton(GridActionsButton.Export);
            var path = $"{GetDownloadedFilePathGridDepended()}{fileName}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");

            LogStep(3, "Make sure data in the exported file matched the Positions grid");
            var alertsCsvData = FileUtilsExtension.ParseCsvIntoObjects<AlertGridModel>(path, expectedNumberOfColumns);
            FileUtilsExtension.DeleteFileGridDepended(path);
            Assert.IsTrue(alertsCsvData.Count == alertsData.Count, "Quantity of rows in grid and CSV are not matched");
            foreach (var model in alertsCsvData)
            {
                var correspondedModel = alertsData.FirstOrDefault(u => u.FirstTimeTriggered == model.FirstTimeTriggered);
                if (correspondedModel == null)
                {
                    Checker.Fail($"No records with FirstTimeTriggered = {model.FirstTimeTriggered}");
                }
                else
                {
                    var unequalValues = new AlertGridModel().GetModelsDifferenceDescription(Dictionaries.AlertsColumnsAndObjProperties.Values.ToList(), model, correspondedModel);
                    Checker.IsTrue(unequalValues.Count == 0, unequalValues.Aggregate("", (current, unequalValue) => current + unequalValue + "\n"));
                }
            }
        }
    }
}