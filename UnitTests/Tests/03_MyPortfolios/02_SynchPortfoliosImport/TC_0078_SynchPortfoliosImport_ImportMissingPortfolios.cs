using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using TradeStops.Common.Enums;
using System.Linq;
using AutomatedTests.Navigation;
using System.Collections.Generic;
using AutomatedTests.Utils;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using System.Text.RegularExpressions;
using AutomatedTests.Models.PortfoliosModels;
using System;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0078_SynchPortfoliosImport_ImportMissingPortfolios : BaseTestUnitTests
    {
        private const int TestNumber = 78;

        private List<int> importedPortfoliosIds;
        private List<string> expectedPortfoliosName = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            expectedPortfoliosName = GetTestDataValuesAsListByColumnName("portfoliosName");

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));
            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.ImportDagSiteInvestment13(true);            
            new MainMenuNavigation().OpenInvestmentPortfoliosTab();
            importedPortfoliosIds = new PortfoliosForm().GetPortfoliosIds();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_78$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("Test checks availability of the Importing missing portfolio feature for synch portfolio; https://tr.a1qa.com/index.php?/cases/view/19232921")]
        public override void RunTest()
        {
            LogStep(1, "Sort the Portfolios grid by Portfolio Name asc.Check that three portfolios are imported ");            
            var portfolioGridsSteps = new PortfolioGridsSteps();
            var actualPortfoliosNames = portfolioGridsSteps.GetPortfolioNamesCompareWithExpected(expectedPortfoliosName);
            var createdDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());

            LogStep(2, "Remember: Positions number in each portfolio; -Currency for each portfolio.");            
            var portfoliosInformation = importedPortfoliosIds.Select(t => portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(t))
                .OrderBy(newPortfoliosId => newPortfoliosId.Currency).ToList();

            LogStep(3, "Open Edit Portfolio popup vie Portfolio Menu (the 2nd column in the grid) -> Edit Portfolio.");
            var editPortfoliosInfo = new List<EditSyncPortfolioModel>();
            var mainMenuNavigation = new MainMenuNavigation();
            var portfoliosForm = new PortfoliosForm();
            foreach (var importedPortfolioId in importedPortfoliosIds)
            {
                var editPortfolioPopup = portfolioGridsSteps.ClickEditPortfolioByIdGetEditPopupForm(importedPortfolioId);
                var editPortfolioInfo = editPortfolioPopup.GetEditPortfolioInformation();

                editPortfolioPopup.ClickClose();
                portfoliosForm.SelectPortfolioContextMenuOption(importedPortfolioId, PortfolioContextNavigation.Synchronize);
                editPortfolioInfo.MemberItemIdentifier = new SyncFlowEditForm().GetVendorAccountIdentifier();
                editPortfoliosInfo.Add(editPortfolioInfo);
                mainMenuNavigation.OpenInvestmentPortfoliosTab();
            }

            LogStep(4, "Tick two random portfolios in the first column of the Portfolios grid. Remember what portfolios exactly was ticked." +
                "Make sure the number of 'Selected Items' in the footer is 2.");
            var deletedPortfoliosIds = SelectTwoPortfolios(importedPortfoliosIds);
            Checker.CheckEquals(deletedPortfoliosIds.Count, portfoliosForm.GetSelectedItemsNumberFromFooter(),
                "Number of 'Selected Items' in the footer is not as expected");

            LogStep(5, "Click Delete button. Confirm deleting in Delete Portfolio popup. Click OK in Success popup.");
            portfoliosForm.ClickDeleteButton();
            portfolioGridsSteps.ConfirmDeletingPortfoliosCloseSuccessPopup();

            var portfoliosQueries = new PortfoliosQueries();
            Checker.IsTrue(deletedPortfoliosIds.Any(), "There are no portfolios were deleted");
            foreach (var portfolioId in deletedPortfoliosIds)
            {
                Checker.IsFalse(portfoliosForm.IsPortfolioPresentInGrid(portfolioId), $"Portfolio with ID {portfolioId} present in grid");
                Checker.IsTrue(Parsing.ConvertToBool(portfoliosQueries.SelectPortfolioDataByPortfolioId(portfolioId).Delisted),
                    "Portfolio is not deleted");
            }
           
            LogStep(6, "Click Refresh button in the grid (3rd column) for the remaining portfolio." +
                "Make sure that Vendor Account Identifier is the same as in the URL.");
            portfoliosForm.SelectPortfolioContextMenuOption(importedPortfoliosIds[0], PortfolioContextNavigation.Synchronize);

            var retainedPortfolioName = portfoliosInformation.Where(portfolioInformation => portfolioInformation.Id == importedPortfoliosIds[0].ToString())
                .ToList().First().PortfolioName;
            var retainedMemberItemIdentifierFromUrl = editPortfoliosInfo.Where(p => p.Portfolio == retainedPortfolioName).ToList().First().MemberItemIdentifier.ToString();
            var memberItemIdentifierFromUrl = Regex.Match(Browser.GetDriver().Url, Constants.NumberPattern, RegexOptions.RightToLeft).ToString();
            Checker.CheckEquals(retainedMemberItemIdentifierFromUrl, memberItemIdentifierFromUrl,
                "Vendor Account Identifier is NOT the same as in the URL.");

            LogStep(7, 8, "Select 'Import Missing Portfolios' option. Click Synchronize.");
            new SyncFlowEditForm().ClickImportMissingClickNexIfPresent();
            portfoliosForm.AssertIsOpen();

            LogStep(9, "Sort the Portfolios grid by Portfolio Name ASC. " +
                "Make sure that deleted portfolios are restored and grid contains all imported in preconditions portfolios: ");
            portfoliosForm.ClickOnPortfolioColumnToSort(PortfolioGridColumnTypes.PortfolioName, SortingStatus.Asc);
            var refreshedPortfoliosNames = portfoliosForm.GetValuesOfPortfolioColumn(PortfolioType.Investment, PortfolioGridColumnTypes.PortfolioName);
            Checker.CheckListsEquals(actualPortfoliosNames, refreshedPortfoliosNames, "Import-Missing restored portfolio names are not as expected");

            LogStep(10, 11, "Compare positions number and currency in each portfolio with stored in the step 2");
            var missingPortfoliosIds = portfoliosForm.GetPortfoliosIds();
            var newPortfoliosIds = missingPortfoliosIds.Except(importedPortfoliosIds).ToList();
            Checker.IsTrue(newPortfoliosIds.Any(), "There are no restored portfolios");
            var portfoliosInformationForRestored = newPortfoliosIds.Select(id => portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(id))
                .ToList().OrderBy(newPortfoliosId => newPortfoliosId.Currency).ToList();

            for (var i = 0; i < newPortfoliosIds.Count; i++)
            {
                Checker.CheckEquals(portfoliosInformation[i].Positions, portfoliosInformationForRestored[i].Positions,
                    $"Count of Positions for portfolio {newPortfoliosIds[i]} on Positions column are not corresponded of step #2");
                Checker.CheckEquals(portfoliosInformationForRestored[i].Currency, portfoliosInformation[i].Currency,
                    $"Currency is not correct for portfolio {newPortfoliosIds[i]}");
                Checker.CheckEquals(createdDate, portfoliosInformation[i].CreatedDate,
                    "Created date is not the same with current date");
            }

            LogStep(12, "Open Edit Portfolio popup vie Portfolio Menu (the 2nd column in the grid) -> Edit Portfolio.");
            foreach (var newPortfolioId in newPortfoliosIds)
            {
                var editPortfolioPopup = portfolioGridsSteps.ClickEditPortfolioByIdGetEditPopupForm(newPortfolioId);
                var editPortfolioInfo = editPortfolioPopup.GetEditPortfolioInformation();
                editPortfolioPopup.ClickClose();

                portfoliosForm.SelectPortfolioContextMenuOption(newPortfolioId, PortfolioContextNavigation.Synchronize);
                editPortfolioInfo.MemberItemIdentifier = new SyncFlowEditForm().GetVendorAccountIdentifier();

                Checker.IsTrue(editPortfoliosInfo.Contains(editPortfolioInfo), "Values are changed");
                mainMenuNavigation.OpenInvestmentPortfoliosTab();
            }
        }

        private List<int> SelectTwoPortfolios(List<int> portfoliosIds)
        {
            var deletedPortfoliosIds = new List<int>();
            var portfoliosForm = new PortfoliosForm();
            var portfolioId1 = portfoliosIds[SRandom.Instance.Next(0, portfoliosIds.Count - 1)];
            portfoliosForm.SetPortfolioCheckboxByIdState(portfolioId1, true);
            deletedPortfoliosIds.Add(portfolioId1);
            portfoliosIds.Remove(portfolioId1);
            var portfolioId2 = portfoliosIds[SRandom.Instance.Next(0, portfoliosIds.Count - 1)];
            portfoliosForm.SetPortfolioCheckboxByIdState(portfolioId2, true);
            deletedPortfoliosIds.Add(portfolioId2);
            portfoliosIds.Remove(portfolioId2);
            return deletedPortfoliosIds;
        }
    }
}