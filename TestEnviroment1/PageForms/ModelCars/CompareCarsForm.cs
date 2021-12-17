using ModelCars.Elements;
using OpenQA.Selenium;
using PageForms.Enums.DropDownResearchEnums;
using PageForms.Enums.TrimParametrs;
using PageForms.ModelCars.NavigationMenu;
using System.Collections.Generic;
using System.Linq;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver.Elements;

namespace PageForms.ModelCars.CompareCarsForm
{
    public class CompareCarsForm : NavigationMenuForm
    {
        private static readonly By titleLocator = By.CssSelector("div[class *= 'compare-']");

        private const string CompareCardLocator = "//div[contains(@class, 'card{0}')]";
        private const string AddCarLink = CompareCardLocator + "//a[@class = 'add-car']";
        private const string DropDownModal = "//div[contains(@class, 'sds-input-container--dropdown')]//label[text() = '{0}']/preceding-sibling::select[contains(@class, 'text-field')]";
        private const string TableItems = "(//td[contains(text(), '{0}')]/following::tr[contains(@class, 'row-count')]/td)[{1}]";
        private const string StyleForCar = "(//div[contains(@class, 'trim-text')])[{0}]";

        private readonly PopUp PopUpLocator = new PopUp(By.XPath("//div[contains(@class, 'sds-modal-visible')]"), "PopUp");
        private readonly Label PopUpLabelToClick = new Label(By.XPath("//div[@class = 'sds-modal__content-body sds-modal__content-body']//h2"), "LabelToClick");
        private readonly Link DescriptionLinkForCar = new Link(By.XPath("(//div[contains(@class, '-url')]//a)[{0}]"), "Description link");        
        private readonly Button AddCarPopUpButton = new Button(By.XPath("//button[@type = 'submit']"), "Add car to comparison");
        private readonly Button SeeTheComparisonButton = new Button(By.XPath("//button[@phx-click = 'details']"), "See the comparison button");

        public CompareCarsForm() : base(titleLocator, "Trims Page Form")
        {
        }

        public CompareCarsForm(By titleLocator, string title) : base(titleLocator, title)
        {
        }       

        public IDictionary<string, int> NumberCardsForCar = new Dictionary<string, int>
            {
                { "card1", 1 },
                { "card2", 2 }
            };

        public void ClickOnAddCardLink(int numberOfCard)
        {            
            var cardNumber = string.Format(AddCarLink, numberOfCard);
            var AddCardLink = new Link(By.XPath(cardNumber), "Add card link");
            AddCardLink.ScrollIntoViewWithCenterAligning();
            AddCardLink.WaitForElementIsPresent();
            try
            {
                AddCardLink.ClickAndWaitForLoading();
            }
            catch (ElementClickInterceptedException)
            {
                AddCardLink.DoubleClick();                
            }            
        }

        public void ClickOnAddCarButtonModal()
        {
            AddCarPopUpButton.WaitForIsEnabled();
            AddCarPopUpButton.Click();        
        }

        public bool IsPopUpPresent()
        {
            return PopUpLocator.IsPresent();
        }

        public void SelectValueInModalDropDown(string text, DropDownResearchEnums value)
        {
            var dropdown = new DropDown(By.XPath(string.Format(DropDownModal, value.GetStringMapping())), "DropDown Modal");           
            dropdown.SelectByText(text);
            PopUpLabelToClick.Click();
        }

        public void ClickOnSeeComparisonButton()
        {
            SeeTheComparisonButton.ClickAndWaitForLoading();        
        }

        public string GetDescriptionForCar(int value)
        {
            return DescriptionLinkForCar.GetText();
        }

        public string GetStyleDescriptionForCar(int value)
        {
            return new Label(By.XPath(string.Format(StyleForCar, value)), "Style Description").GetText();        
        }

        public string GetComparerParametersForCar(TrimParametersEnums column, int value)
        {
            return FindElements(By.XPath(string.Format(TableItems, column.GetStringMapping(), value))).Last().Text;
        }
    }
}