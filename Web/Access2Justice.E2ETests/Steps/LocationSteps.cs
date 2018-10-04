/*
 * Controls are located in shared->map->map.component.html file 
 * How to use css selector: http://www.binaryclips.com/2016/03/css-selectors-for-selenium-webdriver.html
 * How to configure Chrome browser: http://chromedriver.chromium.org/capabilities
 * Explicit Wait: http://toolsqa.com/selenium-webdriver/c-sharp/advance-explicit-webdriver-waits-in-c/
 * Implicit Wait, Explicit Wait or Fluent Wait: https://www.guru99.com/implicit-explicit-waits-selenium.html
 * Sample waiting code: https://gist.github.com/up1/d925783ea8e5f706f3bb58c3ce1ef7eb
 * Wait till element is clickable: https://stackoverflow.com/questions/16057031/webdriver-how-to-wait-until-the-element-is-clickable-in-webdriver-c-sharp
 */

using System;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using Access2Justice.E2ETests.PageObjects;


namespace Access2Justice.E2ETests.Steps 
{
    [Binding]
    public class LocationSteps : TechTalk.SpecFlow.Steps
    {
        HomePage HomePage = new HomePage();

        [Given(@"I am on the Access2Justice website with location detection blocked")]
        public void GivenIAmOnTheAccessJusticeWebsiteWithLocationDetectionBlocked()
        {
            HomePage.ConfirmPageTitle();
        }

        [Given(@"I am on the Access2Justice website with state set")]
        public void GivenIAmOnTheAccessJusticeWebsiteWithStateSet(dynamic instance)
        {
            HomePage.ConfirmPageTitle();
            GivenIAmPromptedToSetMyLocation();
            WhenIEnterMyStateName(instance);
            ThenICanSeeMyStateNameOnTheUpperNavigationBar(instance);
        }

        [Given(@"I am prompted to set my location")]
        public void GivenIAmPromptedToSetMyLocation()
        {
            HomePage.ConfirmModalPresent();
        }

        [When(@"I click on the Change button")]
        public void WhenIClickOnTheChangeButton()
        {
            HomePage.ClickChangeLocationButton();
        }


        [When(@"I enter my state name")]
        public void WhenIEnterMyStateName(dynamic instance)
        {
            HomePage.EnterStateName(instance.State);
        }

        [Then(@"I can see my state name on the upper navigation bar")]
        public void ThenICanSeeMyStateNameOnTheUpperNavigationBar(dynamic instance)
        {
            HomePage.ConfirmStateNameHasBeenSet(instance.State);
        }
    }
}
