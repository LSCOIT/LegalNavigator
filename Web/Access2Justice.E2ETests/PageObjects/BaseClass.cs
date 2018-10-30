using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests.PageObjects
{
    class BaseClass
    {
        #region
        private IWebDriver Driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> FluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");
        #endregion

        //public BaseClass()
        //{
        //    // Need to find another solution for PageFactory in the future: 
        //    // https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras
        //    PageFactory.InitElements(Driver, this);
        //}

        public void ClickOnControl(string Id)
        {
            FluentWait.Until(d =>
            {
                IWebElement control = Driver.FindElement(By.Id(Id));

                IList<IWebElement> canvas = Driver.FindElements(By.ClassName("labelCanvasId"));
                IList<IWebElement> overlay = Driver.FindElements(By.ClassName("black-overlay"));

                if (control != null && control.Displayed && control.Enabled && 
                    canvas.Count == 0 && overlay.Count == 0)
                {
                    control.Click();
                    return true;
                }

                return false;
            });
        }

        public void ConfirmOnCorrectPage(string pageName)
        {
            StringAssert.Contains(Driver.Url, pageName);
        }

        public void ConfirmSiteIsA2J()
        {
            Assert.AreEqual(Driver.Title, "Access to Justice");
        }
    }
}
