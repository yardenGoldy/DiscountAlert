using System.Collections.Generic;
using OpenQA.Selenium;

namespace DiscountAlert.Shared
{
    public interface IGEWebDriver : IGESearchContext
    {
        IWebDriver _driver{ get; }
        IGEDriverState State { get; }
        IGEDriverState Navigate(string url);
        IGEDriverState OpenNewTab(string url);
        IGEDriverState OpenElementInNewTab(IGEWebElement element);
        // IGEDriverState MakeCurrentTabFirst();
        IGEDriverState CloseAllButThis();
        IGEDriverState CloseAllAndOpen(string url);
        IGEDriverState CloseTab();
        IGEDriverState MoveToAnchorTab();
        IList<byte> TakeScreenShot(IWebElement element = null);
    }
}
