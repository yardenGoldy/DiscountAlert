using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using DiscountAlert.Shared;
using OpenQA.Selenium;

namespace DiscountAlert.Core
{
    public class Watcher
    {
        private readonly List<string> identifiers = new List<string>(){
            "price",
            "$",
            "₪",
            "מחיר",
            "שח",
            @"ש""ח",
            "usd",
            "שקל",
            "שקלים",
        };
        //*[matches(text(),'(^|\W)match($|\W)','i')]
        private IGEWebDriver _webDriver;
        public Watcher(IGEWebDriver webDriver){
            _webDriver = webDriver;
        }

        public List<WatcherWebDriver> Watch(string title, string url, string classNameOfResults){
            this._webDriver.Navigate(url);
            return this.FindElementForWatch(classNameOfResults);
        }

        private List<WatcherWebDriver> FindElementForWatch(string classNameOfResults){
            List<WatcherWebDriver> watcherWebDrivers = new List<WatcherWebDriver>();
            Thread.Sleep(6000);
            
            var elementsToWatch = _webDriver.FindElementsByClassName(classNameOfResults);
            for(int i = 0; i < elementsToWatch.Count; i++){
                var elem = elementsToWatch[i];
                watcherWebDrivers.Add(new WatcherWebDriver(){
                    Price = FindPrice(elem),
                    Snapshot = this._webDriver.TakeScreenShot(elem)
                });
            }

            watcherWebDrivers.OrderBy(x => x.Price);
            watcherWebDrivers.Take(3);
            return watcherWebDrivers;

        }

        private double FindPrice(IGEWebElement element){
            for(int i = 0; i < identifiers.Count; i++) {
                string priceIdentify = identifiers[i];
                var priceElementCandidates = element.FindElementsByContainsText(identifiers[i]);
                var filteredPriceElement = priceElementCandidates.Where(x => x.Text != "" && x.Text.Split(identifiers[2]).Length ==2).ToList();
                var price = this.TryGetMaxPriceOfAnElement(filteredPriceElement);
                if(price.HasValue){
                    return price.Value;
                }
            }

            throw new Exception("can not find any price");
        }

        private double? TryGetMaxPriceOfAnElement(IList<IGEWebElement> elements)
        {
            double? resultPrice = null;
            List<double> prices = new List<double>();
            for(int j = 0; j < elements.Count; j++) {
                    string priceText = elements[j].Text;
                    double result;
                    var elementPrice = Regex.Replace(priceText, "[^0-9.]", "");
                    if(double.TryParse(elementPrice, out result))
                        prices.Add(result);
            }
            if(prices.Count != 0){
                resultPrice = prices.Max();
            }

            return resultPrice;
        }
    }
}