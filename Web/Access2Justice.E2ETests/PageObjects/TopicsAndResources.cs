using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TechTalk.SpecFlow.Assist;
//using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;


namespace Access2Justice.E2ETests.PageObjects
{
    class TopicsAndResources
    {
        #region
        private IWebDriver Driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> FluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");
        #endregion

        public void ClickOnTopicByCssSelector(string cssSelector)
        {
            FluentWait.Until(d =>
            {
                IWebElement topic = Driver.FindElement(By.CssSelector(cssSelector));
                if (topic != null && topic.Displayed)
                {
                    topic.Click();
                    return true;
                }

                return false;
            });
        }

        public void ConfirmResourcesAreShown()
        {
            FluentWait.Until(d =>
            {
                IList<IWebElement> resources = Driver.FindElements(By.TagName("app-resource-card"));
                if (resources != null && resources.Count > 0)
                {
                    Assert.IsTrue(resources.Count > 0);
                    return true;
                }

                return false;
            });
        }

        public void ConfirmServiceOrganizationSideBarIsDisplayed()
        {
            IWebElement organizationSideBar = Driver.FindElement(By.TagName("app-service-org-sidebar"));
            Assert.IsTrue(organizationSideBar.Displayed);
        }
    }
}
