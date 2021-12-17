using OpenQA.Selenium;
using PageForms.Enums.NavigationMenu;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.ModelCars.NavigationMenu
{
    public class NavigationMenuForm : BaseForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'global-header-container']");

        private const string NavigationMenu = "//div[contains(@class, 'global-header-menu-links')]";
        private const string NavigationPagesLocator = NavigationMenu + "//li[contains(@class,  'header-link')]//a[text() = '{0}']";

        private readonly MenuItem LogoForMainMenu = new MenuItem(By.XPath("//a[contains(@class, '-logo')]"), "Header Logo");

        protected readonly Logger _logger;

        public NavigationMenuForm() : base(titleLocator, "Navigation Menu Form")
        {
            _logger = Logger.Instance;
        }

        public NavigationMenuForm(By titleLocator, string title) : base(titleLocator, title)
        {
            _logger = Logger.Instance;
        }

        public void NavigateToMainPage()
        {
            LogoForMainMenu.ClickAndWaitForLoading();
        }

        public void NavigateToResearchPage()
        {
           new Link(By.XPath(string.Format(NavigationPagesLocator, NavigationEnums.ResearchReviews.GetStringMapping())), "Research page").Click();           
        }                
    }
}