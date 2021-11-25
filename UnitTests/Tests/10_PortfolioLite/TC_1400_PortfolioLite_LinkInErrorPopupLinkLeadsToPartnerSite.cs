using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Helpers;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1400_PortfolioLite_LinkInErrorPopupLinkLeadsToPartnerSite : BaseTestUnitTests
    {
        private const int TestNumber = 1400;

        private string bigValue;
        private string entryDate;
        private string errorMessage;
        private string expectedUrl;
        private string symbolToSearch;

        [TestInitialize]
        public void TestInitialize()
        {
            symbolToSearch = GetTestDataAsString(nameof(symbolToSearch));
            bigValue = GetTestDataAsString(nameof(bigValue));
            entryDate = GetTestDataAsString(nameof(entryDate));
            expectedUrl = GetTestDataAsString(nameof(expectedUrl));
            errorMessage = GetTestDataAsString(nameof(errorMessage));

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. navigate To Add portfolio Form");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First(), EnumDisplayNamesHelper.Get(PublisherTypes.InvestorPlaceMedia));
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.SearhSymbol(symbolToSearch);
            new PortfolioLiteAnalyzerForm().ClickAdditionalActionsButton(PortfolioLiteAdditionalButtons.AddToPortfolio);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1400$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that contact link in Oops popup leads to the right page. https://tr.a1qa.com/index.php?/cases/view/21116203")]
        public override void RunTest()
        {
            LogStep(1, "Click 'Add positions'");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            var addToPortfolioForm = new PortfolioLiteAddToPortfolioForm();
            addToPortfolioForm.AssertIsOpen();

            LogStep(2, "Type data");
            addToPortfolioForm.SetValueInDatePickerField(PortfolioLiteAddPositionFields.BuyDate, entryDate);
            addToPortfolioForm.SetValueInTextBoxField(PortfolioLiteAddPositionFields.BuyPrice, bigValue);
            addToPortfolioForm.SetValueInTextBoxField(PortfolioLiteAddPositionFields.Qty, bigValue);

            LogStep(3, "Click Save");
            addToPortfolioForm.ClickAdditionalActionsButton(PortfolioLiteAddActionsTypes.Save);
            var customPopup = new ConfirmPopup(PopupNames.Error);
            customPopup.AssertIsOpen();

            LogStep(4, "Check that 'contact' opens Link from test data");
            Checker.CheckContains(errorMessage, customPopup.GetMessage(), "Error message is not as expected");
            Checker.CheckContains(expectedUrl, customPopup.GetLinkFromMessage(), "Contact link is not as expected");
        }
    }
}