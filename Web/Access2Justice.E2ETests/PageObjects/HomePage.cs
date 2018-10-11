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
    class HomePage 
    {
        private IWebDriver driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        public HomePage()
        {
            // Need to find another solution for PageFactory in the future: 
            // https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras
            PageFactory.InitElements(driver, this);
        }

        public void ConfirmPageTitle()
        {
            Assert.AreEqual(driver.Title, "Access to Justice");
        }

        public void ClickButton(string buttonId)
        {

            IWebElement buttonToClick = fluentWait.Until(d =>
            {
                IWebElement button = driver.FindElement(By.Id(buttonId));
                if (button != null && button.Displayed && button.Enabled)
                {
                    return button;
                }

                return null;
            });

            buttonToClick.Click();
        }

        public void ConfirmOnHomePage()
        {
            Assert.AreEqual(driver.Url, ScenarioContext.Current["baseUrl"]);
        }
    }
}
