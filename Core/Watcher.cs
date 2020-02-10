using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DiscountAlert.Shared;

namespace DiscountAlert.Core
{
    public class Watcher
    {
        private readonly List<string> identifiers = new List<string>(){
            "price",
            "$",
            "₪",
            "מחיר"
        };
        private IGEWebDriver _webDriver;
        public Watcher(IGEWebDriver webDriver){
            _webDriver = webDriver;
        }

        public List<WatcherWebDriver> Watch(string title, string url, string classNameOfResults){
            this._webDriver.Navigate(url);
            return this.FindPriceElements(classNameOfResults);
        }

        private List<WatcherWebDriver> FindPriceElements(string classNameOfResults){
            List<WatcherWebDriver> watcherWebDrivers = new List<WatcherWebDriver>();
            Thread.Sleep(10000);
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
                var priceElementCandidates = element.FindElementsByContainsText(priceIdentify);
                double price;
                if(this.TryGetMaxPriceOfAnElement(priceElementCandidates, out price)){
                    return price;
                }
            }

            throw new Exception("cannot find price in the element");
        }

        private bool TryGetMaxPriceOfAnElement(IList<IGEWebElement> elements, out double price)
        {
            List<double> prices = new List<double>();
            for(int j = 0; j < elements.Count; j++) {
                    string priceText = elements[j].Text;
                    string regex = @"\x" + String.Join("|", identifiers) + @"\g";
                    double result;
                    if(double.TryParse(priceText.Replace(regex, ""), out result))
                        prices.Add(result);
            }
            bool hasPrice = false;
            if(prices.Count != 0){
                hasPrice = true;
                price = prices.Max();
            }
            else
            {
                price = 0;
            }
            return hasPrice;
        }
    }
}