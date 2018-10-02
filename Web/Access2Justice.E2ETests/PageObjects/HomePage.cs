using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Access2Justice.E2ETests.PageObjects
{
    class HomePage 
    {
        private readonly IWebDriver _driver;
        private readonly DefaultWait<IWebDriver> _fluentWait;
        [FindsBy(How = How.Id, Using = "language - dropdown")]
        private IWebElement setLanguageButton;

        public HomePage(IWebDriver driver, DefaultWait<IWebDriver> fluentWait)
        {
            _driver = driver;
            _fluentWait = fluentWait;
        }

        public void clickLanguageButton()
        {
            IWebElement setLanguageButton;

            setLanguageButton = fluentWait.Until(d =>
            {
                IWebElement element = driver.FindElement(By.Id("language-dropdown"));
                List<IWebElement> element2 = d.FindElements(By.TagName("modal-container")).ToList();
                if (element != null && element.Displayed && element.Displayed && element2.Count == 0)
                {
                    return element;
                }

                return null;
            });

            setLanguageButton.Click();
            Assert.IsTrue(driver.FindElement(By.Id("language-dropdown")).Displayed);
        }
    }
}
