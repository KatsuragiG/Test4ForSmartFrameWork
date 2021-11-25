using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.Alerts;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0344_PositionsGrid_CheckCombineFiltersApplying : BaseTestUnitTests
    {
        private const int TestNumber = 344;

        private int defaultPositionsQuantity;
        private int greenPositionsQuantity;
        private int issuesPositionsQuantity;
        private int delistedPositionsQuantity;
        private int expiredPositionsQuantity;
        private int untriggeredPositionsQuantity;
        private int triggeredPositionsQuantity;
        private int noAlertsPositionsQuantity;
        private int synchedPositionsQuantity;
        private int manualPositionsQuantity;

        private int combineFiltersQuantity;
        private string tsPercent;
        private string templateName;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();

        private readonly List<string> expectedAssetTypes = EnumUtils.GetValues<SymbolTypes>()
            .Except(new List<SymbolTypes> { SymbolTypes.SeasonalContracts, SymbolTypes.Bond })
            .Select(t => t.ToString()).ToList();
        private readonly List<string> expectedPositionTypes = EnumUtils.GetValues<PositionTradeTypes>()
            .Select(t => t.GetStringMapping()).ToList();
        private readonly List<string> expectedTypes = EnumUtils.GetValues<PortfolioSyncTypes>()
            .Select(t => t.ToString()).ToList();
        private readonly List<string> expectedPositionStatuses = EnumUtils.GetValues<PositionsStatusTypes>()
            .Select(t => t.GetDescription()).ToList();
        private readonly List<string> expectedAlertStatuses = EnumUtils.GetValues<AlertFilterTypes>()
            .Select(t => t.GetStringMapping()).ToList();

        private readonly List<PositionsFiltersModel> combineFiltersModels = new List<PositionsFiltersModel>();
        private List<int> combinePositionsQuantity = new List<int>();
        private readonly Dictionary<PositionsIssuesTypes, int> issuesQuantity = new Dictionary<PositionsIssuesTypes, int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            var tickersList = GetTestDataValuesAsListByColumnName("ticker");
            for (int i = 1; i <= tickersList.Count; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = tickersList[i - 1],
                    TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}")}",
                    PurchasePrice = GetTestDataAsString($"entryPrice{i}"),
                    PurchaseDate = GetTestDataAsString($"entryDate{i}")
                });
            }

            templateName = GetTestDataAsString(nameof(templateName));
            tsPercent = GetTestDataAsString(nameof(tsPercent));

            defaultPositionsQuantity = GetTestDataAsInt(nameof(defaultPositionsQuantity));
            greenPositionsQuantity = GetTestDataAsInt(nameof(greenPositionsQuantity));
            issuesPositionsQuantity = GetTestDataAsInt(nameof(issuesPositionsQuantity));
            delistedPositionsQuantity = GetTestDataAsInt(nameof(delistedPositionsQuantity));
            expiredPositionsQuantity = GetTestDataAsInt(nameof(expiredPositionsQuantity));
            untriggeredPositionsQuantity = GetTestDataAsInt(nameof(untriggeredPositionsQuantity));
            triggeredPositionsQuantity = GetTestDataAsInt(nameof(triggeredPositionsQuantity));
            noAlertsPositionsQuantity = GetTestDataAsInt(nameof(noAlertsPositionsQuantity));
            synchedPositionsQuantity = GetTestDataAsInt(nameof(synchedPositionsQuantity));
            manualPositionsQuantity = GetTestDataAsInt(nameof(manualPositionsQuantity));

            combineFiltersQuantity = GetTestDataAsInt(nameof(combineFiltersQuantity));
            combinePositionsQuantity = GetTestDataValuesAsListByColumnName(nameof(combinePositionsQuantity)).Select(Parsing.ConvertToInt).ToList();
            foreach (var order in EnumUtils.GetValues<PositionsIssuesTypes>().OrderBy(t => t.GetDescription()))
            {
                issuesQuantity.Add(order, GetTestDataAsInt($"{nameof(issuesQuantity)}{(int)order}"));
            }

            for (int i = 1; i <= combineFiltersQuantity; i++)
            {
                AddPositionsFiltersModel(i);
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            AddManualPortfoliosAndPositions();

            new SettingsSteps().LoginNavigateToSettingsAlertsSetAlertsForStockOptionSave(UserModels.First(), false, false, string.Empty, string.Empty);
            PrepareAlertsTemplate();

            PortfoliosSetUp.ImportDagSiteInvestment17(true);
            var portfolioSyncId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment06(portfolioSyncId);

            ApplyTemplateToWatchOnlyPositions();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_344$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("The test checks correctness of filtration using Filter panel for Position grid https://tr.a1qa.com/index.php?/cases/view/19232700")]
        public override void RunTest()
        {
            LogStep(1, "Click 'Filters'. Check that filters form is shown");
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickPositionsFilters();
            var positionsFiltersForm = new PositionsFiltersForm();
            positionsFiltersForm.AssertIsOpen();

            LogStep(2, "Check that Asset Type checkbox expanded group contains only expected items");
            var actualAssetTypes = positionsFiltersForm.GetAllItemsInFilterFrame(PositionFilterType.AssetType);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedAssetTypes, actualAssetTypes),
                $"Default items in Asset Type Filter is not as expected\n{GetExpectedResultsString(expectedAssetTypes)}\r\n{GetActualResultsString(actualAssetTypes)}");

            LogStep(3, "Check that Position Type checkboxes contains only expected items");
            var actualPositionTypes = positionsFiltersForm.GetAllCheckboxesItems(PositionFilterType.PositionType);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPositionTypes, actualPositionTypes),
                 $"Default items in Position Type Filter is not as expected\n{GetExpectedResultsString(expectedPositionTypes)}\r\n{GetActualResultsString(actualPositionTypes)}");

            LogStep(4, "Check Health status selector");
            foreach (var healthStatus in EnumUtils.GetValues<HealthStatusFilter>())
            {
                Checker.IsTrue(positionsFiltersForm.IsHealthStatusSubFilterPresent(healthStatus),
                    $"{healthStatus.GetStringMapping()} subfilter is not exist");
            }

            LogStep(5, "Check that Status filter group contains only expected items");
            var actualPositionStatuses = positionsFiltersForm.GetAllItemsInDropdown(PositionFilterType.PositionStatus);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPositionStatuses, actualPositionStatuses),
                $"Default items in Status Filter is not as expected\n{GetExpectedResultsString(expectedPositionStatuses)}\r\n{GetActualResultsString(actualPositionStatuses)}");

            LogStep(6, "Check that Alert Status filter group contains only expected items");
            var actualAlertStatuses = positionsFiltersForm.GetAllItemsInDropdown(PositionFilterType.AlertStatus);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedAlertStatuses, actualAlertStatuses),
                $"Default items in Alert Status Filter is not as expected\n{GetExpectedResultsString(expectedAlertStatuses)}\r\n{GetActualResultsString(actualAlertStatuses)}");

            LogStep(7, 8, "Check that VQ Range and Gain w/Div field is presented");
            CheckNumericRangeFilter(PositionFilterType.VqRange);
            CheckNumericRangeFilter(PositionFilterType.GainWithDiv);

            LogStep(9, "Check that Type checkboxes contains only expected items");
            var actualTypes = positionsFiltersForm.GetAllCheckboxesItems(PositionFilterType.Type);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedTypes, actualTypes),
                 $"Default items in Position Type Filter is not as expected\n{GetExpectedResultsString(expectedTypes)}\r\n{GetActualResultsString(actualTypes)}");

            LogStep(10, "Close Positions Filters. Check default Positions Quantity");
            positionsFiltersForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Cancel);
            Checker.CheckEquals(defaultPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(),
                 "Default positions quantity is not as expected");

            LogStep(11, "Check quantity of positions");
            var statusesForAllPositions = positionsTabForm.GetStatusColumnsModels();
            Checker.CheckEquals(greenPositionsQuantity, statusesForAllPositions.Count(t => t.StatusFlag == FlagOfStatusColumnGridStates.NewAtBrokerage),
                "Green Flag positions quantity is not as expected");
            Checker.CheckEquals(issuesPositionsQuantity, statusesForAllPositions.Count(t => t.IssueFlag == FlagOfStatusColumnGridStates.NotDetectedAtBrokerage),
                "Issues positions quantity is not as expected");
            Checker.CheckEquals(delistedPositionsQuantity, statusesForAllPositions.Count(t => t.Type == TypeOfStatusColumnPositionGrid.Delisted),
                "Delisted positions quantity is not as expected");
            Checker.CheckEquals(expiredPositionsQuantity, statusesForAllPositions.Count(t => t.Type == TypeOfStatusColumnPositionGrid.Expired),
                "Expired positions quantity is not as expected");
            Checker.CheckEquals(untriggeredPositionsQuantity, statusesForAllPositions.Count(t => t.Alerts == StatusOfAlertOnPositionGridStates.UntriggeredAlert),
                "Untriggered alert positions quantity is not as expected");
            Checker.CheckEquals(triggeredPositionsQuantity, statusesForAllPositions.Count(t => t.Alerts == StatusOfAlertOnPositionGridStates.TriggeredAlert),
                "Triggered alert positions quantity is not as expected");
            Checker.CheckEquals(noAlertsPositionsQuantity, statusesForAllPositions.Count(t => t.Alerts == StatusOfAlertOnPositionGridStates.NoAlert),
                "Without Alerts positions quantity is not as expected");
            Checker.CheckEquals(synchedPositionsQuantity, statusesForAllPositions.Count(t => t.Type == TypeOfStatusColumnPositionGrid.Synched),
                "Synced positions quantity is not as expected");
            Checker.CheckEquals(manualPositionsQuantity, statusesForAllPositions.Count(t => t.Type == TypeOfStatusColumnPositionGrid.Manual),
                "Manual positions quantity is not as expected");

            LogStep(12, "Check Newly synched filtration");
            CheckPositionStatusFiltration(PositionsStatusTypes.NewlySyncedPosition, greenPositionsQuantity);

            LogStep(13, "Check Position With Issues filtration");
            var positionsGridSteps = new PositionsGridSteps();
            positionsGridSteps.ResetPositionsFilters();
            CheckPositionIssuesFilter(positionsTabForm);
            positionsGridSteps.ResetPositionsFilters();

            LogStep(14, "Check Delisted Positions filtration");
            CheckPositionStatusFiltration(PositionsStatusTypes.DelistedPosition, delistedPositionsQuantity);

            LogStep(15, "Check Expired Positions filtration");
            CheckPositionStatusFiltration(PositionsStatusTypes.ExpiredOption, expiredPositionsQuantity);

            LogStep(16, "Check Position with Untriggered Alerts filtration");
            positionsGridSteps.ResetPositionsFilters();
            CheckAlertStatusFiltration(AlertFilterTypes.AlertsUntriggered, untriggeredPositionsQuantity);

            LogStep(17, "Check Position with Triggered Alerts filtration");
            CheckAlertStatusFiltration(AlertFilterTypes.AlertsTriggered, triggeredPositionsQuantity);

            LogStep(18, "Check Position with Without Alerts filtration");
            CheckAlertStatusFiltration(AlertFilterTypes.NoAlertsCreated, noAlertsPositionsQuantity);

            LogStep(19, "Check Synced Position filtration");
            positionsGridSteps.ResetPositionsFilters();
            CheckTypeStatusFiltration(new TypeFilterModel { IsManual = false, IsSync = true }, synchedPositionsQuantity);

            LogStep(20, "Check Manual Position filtration");
            CheckTypeStatusFiltration(new TypeFilterModel { IsManual = true, IsSync = false }, manualPositionsQuantity + expiredPositionsQuantity + delistedPositionsQuantity);

            LogStep(21, 22, "Check combine filtrations");
            for (int i = 0; i < combineFiltersQuantity; i++)
            {
                positionsGridSteps.ResetPositionsFilters();
                positionsTabForm.ClickPositionsFilters();
                positionsFiltersForm.SetAllFilters(combineFiltersModels[i]);
                positionsFiltersForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
                Checker.CheckEquals(combinePositionsQuantity[i], positionsTabForm.GetNumberOfRowsInGrid(),
                    $"Combine filter applying #{i + 1}: positions quantity is not as expected");
            }
        }

        private void CheckPositionIssuesFilter(PositionsTabForm positionsTabForm)
        {
            var positionsGridSteps = new PositionsGridSteps();
            foreach (var item in EnumUtils.GetValues<PositionsIssuesTypes>())
            {
                var positionIssuesFilterModel = new PositionIssuesFilterModel();
                foreach (var issue in EnumUtils.GetValues<PositionsIssuesTypes>())
                {
                    positionIssuesFilterModel.IssueNameToState.Add(issue, false);
                }

                positionIssuesFilterModel.IssueNameToState[item] = true;
                positionsGridSteps.FilterPositionsIssues(positionIssuesFilterModel);
                Checker.CheckEquals(issuesQuantity[item], positionsTabForm.GetNumberOfRowsInGrid(),
                    $"Issue filter applying for {item}: positions quantity is not as expected");
            }

            positionsGridSteps.ResetPositionsFilters();
            var positionIAllssuesFilterModel = new PositionIssuesFilterModel();
            foreach (var item in EnumUtils.GetValues<PositionsIssuesTypes>())
            {
                positionIAllssuesFilterModel.IssueNameToState[item] = true;
            }

            positionsGridSteps.FilterPositionsIssues(positionIAllssuesFilterModel);
            Checker.CheckEquals(issuesPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(),
                "Issues filter applying for all items: positions quantity is not as expected");
        }

        private void CheckPositionStatusFiltration(PositionsStatusTypes positionsStatusTypes, int expectedPositionsQuantity)
        {
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickPositionsFilters();
            var positionsFiltersForm = new PositionsFiltersForm();
            positionsFiltersForm.AssertIsOpen();
            var currentPositionStatusFilter = new PositionStatusFilterModel();
            foreach (var status in expectedPositionStatuses)
            {
                currentPositionStatusFilter.PositionsStatusNameToState.Add(status.ParseAsEnumFromDescription<PositionsStatusTypes>(), false);
            }
            currentPositionStatusFilter.PositionsStatusNameToState[positionsStatusTypes] = true;
            positionsFiltersForm.SetPositionStatusFilter(currentPositionStatusFilter);
            positionsFiltersForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            Checker.CheckEquals(expectedPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(),
                $"{positionsStatusTypes.GetDescription()} positions quantity is not as expected");
        }

        private void CheckAlertStatusFiltration(AlertFilterTypes alertsStatusTypes, int expectedPositionsQuantity)
        {
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickPositionsFilters();
            var positionsFiltersForm = new PositionsFiltersForm();
            positionsFiltersForm.AssertIsOpen();
            var currentAlertStatusFilterModel = new AlertStatusFilterModel();
            foreach (var status in expectedAlertStatuses)
            {
                currentAlertStatusFilterModel.AlertsStatusNameToState.Add(status.ParseAsEnumFromStringMapping<AlertFilterTypes>(), false);
            }
            currentAlertStatusFilterModel.AlertsStatusNameToState[alertsStatusTypes] = true;
            positionsFiltersForm.SetAlertStatusFilter(currentAlertStatusFilterModel);
            positionsFiltersForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            Checker.CheckEquals(expectedPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(),
                $"{alertsStatusTypes.GetStringMapping()} positions quantity is not as expected");
        }

        private void CheckTypeStatusFiltration(TypeFilterModel filterModel, int expectedPositionsQuantity)
        {
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickPositionsFilters();
            var positionsFiltersForm = new PositionsFiltersForm();
            positionsFiltersForm.AssertIsOpen();
            positionsFiltersForm.SetTypeFilter(filterModel);
            positionsFiltersForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            Checker.CheckEquals(expectedPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(),
                "Type positions quantity is not as expected");
        }

        private void CheckNumericRangeFilter(PositionFilterType filter)
        {
            var positionsFiltersForm = new PositionsFiltersForm();
            positionsFiltersForm.ScrollToPreviousFilter(filter);
            var dropdownItems = EnumUtils.GetValues<NumericRangeFilterThresholdTypes>();
            foreach (var subfilter in dropdownItems)
            {
                Checker.IsTrue(positionsFiltersForm.IsNumericRangeOptionDropDownPresent(filter, subfilter),
                    $"{filter.GetStringMapping()} filter on the Position Filters does not have {subfilter.GetStringMapping()} subfilter");
            }
        }

        private void ApplyTemplateToWatchOnlyPositions()
        {
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.AllWatch.GetStringMapping());
            new PositionsGridSteps().SelectAllPositionApplyTemplate(templateName);
            new ConfirmPopup(PopupNames.Warning).ClickOkButton();
        }

        private void PrepareAlertsTemplate()
        {
            var templateSetUps = new TemplateSetUps();
            templateSetUps.NavigateToTemplatesClickAddEnterTemplateName(templateName);
            var addTemplateForm = new AddTemplateForm();
            addTemplateForm.SelectAlertCategory(AlertCategoryTypes.TrailingStop.GetStringMapping());
            var percentageTrailingStopForm = new PercentageTrailingStopForm();
            percentageTrailingStopForm.SelectPercentageTrailingStopType(TrailingStopAlertTypes.Ts.ToString());
            percentageTrailingStopForm.SetCustomTsPercent(tsPercent);
            addTemplateForm.ClickOnAddAlertButton(AlertTypes.PercentageTrailingStop.GetStringMapping());
            addTemplateForm.SelectAlertCategory(AlertCategoryTypes.Fundamentals.GetStringMapping());
            templateSetUps.AddTargetAlert(FundamentalAlertTypes.MarketCap, OperationType.Above);
            addTemplateForm.ClickSaveTemplate();
        }

        private void AddManualPortfoliosAndPositions()
        {
            var portfoliosIds = new List<int>
            {
                PortfoliosSetUp.AddWatchOnlyPortfolio(UserModels.First().Email),
                PortfoliosSetUp.AddWatchOnlyPortfolio(UserModels.First().Email)
            };

            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds.First(), positionModel);
            }
            var positionsQueries = new PositionsQueries();
            foreach (var positionModel in positionsModels)
            {
                var positionId = PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds.Last(), positionModel);
                if (positionModel == positionsModels.Last())
                {
                    positionsQueries.SetStatusTypeAsExpired(positionId);
                }
                else
                {
                    positionsQueries.SetStatusTypeAsDelisted(positionId);
                }
            }
        }

        private void AddPositionsFiltersModel(int filterOrder)
        {
            var enabledAssetTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty($"assetType{filterOrder}Value");
            var assetTypeModel = new AssetTypeFilterModel();
            foreach (var assetType in enabledAssetTypes)
            {
                assetTypeModel.AssetFilterNameToState.Add(assetType, true);
            }

            var positionTypeFilterModel = new PositionTypeFilterModel
            {
                IsLong = GetTestDataAsBool($"positionType{filterOrder}IsLong"),
                IsShort = GetTestDataAsBool($"positionType{filterOrder}IsShort"),
            };

            var healthStatusFilterModel = FillDataForHealthFilter("health", filterOrder);

            var enabledPositionStatuses = GetTestDataValuesAsListByColumnNameAndRemoveEmpty($"positionStatus{filterOrder}Value");
            var positionStatusFilterModel = new PositionStatusFilterModel();
            foreach (var positionStatus in enabledPositionStatuses)
            {
                positionStatusFilterModel.PositionsStatusNameToState.Add(positionStatus.ParseAsEnumFromDescription<PositionsStatusTypes>(), true);
            }

            var enabledAlertStatuses = GetTestDataValuesAsListByColumnNameAndRemoveEmpty($"alertStatus{filterOrder}Value");
            var alertStatusFilterModel = new AlertStatusFilterModel();
            foreach (var alertStatus in enabledAlertStatuses)
            {
                alertStatusFilterModel.AlertsStatusNameToState.Add(alertStatus.ParseAsEnumFromStringMapping<AlertFilterTypes>(), true);
            }

            var vqRangeFilterModel = FillDataForNumericFilter(PositionFilterType.VqRange, "vqFilter", filterOrder);

            var typeFilterModel = new TypeFilterModel
            {
                IsSync = GetTestDataAsBool($"type{filterOrder}IsSync"),
                IsManual = GetTestDataAsBool($"type{filterOrder}IsManual"),
            };
            combineFiltersModels.Add(new PositionsFiltersModel
            {
                AssetTypeFilter = assetTypeModel,
                PositionTypeFilter = positionTypeFilterModel,
                HealthStatusFilter = healthStatusFilterModel,
                PositionStatusFilter = positionStatusFilterModel,
                AlertStatusFilter = alertStatusFilterModel,
                VqRangeFilter = vqRangeFilterModel,
                TypeFilter = typeFilterModel
            });
        }
    }
}