using System.Collections.ObjectModel;

namespace DiscountAlert.Shared
{
    public interface IGESearchContext
    {
        IGEWebElement FindElementById(string id);
        IGEWebElement FindElementByClassName(string className);
        IGEWebElement FindElementByTag(string tag);
        IGEWebElement FindElement(string attribute, string value);
        IGEWebElement FindElement(string tag, string attribute, string value);
        IGEWebElement FindElementByText(string text);
        IGEWebElement FindElementByContainsText(string text);
        IGEWebElement Root { get; }
        ReadOnlyCollection<IGEWebElement> FindElements(string tag, string attribute, string value);
        ReadOnlyCollection<IGEWebElement> FindElements(string attribute, string value);
        ReadOnlyCollection<IGEWebElement> FindElementsByClassName(string className);
        ReadOnlyCollection<IGEWebElement> FindElementsByTag(string tag);
        ReadOnlyCollection<IGEWebElement> FindElementsByText(string text);
        ReadOnlyCollection<IGEWebElement> FindElementsByContainsText(string containText);
    }
}