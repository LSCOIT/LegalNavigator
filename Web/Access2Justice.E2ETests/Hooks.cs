/*
 * Used to perform automation logic
 * Documentation: https://specflow.org/documentation/hooks/
 * For more info, check https://www.automatetheplanet.com/extend-test-execution-workflow-specflow-hooks/
 * Example repo: https://github.com/LirazShay/SpecFlowDemo/blob/master/src/SpecFlowDemo/Steps/LoginPageSteps.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace Access2Justice.E2ETests
{
    [Binding]
    public sealed class Hooks : TechTalk.SpecFlow.Steps
    {

        //[BeforeScenario]
        //public void OutputScenario()
        //{
        //    Console.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
        //    Console.ReadLine();
        //    Console.WriteLine(FeatureContext.Current.FeatureInfo.Description);
        //    Console.WriteLine("\r\nScenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        //}

        [BeforeScenario]
        public void OpenBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            // Start Chrome maximized
            options.AddArguments("start-maximized");
            // Disable the "Know your location" pop up
            options.AddUserProfilePreference("profile.managed_default_content_settings.geolocation", 2);
            IWebDriver driver = new ChromeDriver(options);
            driver.Url = "http://localhost:5150/";
            ScenarioContext.Add("driver", driver);
            ScenarioContext.Add("baseUrl", driver.Url);

            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(20);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(200);
            fluentWait.IgnoreExceptionTypes(typeof(WebDriverException));

            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(TimeoutException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementNotVisibleException));
            fluentWait.Message = "Why :(((((((";
            ScenarioContext.Add("fluentWait", fluentWait);
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            ScenarioContext.Get<IWebDriver>("driver").Quit();
        }
    }
}
