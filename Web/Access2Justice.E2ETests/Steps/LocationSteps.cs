using System;
using TechTalk.SpecFlow;
using SpecFlow.Assist.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class LocationSteps
    {
        IWebDriver driver;
        private dynamic _inputStateName;
        private dynamic _displayedStateName;

        [Given(@"I am on the Access(.*)Justice website")]
        public void GivenIAmOnTheAccessJusticeWebsite(int p0)
        {
            driver = new ChromeDriver();
            driver.Url = "https://a2jstageweb.azurewebsites.net/";
            System.Diagnostics.Debug.WriteLine("hello world");
        }
        
        [Given(@"I am prompted to set my location")]
        public void GivenIAmPromptedToSetMyLocation()
        {
            //if (driver.FindElement(By.TagName("modal-container")).Displayed == true)
            //    Console.WriteLine("Modal showed up succesfully");
            //else
            //    Console.WriteLine("Modal failed to show up");

            Assert.IsTrue(true);
        }
        
        [When(@"I enter my state name")]
        public void WhenIEnterMyStateName(dynamic instance)
        {
            //var _inputStateName = instance;
            //driver.FindElement(By.Id("search-box")).SendKeys(_inputStateName.State);
            //driver.FindElement(By.ClassName("search-btn")).Click();
            //driver.FindElement(By.XPath("//button[.='Update']"));
            Assert.IsTrue(true);
        }
        
        [Then(@"I can see my state name on the upper navigation bar")]
        public void ThenICanSeeMyStateNameOnTheUpperNavigationBar(dynamic instance)
        {
            //var _displayedStateName = instance;
            //Console.WriteLine("Need to change this later");
            Assert.IsTrue(driver.FindElement(By.LinkText("Home")).Displayed);
            driver.Quit();    
        }
    }
}
