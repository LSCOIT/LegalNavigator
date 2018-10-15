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
    class TopicsAndResources
    {
        private IWebDriver driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        public void ConfirmOnTopicsAndResourcesPage()
        {
            StringAssert.Contains(driver.Url, "topics");
        }

        public void ClickOnTopic()
        {
            IWebElement topic = fluentWait.Until(d =>
            {
                IWebElement elem = driver.FindElement(By.CssSelector(".topic-list a"));
                if (elem != null && elem.Displayed)
                {
                    return elem;
                }

                return null;
            });
            topic.Click();
        }

        public void ClickOnSubtopic()
        {
            IWebElement subtopic = fluentWait.Until(d =>
            {
                IWebElement elem = driver.FindElement(By.CssSelector(".subtopics ul li a"));
                if (elem != null && elem.Displayed)
                {
                    return elem;
                }

                return null;
            });
            subtopic.Click();
        }

        public void ConfirmResourcesAreShown()
        {
            List<IWebElement> resources; 
            resources = fluentWait.Until(d =>
            {
                List<IWebElement> elements = driver.FindElements(By.TagName("app-resource-card")).ToList();
                if (elements != null && elements.Count > 0)
                {
                    return elements;
                }

                return null;
            });
            
            Assert.IsTrue(resources.Count > 0);
        }

        public void ConfirmServiceOrganizationSideBarIsDisplayed()
        {
            Assert.IsTrue(driver.FindElement(By.TagName("app-service-org-sidebar")).Displayed);
        }
    }
}
