using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageForms.ModelCars.CarsForm;
using PageForms.ModelCars.ResearchForm;
using System;
using System.Linq;
using TestEnviroment.BrowserStart;
using WebdriverFramework.Framework.WebDriver;

namespace TestForModels
{
    [TestClass]
    public class Test_02_Models : BaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            new BrowserStart().NavigateToMainPage();
        }

        [TestMethod]
        public override void RunTest()
        {
            LogStep(1, "Check that main page open and go to Research page");
            var carsForm = new CarsForm();            
            carsForm.AssertIsOpen();
            carsForm.NavigateToReserchPage();
            var reserchForm = new ResearchForm();
            reserchForm.AssertIsOpen();

            LogStep(2, "Select rundom value for car and search the auto");
            //reserchForm.SelectValueInMakeDropDown("Acura");
            var listMake = reserchForm.GetElementFromMakeDropDown();
            var rnd = new Random();
            var numberOfValueMake = rnd.Next(0, listMake.Count);
            var textElement = listMake.ElementAt(numberOfValueMake);
            reserchForm.SelectValueInMakeDropDown(textElement);
        }       
    }
}
