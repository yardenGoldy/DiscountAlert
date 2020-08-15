namespace DiscountAlert.Shared
{
    public interface IGEWebElement : IGESearchContext
    {
        IGEWebElement Parent { get; }
        string Text { get; }
        string TagName { get; }
        void SetValueLikeHuman(string value);

        string GetAttribute(string attribute);
        void Click();
        object SourceElement { get;}
    }
}
