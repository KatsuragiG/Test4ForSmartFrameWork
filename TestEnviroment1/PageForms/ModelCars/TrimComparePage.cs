using OpenQA.Selenium;
using PageForms.Enums.TrimParametrs;
using PageForms.ModelCars.NavigationMenu;
using System.Collections.Generic;
using System.Linq;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.ModelCars.TrimComparePages
{
    public class TrimComparePage : NavigationMenuForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'research_trim_page']");

        private const string TrimsParameters = "//section[contains(@class, 'trim-accordion-container')]//div[contains(@class, 'sds-accordion')]";
        private const string TrimPanelFirst = "(//div[contains(@id, '-panel1')])[1]";
        private const string TrimColumnsForCar = TrimPanelFirst + "//tr[@class = 'heading-row']";
        private const string TrimParametersForModel = TrimColumnsForCar + "//th[contains(text(), '{0}')]/following::tr[@class = 'content-row'][1]//td[1]";
                
        private readonly Button BaseParametersButton = new Button(By.XPath("(//button[contains(@class, '-accordion-trigger')])[1]"), "button for cars");
        public TrimComparePage() : base(titleLocator, "Trims Page Form")
        {
        }

        public TrimComparePage(By titleLocator, string title) : base(titleLocator, title)
        {
        }

        public bool IsTrimParametersIsShown()
        {
            return FindElements(By.XPath(TrimsParameters)).Any(l => l.Displayed);
        }

        public void ClickOnBaseSectionForCar()
        {
            BaseParametersButton.ClickAndWaitForLoading();
        }

        public List<string> GetTrimColumnsForCar()
        {
            return FindElements(By.XPath(TrimColumnsForCar)).Select(l => l.Text).ToList();
        }

        public string GetTrimParametrsForCar(TrimParametersEnums column)
        {            
            return FindElements(By.XPath(string.Format(TrimParametersForModel, column.GetStringMapping()))).Last().Text;
        }
    }
}