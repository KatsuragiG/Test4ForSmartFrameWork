using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageForms.Enums.DropDownResearchEnums;
using PageForms.Enums.TrimParametrs;
using PageForms.ModelCars.CarsForm;
using PageForms.ModelCars.CompareCarsForm;
using PageForms.ModelCars.Models.CarsDropDownModel;
using PageForms.ModelCars.Models.CarsParametrsModel;
using PageForms.ModelCars.NavigationMenu;
using PageForms.ModelCars.ResearchForm;
using PageForms.ModelCars.TrimComparePages;
using System.Linq;
using TestEnviroment.BrowserStart;
using WebdriverFramework.Framework.WebDriver;

namespace TestForModels
{
    [TestClass]
    public class Test_02_Models : BaseTest
    {
        private CarsDropDownModel valuesDropDownForFirstCar;
        private CarsDropDownModel valuesDropDownForSecondCar;
        private CarsParametersModel parametersForFirstCar;
        private CarsParametersModel parametersForSecondCar;
        private CarsParametersModel comparedParametersForFirstCar;
        private CarsParametersModel comparedParametersForSecondCar;

        private const int OuantityOfRepeatCycle = 10;
        private readonly string allYearsValue = "All years";
        private readonly string notAvailable = "not available";
        private int steps;        
        private int quantityOfRetry;
        private int cardCount = 1;
        private bool isTrimShown;

        [TestInitialize]
        public void TestInitialize()
        {            
            new BrowserStart().NavigateToMainPage();
        }

        [TestMethod]
        public override void RunTest()
        {
            LogStep(steps++, "Check that main page is opened");
            var carsForm = new CarsForm();
            carsForm.AssertIsOpen();

            LogStep(steps++, "Go to Research page and do steps for check that trim parameters is shown for car");
            valuesDropDownForFirstCar = StepsToTrim();

            LogStep(steps++, "Catch the parameters for base first trim: Available Engines, Transmissions");
            parametersForFirstCar = CatchParametersForCarModel();
            carsForm.NavigateToMainPage();

            LogStep(steps++, "Catch the parameters for second car");            
            valuesDropDownForSecondCar = StepsToTrim();
            parametersForSecondCar = CatchParametersForCarModel();

            LogStep(steps++, "Navigate to research page and click Side-bySide Comparisons");
            carsForm.NavigateToResearchPage();
            var researchForm = new ResearchForm();
            researchForm.AssertIsOpen();
            Assert.IsTrue(researchForm.IsCompareModelLinkPresent(), "CompareModel Link is not present");
            researchForm.ClickOnCompareModelLink();

            LogStep(steps++, "Select for comparer cars that were selected in previews steps");
            var compareForm = new CompareCarsForm();
            compareForm.AssertIsOpen();
            SelectValueInDropDownForComparison(valuesDropDownForFirstCar, parametersForFirstCar);
            SelectValueInDropDownForComparison(valuesDropDownForSecondCar, parametersForSecondCar);
            compareForm.ClickOnSeeComparisonButton();

            LogStep(steps++, "Catch the parameters and check that it equals for cars");
            cardCount = 1;
            comparedParametersForFirstCar = CatchParametersForCompare();
            comparedParametersForSecondCar = CatchParametersForCompare();
            Checker.IsTrue(parametersForFirstCar.Equals(comparedParametersForFirstCar), "Parameters for first car are equals");
            Checker.IsTrue(parametersForSecondCar.Equals(comparedParametersForSecondCar), "Parameters for second car are not equals");                      
        }       

        private CarsParametersModel CatchParametersForCompare()
        {
            var compareForm = new CompareCarsForm();
            var carsModel = new CarsParametersModel
            {
                Style = compareForm.GetStyleDescriptionForCar(cardCount),
                Engine = string.IsNullOrWhiteSpace(compareForm.GetComparerParametersForCar(TrimParametersEnums.Engine, cardCount))                
                    ? "-"
                    : compareForm.GetComparerParametersForCar(TrimParametersEnums.Engine, cardCount).Split('\r').First().Replace(" (", "("),
                Transmission = string.IsNullOrWhiteSpace(compareForm.GetComparerParametersForCar(TrimParametersEnums.Transmission, cardCount))
                    ? "-"
                    : compareForm.GetComparerParametersForCar(TrimParametersEnums.Transmission, cardCount).Split('\r').First()
            };
            cardCount++;

            return carsModel;
        }

        private void SelectValueInDropDownForComparison(CarsDropDownModel dropdown, CarsParametersModel style)
        {            
            var compareForm = new CompareCarsForm();
            compareForm.ClickOnAddCardLink(compareForm.NumberCardsForCar[$"card{cardCount++}"]);
            Checker.IsTrue(compareForm.IsPopUpPresent(), "PopUp is not present");
            compareForm.SelectValueInModalDropDown(dropdown.Make, DropDownResearchEnums.Make);
            compareForm.SelectValueInModalDropDown(dropdown.Model, DropDownResearchEnums.Model);
            compareForm.SelectValueInModalDropDown(dropdown.Year, DropDownResearchEnums.Year);
            compareForm.SelectValueInModalDropDown(style.Style, DropDownResearchEnums.Trim);
            compareForm.ClickOnAddCarButtonModal();
        }

        private CarsParametersModel CatchParametersForCarModel()
        {
            var trimPageForm = new TrimComparePage();
            Assert.IsTrue(trimPageForm.GetTrimColumnsForCar().Any(), "Columns with parameters for car is not shown");
            var parametrsForCar = new CarsParametersModel
            {
                Style = trimPageForm.GetTrimParametrsForCar(TrimParametersEnums.Style),
                Engine = trimPageForm.GetTrimParametrsForCar(TrimParametersEnums.Engine).Contains(notAvailable)
                    ? "-"
                    : trimPageForm.GetTrimParametrsForCar(TrimParametersEnums.Engine).Replace(" (", "("),
                Transmission = trimPageForm.GetTrimParametrsForCar(TrimParametersEnums.Transmission).Contains(notAvailable)
                    ? "-"
                    : trimPageForm.GetTrimParametrsForCar(TrimParametersEnums.Transmission)
            };

            return parametrsForCar;
        }

        private CarsDropDownModel StepsToTrim()
        {
            var dropDownModel = new CarsDropDownModel();
            var navigationMenu = new NavigationMenuForm();
            navigationMenu.NavigateToResearchPage();
            do
            {
                LogStep(steps++, "Select random value for car and search the auto");
                var reserchForm = new ResearchForm();
                var listDropDownCount = false;
                while (listDropDownCount == false)
                {
                    dropDownModel = new CarsDropDownModel
                    {
                        Make = reserchForm.SelectRandomValueInDropDown(DropDownResearchEnums.Make),
                        Model = reserchForm.SelectRandomValueInDropDown(DropDownResearchEnums.Model),
                        Year = reserchForm.SelectRandomValueInDropDown(DropDownResearchEnums.Year)
                    };
                    if (dropDownModel.Year != allYearsValue)
                    {
                        listDropDownCount = true;
                    }
                }
                reserchForm.ClickOnResearchButton();

                LogStep(steps++, "Check that trims is shown and description for car is shown");
                var description = $"{dropDownModel.Year} {dropDownModel.Make} {dropDownModel.Model}";
                Checker.CheckContains(description, reserchForm.GetDescriptionsForCar(), $"Description {reserchForm.GetDescriptionsForCar()} on page is not equals {description}");

                LogStep(steps++, "Check that trim compare link is present and click on trim link");
                Checker.IsTrue(reserchForm.IsTrimLinkPresent(), "Trim compare link is not present");
                reserchForm.ClickOnTrimCompareLink();

                LogStep(steps++, "Check that page with parameters for auto is shown");
                var trimPageForm = new TrimComparePage();
                isTrimShown = trimPageForm.IsTrimParametersIsShown();
                if (isTrimShown == false)
                {
                    trimPageForm.NavigateToResearchPage();
                }
                else
                {
                    quantityOfRetry = 0;
                    Checker.IsTrue(trimPageForm.IsTrimParametersIsShown(), "Trim with parameters for car is not shown");
                    trimPageForm.ClickOnBaseSectionForCar();
                }
                quantityOfRetry++;
            }
            while (OuantityOfRepeatCycle > quantityOfRetry && isTrimShown == false);

            return dropDownModel;
        }
    }
}
