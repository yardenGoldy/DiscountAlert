
using DiscountAlert.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
namespace DiscountAlert.WebDriver
{
    public abstract class GEElementFinder<T> : IGESearchContext
        where T : ISearchContext
    {
        protected IGERetryMechanism _retryMechanism;

        protected abstract ref T relativeRootElement { get; }

        public GEElementFinder(IGERetryMechanism retryMechanism)
        {
            _retryMechanism = retryMechanism;
        }

        public IGEWebElement Root => FindElementByXPath("/*");

        public IGEWebElement FindElement(string attribute, string value)
        {
            var attributeValuePattern = $".//*[@{attribute}='{value}']";
            return FindElementByXPath(attributeValuePattern);
        }

        public IGEWebElement FindElement(string tag, string attribute, string value)
        {
            // XPath pattern
            var tagAttributeValuePattern = $".//{tag}[@{attribute}='{value}']";
            return FindElementByXPath(tagAttributeValuePattern);
        }

        public IGEWebElement FindElementByClassName(string className)
        {
            return FindElement("class", className);
        }

        public IGEWebElement FindElementById(string id)
        {
            return FindElement("id", id);
        }

        public IGEWebElement FindElementByTag(string tag)
        {
            var allTagElementPattern = $".//{tag}";
            return FindElementByXPath(allTagElementPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindChildElementsByTagName(string tag)
        {
            var byTag = By.TagName(tag);
            var elements = _retryMechanism.InvokeFunc<T, ReadOnlyCollection<IWebElement>>(ref relativeRootElement, "FindElements", 3, byTag);

            return ConvertMany(elements);
        }

        public IGEWebElement FindElementByText(string text)
        {
            var findByTextPattern = $".//*[text()='{text}']";
            return FindElementByXPath(findByTextPattern);
        }
        public IGEWebElement FindElementByContainsText(string text)
        {
            var containTextPattern = $".//*[contains(text(), '{text}')]";
            return FindElementByXPath(containTextPattern);
        }


        public ReadOnlyCollection<IGEWebElement> FindElements(string tag, string attribute, string value)
        {
            var tagAttributeValuePattern = $".//{tag}[@{attribute}='{value}']";
            return FindElementsByXPath(tagAttributeValuePattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElements(string attribute, string value)
        {
            var attributeValuePattern = $".//*[contains(concat(' ', @{attribute}, ' '),' {value} ')]";
            return FindElementsByXPath(attributeValuePattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByClassName(string className)
        {
            return FindElements("class", className);
        }
        public ReadOnlyCollection<IGEWebElement> FindElementsByTag(string tag)
        {
            var allTagElementPattern = $".//{tag}";
            return FindElementsByXPath(allTagElementPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByContainsText(string containText)
        {
            var containTextPattern = $".//*[contains(text(), '{containText}')]";
            return FindElementsByXPath(containTextPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByContainsTextIgnoreSpaces(string containText)
        {
            var containTextPattern = $".//*[contains(normalize-space(), '{containText}')]";
            return FindElementsByXPath(containTextPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByText(string text)
        {
            var findByTextPattern = $".//*[text()='{text}']";
            return FindElementsByXPath(findByTextPattern);
        }

        public IGEWebElement FindElementByXPath(string pattern)
        {
            // XPath pattern
            var xpath = By.XPath(pattern);
            var element = _retryMechanism.InvokeFunc<T, IWebElement>(ref relativeRootElement, nameof(relativeRootElement.FindElement), 3, xpath);

            return ConvertOne(element);
        }
        internal ReadOnlyCollection<IGEWebElement> FindElementsByXPath(string pattern)
        {
            var xpath = By.XPath(pattern);

            var elements = _retryMechanism.InvokeFunc<T, ReadOnlyCollection<IWebElement>>(ref relativeRootElement, "FindElements", 3, xpath);

            return ConvertMany(elements);
        }

        private IGEWebElement ConvertOne(IWebElement element)
        {
            return new GEWebElement(element, _retryMechanism);
        }
        private ReadOnlyCollection<IGEWebElement> ConvertMany(ReadOnlyCollection<IWebElement> elements)
        {
            var geElements = new List<IGEWebElement>();
            foreach (var element in elements)
            {
                var geElement = ConvertOne(element);
                geElements.Add(geElement);
            }

            return new ReadOnlyCollection<IGEWebElement>(geElements);
        }

        public bool IsElementExist(string tag, string attribute, string value)
        {

            var elements = FindElements(tag, attribute, value);
            return (elements.Count > 0) ? true : false;
        }

        public bool IsElementExist(string attribute, string value)
        {
            var allTags = "*";
            return IsElementExist(allTags, attribute, value);
        }

        public bool IsElementExistByContainText(string containText)
        {
            var elements = FindElementsByContainsText(containText);
            return (elements.Count > 0) ? true : false;
        }
    }
}