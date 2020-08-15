using System;
using System.Collections.Generic;
using System.Linq;
using DiscountAlert.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Opera;

namespace DiscountAlert.WebDriver
{
    public class GEWebDriver : GEElementFinder<IWebDriver>, IGEWebDriver
    {
        private IWebDriver _driver;

        public GEWebDriver(IGERetryMechanism retryMechanism)
            : base(retryMechanism)
        {
            _driver = InitialDriver();
        }

        protected override ref IWebDriver relativeRootElement => ref _driver;

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

        private IGEDriverState CalculateState(IWebDriver driver)
        {
            var url = driver.Url;
            var currentTabId = driver.CurrentWindowHandle;
            var openTabs = driver.WindowHandles.Count;

            return new DriverState(url, currentTabId, openTabs);
        }

        private IWebDriver InitialDriver(BrowserType browser = BrowserType.Chrome)
        {
            // We are at path 'projectDirectory\\src\\Part-A-Worker'
            var os = WhatOSAmI();
            var toProjectDirectory = "..\\";
            var driverPath = $"{toProjectDirectory}\\web-drivers\\chrome\\{os}\\";

            if (os == "linux") driverPath = $"./{driverPath.Replace('\\', '/')}";

            switch (browser)
            {
                case BrowserType.Chrome:
                    var options = new ChromeOptions();
                    // options.AddArguments("headless");
                    // options.AddArgument("port=49675");
                    // options.BrowserVersion = "81.0.4044.20";
                    // options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome Beta\Application";
                    // options.AddArgument("chromever=81.0.4044.20");
                    options.AddArgument("no-sandbox");
                    options.AddArgument("--no-sandbox");
                    return new ChromeDriver(driverPath, options);
                case BrowserType.Opera:
                    var oOptions = new OperaOptions();
                    // options.AddArguments("headless");
                    return new OperaDriver(driverPath, oOptions);
            }

            throw new System.Exception();
        }

        public IGEDriverState CloseTab()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.close();");
            int lastOpenTabIndex = _driver.WindowHandles.Count - 2;
            if (lastOpenTabIndex > 0)
                _driver.SwitchTo().Window(_driver.WindowHandles[lastOpenTabIndex]);
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

        public IList<byte> TakeScreenShot(IGEWebElement element = null)
        {
            Screenshot screenshot = null;
            if (element == null)
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

        public void Dispose()
        {
            _driver.Dispose();
        }

        public IGEDriverState Restart()
        {
            Dispose();
            _driver = InitialDriver();
            return State;
        }

        private string WhatOSAmI()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                return "windows";
                // Do something for Windows
            }
            else
            { // Do something for Linux
                return "linux";
            }
        }
    }

    internal enum BrowserType
    {
        Chrome,
        Opera
    }
}