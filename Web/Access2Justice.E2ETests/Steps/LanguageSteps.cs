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
        HomePage HomePage = new HomePage();

        [When(@"I select a language from the navigation bar")]
        public void WhenISelectALanguageFromTheNavigationBar(dynamic instance)
        {
            HomePage.PickLanguage(instance.SelectedLanguage);        
        }
        
        [Then(@"I should see my page translated")]
        public void ThenIShouldSeeMyPageTranslated(dynamic instance)
        {
            HomePage.ConfirmPageTranslated(instance.ExpectedLanguage);
        }
    }
}
