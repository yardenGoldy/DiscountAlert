using System.Collections.Generic;

namespace DiscountAlert.Shared
{
    public interface IGEWebDriver : IGESearchContext
    {
        IGEDriverState State { get; }
        IGEDriverState Navigate(string url);
        IGEDriverState OpenNewTab(string url);
        IGEDriverState OpenElementInNewTab(IGEWebElement element);
        // IGEDriverState MakeCurrentTabFirst();
        IGEDriverState CloseAllButThis();
        IGEDriverState CloseAllAndOpen(string url);
        IGEDriverState CloseTab();
        IGEDriverState MoveToAnchorTab();
        IList<byte> TakeScreenShot(IGEWebElement element = null);
    }
}
