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
    class SharedObjects
    {
        private IWebDriver driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        public void ClickButton(string buttonId)
        {
            IWebElement buttonToClick = driver.FindElement(By.Id(buttonId));

            fluentWait.Until(d =>
            {
                List<IWebElement> canvas = d.FindElements(By.Id("labelCanvasId")).ToList();
                List<IWebElement> overlay = d.FindElements(By.ClassName("black-overlay")).ToList();

                if (buttonToClick != null && buttonToClick.Displayed && buttonToClick.Enabled && canvas.Count == 0 && overlay.Count == 0)
                {
                    buttonToClick.Click();
                    return true;
                }

                return false;
            });
        }
    }
}
