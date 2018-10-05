using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
//using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;
//using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeleniumExtras.PageObjects;

namespace Access2Justice.E2ETests.PageObjects
{
    class GuidedAssistant
    {
        private IWebDriver driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        public void NavigateToGuidedAssistantPage()
        {
            driver.Url = ScenarioContext.Current["baseUrl"] + "guidedassistant";
            StringAssert.Contains(driver.Url, "guidedassistant");
        }

        public void ClickButton(string buttonId)
        {

            IWebElement buttonToClick = fluentWait.Until(d =>
            {
                List<IWebElement> overlay = driver.FindElements(By.ClassName("black-overlay")).ToList();
                IWebElement button = driver.FindElement(By.Id(buttonId));
                if (button != null && button.Displayed && button.Enabled && overlay.Count == 0)
                {
                    return button;
                }

                return null;
            });

            buttonToClick.Click();
        }
    }
}
