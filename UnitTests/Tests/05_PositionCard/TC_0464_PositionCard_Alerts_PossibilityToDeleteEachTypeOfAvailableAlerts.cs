using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Popups;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0464_PositionCard_Alerts_PossibilityToDeleteEachTypeOfAvailableAlerts : BaseTestUnitTests
    {
        private const int TestNumber = 464;

        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private readonly List<int> positionsIds = new List<int>();
        private int numberOfPositions;
        private string templateName;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            templateName = StringUtility.RandomString("Template######");
            numberOfPositions = GetTestDataAsInt(nameof(numberOfPositions));
            for (int i = 1; i <= numberOfPositions; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}")}"
                });
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
            {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
            }));
            var portfolioManualInvestment = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioManualInvestment, positionModel));
            }
            LoginSetUp.LogIn(UserModels.First());
            new PreconditionsCommonSteps().AddTemplateWithAllTypesOfAlertsToAllPositionsForNonBasic(templateName);
            new ConfirmPopup(PopupNames.Warning).ClickOkButton();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_464$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232580 The test checks that possible delete each of added alerts for Premium TS.")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardAlertsTab"), TestCategory("AlertDelete"), TestCategory("Alerts")]
        public override void RunTest()
        {
            LogStep(1, 4, "Repeat steps #1-8 for position of precondition #2-2.");
            foreach (var positionId in positionsIds)
            {
                new AlertTabPositionCardSteps().DeleteAllAddedAlertsFromPositionCardWithVerification(positionId);
            }
        }
    }
}