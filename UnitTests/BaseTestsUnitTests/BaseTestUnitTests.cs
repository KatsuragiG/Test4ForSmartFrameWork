using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Publications;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Models.PublicationsModels;
using AutomatedTests.Models.UserModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.ThirdPartyResourcesSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.BaseTestsUnitTests
{
    [TestClass]
    public class BaseTestUnitTests : BaseTest
    {
        protected List<UserModel> UserModels = new List<UserModel>();

        public override void RunTest()
        {
            throw new NotImplementedException();
        }

        protected const string DataProviderName = "System.Data.OleDb";
        protected const string DataProviderConnectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\TSPData.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
        protected bool IsDeleteUserViaApi = true;

        protected string GetTestDataAsString(string propName) => TestContext.DataRow[propName].ToString();
        protected bool GetTestDataAsBool(string propName) => Parsing.ConvertToBool(GetTestDataAsString(propName));
        protected int GetTestDataAsInt(string propName) => Parsing.ConvertToInt(GetTestDataAsString(propName));
        protected double GetTestDataAsDouble(string propName) => Parsing.ConvertToDouble(GetTestDataAsString(propName));
        protected decimal GetTestDataAsDecimal(string propName) => Parsing.ConvertToDecimal(GetTestDataAsString(propName));

        protected T GetTestDataParsedAsEnumFromStringMapping<T>(string propName) where T : struct, IConvertible
        {
            return GetTestDataAsString(propName).ParseAsEnumFromStringMapping<T>();
        }

        protected T GetTestDataParsedAsEnumFromDescription<T>(string propName) where T : struct, IConvertible
        {
            return GetTestDataAsString(propName).ParseAsEnumFromDescription<T>();
        }

        protected List<string> GetTestDataValuesAsListByColumnName(string columnName)
        {
            var values = new List<string>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            foreach (var column in tableColumns)
            {
                if (column.ToString().StartsWith(columnName))
                {
                    values.Add(GetTestDataAsString(column.ToString()));
                }
            }
            return values;
        }

        protected List<string> GetTestDataValuesAsListByColumnNameAndRemoveEmpty(string columnName)
        {
            return GetTestDataValuesAsListByColumnName(columnName).Where(t => !string.IsNullOrEmpty(t)).ToList();
        }

        protected NumericFilterModel FillDataForNumericFilter(Enum filterType, string testDataStartFieldName)
        {
            return FillDataForNumericFilter(filterType, testDataStartFieldName, Constants.ItemNotFoundInCollection);
        }

        protected NumericFilterModel FillDataForNumericFilter(Enum filterType, string testDataStartFieldName, int filterOrder)
        {
            var filterStringOrder = FormatFilterOrder(filterOrder);
            var filterModel = new NumericFilterModel
            {
                SubFilterName = filterType
            };
            var numericFiltersSubfiltersCount = EnumUtils.GetValues<NumericRangeSubFilterTypes>().Count();
            for (int i = 1; i <= numericFiltersSubfiltersCount; i++)
            {
                filterModel.NumericRangeSubFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<NumericRangeSubFilterTypes>($"{testDataStartFieldName}{filterStringOrder}Option{i}"),
                    GetTestDataAsString($"{testDataStartFieldName}{filterStringOrder}Value{i}"));
            }

            return filterModel;
        }

        protected HealthStatusFilterModel FillDataForHealthFilter(string testDataStartFieldName)
        {
            return FillDataForHealthFilter(testDataStartFieldName, Constants.ItemNotFoundInCollection);
        }

        protected HealthStatusFilterModel FillDataForHealthFilter(string testDataStartFieldName, int filterOrder)
        {
            var filterStringOrder = FormatFilterOrder(filterOrder);
            var filter = new HealthStatusFilterModel();
            for (int i = 1; i <= EnumUtils.GetValues<HealthStatusFilter>().Count(); i++)
            {
                filter.HealthStatusFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<HealthStatusFilter>($"{testDataStartFieldName}{filterStringOrder}Option{i}"),
                    Parsing.ConvertToBool(GetTestDataAsString($"{testDataStartFieldName}{filterStringOrder}Value{i}")));
            }

            return filter;
        }

        protected void CloseCurrentBrowserTabViaDriverAndSwitchToWindow(string currentWindow)
        {
            Browser.GetDriver().Close();
            Browser.GetDriver().SwitchTo().Window(currentWindow);
        }

        protected string GetDownloadedFilePathGridDepended()
        {
            if (WebdriverFramework.Framework.WebDriver.Configuration.UseSeleniumGrid)
            {
                var uriParts = WebdriverFramework.Framework.WebDriver.Configuration.SeleniumGridUrl.Split(':');
                return $"{uriParts[0]}:{uriParts[1]}:4444/download/{Browser.GetDriver().SessionId}/";
            }
            return $"{WebdriverFramework.Framework.WebDriver.Configuration.DownloadDirectory}{Path.DirectorySeparatorChar}";
        }

        protected List<ProductSubscriptions> GetUserProductSubscriptions(string fieldsName)
        {
            var userSubscriptions = GetTestDataValuesAsListByColumnName(fieldsName).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return userSubscriptions.Select(userSubscription => (ProductSubscriptions)(int)userSubscription.ParseAsEnumFromStringMapping<ProductSubscriptionTypes>()).ToList();
        }

        protected void CheckPublicationOpening(PublicationModel selectedPublication, PublicationDbModel currentPublicationDbModel)
        {
            if (!(string.IsNullOrEmpty(currentPublicationDbModel.PdfReportName) || currentPublicationDbModel.SourceUrl.Contains(Constants.WistaUrlAttribute)))
            {
                var filePath = $"{GetDownloadedFilePathGridDepended()}{currentPublicationDbModel.PdfReportName}";
                FileUtilsExtension.WaitUntilFileIsDownloaded(filePath);
                Checker.IsTrue(FileUtilsExtension.IsFileExistGridDepended(filePath),
                    $"File '{currentPublicationDbModel.PdfReportName}' is not downloaded for {selectedPublication.PublicationTitle} publication  " +
                    $"for {selectedPublication.PublicationType}");
            }
            else if (currentPublicationDbModel.SourceUrl.Contains(Constants.WistaUrlAttribute))
            {
                Browser.SwitchToLastWindow();
                var newsPopupWistaForm = new NewsPopupWistaForm();
                newsPopupWistaForm.AssertIsOpen();
                var driver = Browser.GetDriver();
                if (driver.WindowHandles.Count > 1)
                {
                    driver.Close();
                }
                else
                {
                    newsPopupWistaForm.ClickCloseButton();
                }
                Browser.SwitchToFirstWindow();
            }
            else if (currentPublicationDbModel.SourceUrl.Contains(Constants.TradeSmithWordpressSitesAttribute) || currentPublicationDbModel.PublicationSourceId == (int)PublicationSources.WordPressPosts)
            {
                new BrowserSteps().CheckThatNewTabOpensPerformActionWithSwitchToNewTabBackAfterClosing(() =>
                    new ThirdPartyResourcesSteps().CheckOpenedNewsPageWithHeader(selectedPublication.PublicationTitle));
            }
            else
            {
                Checker.Fail($"No PDF or Wista filename for '{selectedPublication.PublicationTitle}' in DB with description {selectedPublication.PublicationDescription}");
            }
        }

        protected static string GetActualResultsString<T>(List<T> actual)
        {
            return $"Actual: \r\n{string.Join("\n", actual)}";
        }

        protected static string GetExpectedResultsString<T>(List<T> expected)
        {
            return $"Expected: \r\n{string.Join("\n", expected)}";
        }

        private string FormatFilterOrder(int filterOrder)
        {
            return filterOrder == Constants.ItemNotFoundInCollection
                ? string.Empty
                : filterOrder.ToString();
        }

        [AssemblyInitialize]
        public static void BeforeTest(TestContext tc)
        {
            var supervisorEmail = new CustomTestDataReader().GetNewAdminUser().Email;
            var isSupervisorAlreadyExist = new UsersQueries().SelectTradeSmithUserFromMasterDBByUserEmail(supervisorEmail) != null;

            if (WebdriverFramework.Framework.WebDriver.Configuration.UseSeleniumGrid 
                && !isSupervisorAlreadyExist)
            {
                ApiClientSetUp.CreateSupervisorUser();
            }
        }

        [TestCleanup]
        public new void CleanAfterTest()
        {
            if (IsDeleteUserViaApi)
            {
                UserModels.ForEach(TearDowns.DeleteUserViaApi);
            }
            base.CleanAfterTest();
        }
    }
}