using System;
using DiscountAlert.Shared;
using OpenQA.Selenium;

namespace DiscountAlert.WebDriver
{
    internal class GEWebElement : GEElementFinder<IWebElement>, IGEWebElement
    {
        private IWebElement _relativeRootElement;

        public object SourceElement { get { return _relativeRootElement;}}
        public GEWebElement(IWebElement element, IGERetryMechanism retryMechanism)
        : base(retryMechanism)
        {
            // relativeRootElement = element;
            _relativeRootElement = element;
        }

        // '..' parent pattern
        protected override ref IWebElement relativeRootElement => ref _relativeRootElement;
        public IGEWebElement Parent => FindElementByXPath("..");

        public string Text => relativeRootElement.Text;
        public string TagName => relativeRootElement.TagName;

        public void Click()
        {
            Action a = relativeRootElement.Click;
            var status = _retryMechanism.InvokeAction(a, 3, null);
            // relativeRootElement.Click();
        }

        public void SetValueLikeHuman(string value)
        {
            relativeRootElement.SendKeys(value);
        }

        public string GetAttribute(string attributeName)
        {
            var value = _retryMechanism.InvokeFunc<IWebElement, string>(ref relativeRootElement, nameof(relativeRootElement.GetAttribute), 3, attributeName);
            return value;
        }
    }
}