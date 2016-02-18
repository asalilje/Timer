using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Timer
{
    class Program
    {
        static void Main(string[] args)
        {
            var mobsters = new List<string> { "Martin", "Håkan", "John", "Petter", "Per", "Åsa" }
                .OrderBy(a => Guid.NewGuid()).ToList();
            var url = "http://oss.jahed.io/agility/timer.html";
            var options = new ChromeOptions();
            options.AddArguments("--lang=sv");
            var webdriver = new ChromeDriver(options);
            webdriver.Url = url;
            webdriver.Navigate();

            var settingsButton = webdriver.WaitGetElement(By.ClassName("nav-timer-settings"), 10, true);
            settingsButton.Click();

            var periodInput = webdriver.WaitGetElement(By.Id("period"), 10, true);

            periodInput.SendKeys("12:00");

            var addMobsterButton = webdriver.WaitGetElement(By.ClassName("add-mobster"), 10, true);

            mobsters.ForEach(x => addMobsterButton.Click());

            var mobsterInputs = webdriver.FindElementsByClassName("mobster-control-name");

            for (var i = 0; i < mobsters.Count; i++)
            {
                mobsterInputs[i].Clear();
                mobsterInputs[i].SendKeys(mobsters[i]);
            }

        }


    }

    public static class WebdriverExtensions
    {
        public static IWebElement WaitGetElement(this IWebDriver driver, By by, int timeoutInSeconds, bool checkIsVisible = false)
        {
            IWebElement element;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                if (checkIsVisible)
                {
                    element = wait.Until(ExpectedConditions.ElementIsVisible(by));
                }
                else
                {
                    element = wait.Until(ExpectedConditions.ElementExists(by));
                }
            }
            catch (NoSuchElementException) { element = null; }
            catch (WebDriverTimeoutException) { element = null; }
            catch (TimeoutException) { element = null; }

            return element;
        }
    }
}

