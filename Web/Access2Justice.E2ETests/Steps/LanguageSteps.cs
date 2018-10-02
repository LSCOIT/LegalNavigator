﻿using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Access2Justice.E2ETests.PageObjects;

namespace Access2Justice.E2ETests
{
    [Binding]
    public class SetLanguageSteps : TechTalk.SpecFlow.Steps
    {
        private IWebDriver driver => ScenarioContext.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Get<DefaultWait<IWebDriver>>("fluentWait");
        private HomePage HomePage;

        [When(@"I select a language from the navigation bar")]
        public void WhenISelectALanguageFromTheNavigationBar()
        {
            
        }
        
        [Then(@"I should see my page translated")]
        public void ThenIShouldSeeMyPageTranslated()
        {
            Assert.IsTrue(true);
        }
    }
}
