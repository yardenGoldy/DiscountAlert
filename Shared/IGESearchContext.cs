using System.Collections.Generic;
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
        string findUrls();
        ReadOnlyCollection<IGEWebElement> FindChildElementsByTagName(string tag);
        ReadOnlyCollection<IGEWebElement> FindElementsByTag(string tag);
        ReadOnlyCollection<IGEWebElement> FindElementsByText(string text);
        ReadOnlyCollection<IGEWebElement> FindElementsByContainsText(string containText);
        bool IsElementExist(string tag, string attribute, string value);
        bool IsElementExist(string attribute, string value);
        bool IsElementExistByContainText(string containText);
    }
}