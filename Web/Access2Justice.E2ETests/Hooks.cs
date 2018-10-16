/*
 * Used to perform automation logic
 * Documentation: https://specflow.org/documentation/hooks/
 * For more info, check https://www.automatetheplanet.com/extend-test-execution-workflow-specflow-hooks/
 * Example repo: https://github.com/LirazShay/SpecFlowDemo/blob/master/src/SpecFlowDemo/Steps/LoginPageSteps.cs
 */

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests
{
    [Binding]
    public sealed class Hooks : TechTalk.SpecFlow.Steps
    {
        #region
        protected IWebDriver driver;
        protected string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        protected string browser = ConfigurationManager.AppSettings["browser"];
        protected int globalTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
        #endregion

        [BeforeScenario]
        public void SetUp()
        {
            SetUpBrowser();

            SetUpSmartWait();
        }

        [AfterScenario]
        public void CleanUp()
        {
            CloseBrowser();
        }

        /// <summary>
        /// Sets up the appropriate WebDriver
        /// </summary>
        public void SetUpBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            // Start Chrome maximized
            //options.AddArguments("start-maximized");
            options.AddArguments("--disable-gpu");
            // Disable the "Know your location" pop up
            options.AddUserProfilePreference("profile.managed_default_content_settings.geolocation", 2);
            options.AddArguments("--window-size=1024,768");
            // Start ChromeDriver in headless mode
            options.AddArguments("headless");
            driver = new ChromeDriver(options);
            driver.Url = baseUrl;
            ScenarioContext.Add("driver", driver);
            ScenarioContext.Add("baseUrl", driver.Url);
        }

        /// <summary>
        /// Configures smart wait that waits for a certain condition to be true
        /// </summary>
        public void SetUpSmartWait()
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(globalTimeOut);
            // Verify the condition every 200 miliseconds
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(200);
            fluentWait.IgnoreExceptionTypes(typeof(WebDriverException));
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(TimeoutException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementNotVisibleException));
            fluentWait.Message = "It took too long to find the element.";
            ScenarioContext.Add("fluentWait", fluentWait);
        }

        /// <summary>
        /// Closes the browser window when test is finished
        /// </summary>
        public void CloseBrowser()
        {
            ScenarioContext.Get<IWebDriver>("driver").Quit();
        }
    }
}
