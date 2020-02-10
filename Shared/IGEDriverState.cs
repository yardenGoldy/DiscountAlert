namespace DiscountAlert.Shared
{
    public interface IGEDriverState
    {
        string Url { get; }
        string CurrentTabId { get; }
        int OpenTabs { get; }
    }
}