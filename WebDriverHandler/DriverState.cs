
using DiscountAlert.Shared;
namespace DiscountAlert.WebDriver
{
    internal class DriverState : IGEDriverState
    {
        public DriverState(string url, string currentTabId, int openTabs)
        {
            Url = url;
            CurrentTabId = currentTabId;
            OpenTabs = openTabs;
        }
        public string Url { get; }
        public string CurrentTabId { get; }
        public int OpenTabs { get; }
    }
}