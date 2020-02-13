using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DiscountAlert.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Opera;

namespace DiscountAlert.WebDriver
{
    public class GEWebDriver : IGEWebDriver
    {
        public IWebDriver _driver { get; set; }

        private IGEWebElement _rootElement;

        private IGERetryMechanism _retryMechanism;

        public GEWebDriver(IGERetryMechanism retryMechanism)
        {

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                _driver = InitialDriver("windows");
                // Do something for Windows
            }
            else
            { // Do something for Linux
                _driver = InitialDriver("linux");
            }

            _retryMechanism = retryMechanism;

            _rootElement = new GEWebElement(NativeRootElement, _retryMechanism);
        }

        private IWebElement NativeRootElement
        {
            get
            {
                var rootPattern = By.XPath("/*");
                return _driver.FindElement(rootPattern);
            }
        }

        IGEWebElement IGESearchContext.Root => _rootElement;

        public IGEDriverState State => this.CalculateState(_driver);

        public IGEDriverState Navigate(string url)
        {
            Action<string> goToUrlAction = _driver.Navigate().GoToUrl;
            var invoked = _retryMechanism.InvokeAction(goToUrlAction, 3, url);

            return CalculateState(_driver);
        }

        public IGEDriverState CloseAllButThis()
        {
            String originalHandle = _driver.CurrentWindowHandle;

            //Do something to open new tabs

            foreach (string handle in _driver.WindowHandles)
            {
                if (!handle.Equals(originalHandle))
                {
                    _driver.SwitchTo().Window(handle);
                    CloseTab();
                    // _driver.Close();
                }
            }

            return CalculateState(_driver);
        }

        public IGEDriverState OpenNewTab(string url)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.open();");
            var state = Navigate(url);
            return state;
        }

        public IGEDriverState CloseAllAndOpen(string url)
        {
            var state = CloseAllButThis();
            var finalState = Navigate(url);
            return finalState;
        }

        public IGEDriverState MoveToAnchorTab()
        {
            var anchorTabId = _driver.WindowHandles[0];
            _driver.SwitchTo().Window(anchorTabId);
            return CalculateState(_driver);
        }

        public IList<byte> TakeScreenShot(IWebElement element = null) {
            Screenshot screenshot = null;
            if(element == null)
            {
                screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            }
            else
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element); 
                screenshot = ((ITakesScreenshot)element).GetScreenshot();
            }
                
            return screenshot.AsByteArray.ToList();
        }

        private IGEDriverState CalculateState(IWebDriver driver)
        {
            var url = driver.Url;
            var currentTabId = driver.CurrentWindowHandle;
            var openTabs = driver.WindowHandles.Count;

            return new DriverState(url, currentTabId, openTabs);
        }

        public IGEWebElement FindElement(string attribute, string value)
        {
            return _rootElement.FindElement(attribute, value);
        }

        public IGEWebElement FindElement(string tag, string attribute, string value)
        {
            return _rootElement.FindElement(tag, attribute, value);
        }

        public IGEWebElement FindElementByClassName(string className)
        {
            return _rootElement.FindElementByClassName(className);
        }

        public IGEWebElement FindElementById(string id)
        {
            return _rootElement.FindElementById(id);
        }

        public IGEWebElement FindElementByTag(string tag)
        {
            return _rootElement.FindElementByTag(tag);
        }

        public IGEWebElement FindElementByText(string text)
        {
            return _rootElement.FindElementByText(text);
        }
        public IGEWebElement FindElementByContainsText(string text)
        {
            return _rootElement.FindElementByContainsText(text);
        }

        public ReadOnlyCollection<IGEWebElement> FindElements(string tag, string attribute, string value)
        {
            return _rootElement.FindElements(tag, attribute, value);
        }

        public ReadOnlyCollection<IGEWebElement> FindElements(string attribute, string value)
        {
            return _rootElement.FindElements(attribute, value);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByClassName(string className)
        {
            return _rootElement.FindElementsByClassName(className);
        }
        public ReadOnlyCollection<IGEWebElement> FindElementsByTag(string tag)
        {
            return _rootElement.FindElementsByTag(tag);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByContainsText(string containText)
        {
            return _rootElement.FindElementsByContainsText(containText);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByText(string text)
        {
            return _rootElement.FindElementsByText(text);
        }

        private IWebDriver InitialDriver(string os, BrowserType browser = BrowserType.Chrome)
        {
            switch (browser)
            {
                case BrowserType.Chrome:
                    var options = new ChromeOptions();
                    // options.AddArguments("headless");
                    // todo : change it to relative
                    return new ChromeDriver("C:/git/DiscountAlert/WebDriverHandler/web-drivers/chrome/" + os + "/", options);
                case BrowserType.Opera:
                    var oOptions = new OperaOptions();
                    // oOptions.AddArguments("headless");
                    return new OperaDriver("./web-drivers/opera/" + os + "/", oOptions);
            }

            throw new System.Exception();
        }

        public IGEDriverState CloseTab()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.close();");
            int newTabIndex = _driver.WindowHandles.Count - 2;
            if (newTabIndex > 0)
                _driver.SwitchTo().Window(_driver.WindowHandles[newTabIndex]);
            return this.CalculateState(_driver);
        }

        public IGEDriverState OpenElementInNewTab(IGEWebElement element)
        {
            Actions newTab = new Actions(_driver);
            int newTabIndex = _driver.WindowHandles.Count;
            newTab.KeyDown(Keys.LeftControl).Click(element as IWebElement).KeyUp(Keys.LeftControl).Build().Perform();
            _driver.SwitchTo().Window(_driver.WindowHandles[newTabIndex]);
            return this.CalculateState(_driver);
        }
    }

    internal enum BrowserType
    {
        Chrome,
        Opera
    }
}