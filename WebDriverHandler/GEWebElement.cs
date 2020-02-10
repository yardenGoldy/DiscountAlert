using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiscountAlert.Shared;
using OpenQA.Selenium;

namespace DiscountAlert.WebDriver
{
    internal class GEWebElement : IGEWebElement
    {
        private IWebElement _seleniumElement;
        private IGERetryMechanism _retryMechanism;

        public GEWebElement(IWebElement element, IGERetryMechanism retryMechanism)
        {
            _seleniumElement = element;
            _retryMechanism = retryMechanism;
        }

        // '/*' root pattern
        public IGEWebElement Root => FindElementByXPath("/*");

        // '..' parent pattern
        public IGEWebElement Parent => FindElementByXPath("..");

        public string Text => throw new System.NotImplementedException();

        public IGEWebElement FindElement(string attribute, string value)
        {
            var attributeValuePattern = $"//[@{attribute}='{value}']";
            return FindElementByXPath(attributeValuePattern);
        }

        public IGEWebElement FindElement(string tag, string attribute, string value)
        {
            // XPath pattern
            var tagAttributeValuePattern = $"//{tag}[@{attribute}='{value}']";
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
            var allTagElementPattern = $"//{tag}]";
            return FindElementByXPath(allTagElementPattern);
        }

        public IGEWebElement FindElementByText(string text)
        {
            var findByTextPattern = $"//*[text()='{text}']";
            return FindElementByXPath(findByTextPattern);
        }
        public IGEWebElement FindElementByContainsText(string text)
        {
            var containTextPattern = $"//*[contains(text()='{text}')]";
            return FindElementByXPath(containTextPattern);
        }


        public ReadOnlyCollection<IGEWebElement> FindElements(string tag, string attribute, string value)
        {
            var tagAttributeValuePattern = $"//{tag}[@{attribute}='{value}']";
            return FindElementsByXPath(tagAttributeValuePattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElements(string attribute, string value)
        {
            var attributeValuePattern = $"//[@{attribute}='{value}']";
            return FindElementsByXPath(attributeValuePattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByClassName(string className)
        {
            return FindElements("class", className);
        }
        public ReadOnlyCollection<IGEWebElement> FindElementsByTag(string tag)
        {
            var allTagElementPattern = $"//{tag}]";
            return FindElementsByXPath(allTagElementPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByContainsText(string containText)
        {
            var containTextPattern = $"//*[contains(text()='{containText}')]";
            return FindElementsByXPath(containTextPattern);
        }

        public ReadOnlyCollection<IGEWebElement> FindElementsByText(string text)
        {
            var findByTextPattern = $"//*[text()='{text}']";
            return FindElementsByXPath(findByTextPattern);
        }

        public string GetAttribute(string attributeName)
        {
            Func<string, string> getAttributeFunc = _seleniumElement.GetAttribute;
            var value = _retryMechanism.InvokeFunc<string>(getAttributeFunc, 3, attributeName);
            return value;
        }

        public void Click()
        {
            // Action clickAction = _seleniumElement.Click;
            // _retry.Retry(clickAction, 3);
            _seleniumElement.Click();
        }

        public void SetValueLikeHuman(string value)
        {
            _seleniumElement.SendKeys(value);
        }

        internal IGEWebElement FindElementByXPath(string pattern)
        {
            // XPath pattern
            var xpath = By.XPath(pattern);

            Func<By, IWebElement> findElementsFunc = _seleniumElement.FindElement;
            var element = _retryMechanism.InvokeFunc<IWebElement>(findElementsFunc, 3, xpath);

            return new GEWebElement(element, _retryMechanism);
        }
        internal ReadOnlyCollection<IGEWebElement> FindElementsByXPath(string pattern)
        {
            var xpath = By.XPath(pattern);

            Func<By, ReadOnlyCollection<IWebElement>> findElementsFunc = _seleniumElement.FindElements;
            var elements = _retryMechanism.InvokeFunc<ReadOnlyCollection<IWebElement>>(findElementsFunc, 3, xpath);

            var geElements = new List<IGEWebElement>();
            foreach (var element in elements)
                geElements.Add(new GEWebElement(element, _retryMechanism));

            return new ReadOnlyCollection<IGEWebElement>(geElements);
        }
    }
}