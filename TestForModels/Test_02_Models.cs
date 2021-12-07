using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageForms.Enums.DropDownResearchEnums;
using PageForms.Enums.TrimParametrs;
using PageForms.ModelCars.CarsForm;
using PageForms.ModelCars.CompareCarsForm;
using PageForms.ModelCars.Models.CarsDropDownModel;
using PageForms.ModelCars.Models.CarsParametrsModel;
using PageForms.ModelCars.NavigationMenu;
using PageForms.ModelCars.ResearchForm;
using System;
using System.Collections.Generic;
using System.Linq;
using TestEnviroment.BrowserStart;
using WebdriverFramework.Framework.WebDriver;

namespace TestForModels
{
    [TestClass]
    public class Test_02_Models : BaseTest
    {
        private CarsDropDownModel ValuesDropDownForFirstCar;
        private CarsDropDownModel ValuesDropDownForSecondCar;
        private CarsParametrsModel ParametrsForFirstCar;
        private CarsParametrsModel ParametrsForSecondCar;
        private CarsParametrsModel ComparerParametrsForFirstCar;
        private CarsParametrsModel ComparerParametrsForSecondCar;

        private const int OuantityOfRepitCycle = 10;
        private int steps;
        private readonly string AllYearsValue = "All years";
        private bool isTrimShown;
        private int QuantityOfRetry = 0;
        private int CardCount = 1;

        [TestInitialize]
        public void TestInitialize()
        {            
            new BrowserStart().NavigateToMainPage();
        }

        [TestMethod]
        public override void RunTest()
        {
            LogStep(steps++, "Check that main page open and go to Research page");
            var carsForm = new CarsForm();
            carsForm.AssertIsOpen();

            LogStep(steps++, "Go to Research page and do steps for check that trim parameters is shown for car");
            ValuesDropDownForFirstCar = StepsToTrim();

            LogStep(steps++, "Catch the parameters for base first trim: Available Engines, Transmissions");
            ParametrsForFirstCar = CatchParametersForCarModel();
            carsForm.NavigateToMainPage();

            LogStep(steps++, "Catch the parameters for second car");            
            ValuesDropDownForSecondCar = StepsToTrim();
            ParametrsForSecondCar = CatchParametersForCarModel();

            LogStep(steps++, "Navigate to research page and click Side-bySide Comparisons");
            carsForm.NavigateToReserchPage();
            var researchForm = new ResearchForm();
            researchForm.AssertIsOpen();
            Assert.IsTrue(researchForm.IsCompareModelLinkPresent(), "CompareModel Link is not present");
            researchForm.ClickOnCompareModelLink();

            LogStep(steps++, "Select for comparer cars that were selected in previews steps");
            var compareForm = new CompareCarsForm();
            compareForm.AssertIsOpen();
            SelecteValueInDropDownForComparison(ValuesDropDownForFirstCar, ParametrsForFirstCar);
            SelecteValueInDropDownForComparison(ValuesDropDownForSecondCar, ParametrsForSecondCar);
            compareForm.ClickOnSeeComparisonButton();

            LogStep(steps++, "Catch the parameters and check that it equals for cars");
            CardCount = 1;
            ComparerParametrsForFirstCar = CatchParametrsForCompare();
            ComparerParametrsForSecondCar = CatchParametrsForCompare();
            CheckThatModelsAreEquals(ParametrsForFirstCar, ComparerParametrsForFirstCar);
            CheckThatModelsAreEquals(ParametrsForSecondCar, ComparerParametrsForSecondCar);
        }

        private void CheckThatModelsAreEquals(CarsParametrsModel model, CarsParametrsModel model2)
        {
            Checker.CheckEquals(model.Style, model2.Style, $"{model.Style}are not equal {model2.Style} in first car");
            Checker.CheckEquals(model.Engine, model2.Engine, $"{model.Engine}are not equal {model2.Engine} in first car");
            Checker.CheckEquals(model.Transmission, model2.Transmission, $"{model.Transmission}are not equal {model2.Transmission} in first car");
        }

        private CarsParametrsModel CatchParametrsForCompare()
        {
            var compareForm = new CompareCarsForm();
            var carsModel = new CarsParametrsModel
            {
                Style = compareForm.GetStyleDescriptionForCar(CardCount),
                Engine = compareForm.GetComparerParametrsForCar(TrimParametrsEnums.Engine, CardCount).Contains("is not available")
                ? "-"
                : compareForm.GetComparerParametrsForCar(TrimParametrsEnums.Engine, CardCount),
                Transmission = compareForm.GetComparerParametrsForCar(TrimParametrsEnums.Transmission, CardCount).Split('\r').First().Contains("is not available")
                ? "-"
                : compareForm.GetComparerParametrsForCar(TrimParametrsEnums.Transmission, CardCount).Split('\r').First()
            };
            CardCount++;
            return carsModel;
        }

        private void SelecteValueInDropDownForComparison(CarsDropDownModel dropdown, CarsParametrsModel style)
        {            
            var compareForm = new CompareCarsForm();
            compareForm.ClickOnAddCardLink(compareForm.NumberCardsForCar[$"card{CardCount++}"]);
            Checker.IsTrue(compareForm.IsPopUpPresent(), "PopUp is not present");
            compareForm.SelectValueInModalDropDown(dropdown.Make, DropDownResearchEnums.Make);
            compareForm.SelectValueInModalDropDown(dropdown.Model, DropDownResearchEnums.Model);
            compareForm.SelectValueInModalDropDown(dropdown.Year, DropDownResearchEnums.Year);
            compareForm.SelectValueInModalDropDown(style.Style, DropDownResearchEnums.Trim);
            compareForm.ClickOnAddCarButtonModal();
        }

        private CarsParametrsModel CatchParametersForCarModel()
        {
            var trimPageForm = new TrimComparePage();
            Assert.IsTrue(trimPageForm.GetTrimColumnsForCar().Any(), "Columns with parameters for car is not shown");
            var parametrsForCar = new CarsParametrsModel
            {
                Style = trimPageForm.GetTrimParametrsForCar(TrimParametrsEnums.Style),
                Engine = trimPageForm.GetTrimParametrsForCar(TrimParametrsEnums.Engine),
                Transmission = trimPageForm.GetTrimParametrsForCar(TrimParametrsEnums.Transmission)
            };
            return parametrsForCar;
        }

        private CarsDropDownModel StepsToTrim()
        {
            var dropDownModel = new CarsDropDownModel();
            var navigationMenu = new NavigationMenuForm();
            navigationMenu.NavigateToReserchPage();
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
                    if (dropDownModel.Year != AllYearsValue)
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
                isTrimShown = trimPageForm.IsTrimParametrsIsShown();
                if (isTrimShown == false)
                {
                    trimPageForm.NavigateToReserchPage();
                }
                else
                {
                    QuantityOfRetry = 0;
                    Checker.IsTrue(trimPageForm.IsTrimParametrsIsShown(), "Trim with parameters for car is not shown");
                    trimPageForm.ClickOnBaseSectionForCar();
                }
                QuantityOfRetry++;
            }
            while (OuantityOfRepitCycle > QuantityOfRetry && isTrimShown == false);
            return dropDownModel;
        }
    }
}
