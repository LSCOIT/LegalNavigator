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

            IWebElement buttonToClick = fluentWait.Until(d =>
            {
                IWebElement button = driver.FindElement(By.Id(buttonId));
                List<IWebElement> canvas = d.FindElements(By.Id("labelCanvasId")).ToList();
                List<IWebElement> overlay = d.FindElements(By.ClassName("black-overlay")).ToList();
                if (button != null && button.Displayed && button.Enabled && canvas.Count == 0 && overlay.Count == 0)
                {
                    return button;
                }

                return null;
            });

            buttonToClick.Click();
        }
    }
}
