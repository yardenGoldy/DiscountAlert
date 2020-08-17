using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using DiscountAlert.Shared;
using OpenQA.Selenium;
using SixLabors.ImageSharp;

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
                var priceElementCandidates = element.FindElementsByContainsText(priceIdentify);
                if(priceElementCandidates.Count == 0)
                    priceElementCandidates = element.FindElementsByContainsTextIgnoreSpaces(priceIdentify);
                var filteredPriceElement = priceElementCandidates.Where(x => x.Text != "").ToList();
                var price = this.TryGetMinPriceOfAnElement(filteredPriceElement);
                if(price.HasValue){
                    return price.Value;
                }
            }

            throw new Exception("can not find any price");
        }

        private double? TryGetMinPriceOfAnElement(IList<IGEWebElement> elements)
        {
            double? resultPrice = null;
            List<double> prices = new List<double>();
            for(int j = 0; j < elements.Count; j++) {
                string priceText = elements[j].Text;
                double result;
                    
                if(this.TryGetPrice(out result, priceText))
                {
                   prices.Add(result);
                }
                else {
                    string parentPriceText = elements[j].Parent.Text;
                    if(this.TryGetPrice(out result , parentPriceText))
                    {
                        prices.Add(result);
                    }
                }   
            }
            
            if(prices.Count != 0){
                resultPrice = choosePrice(prices);
            }

            return resultPrice;
        }

        private bool TryGetPrice(out double result, string priceText){
            result = 0;
            var elementPrice = Regex.Replace(priceText, "[^0-9.-]", "");
            var rangePrices = elementPrice.Split("-");
            if(rangePrices.Count() ==2)
            {
                double firstNumber, secondNumber = default(double);
                bool isPrices = double.TryParse(rangePrices[0], out firstNumber) && 
                                double.TryParse(rangePrices[1], out secondNumber);
                if(isPrices)    result = Math.Max(firstNumber, secondNumber);
                return isPrices;
            }
            return double.TryParse(elementPrice, out result);
        }
        private void ShowElement(IGEWebElement elem){
            var screenShot = this._webDriver.TakeScreenShot(elem);
            using(Image image = Image.Load(new MemoryStream(screenShot.ToArray())))
            {
                image.Save((".\\Snapshot\\" + "title" + ".png"));
            }
        }

        private double choosePrice(IList<double> prices){
            double median = this.getMedian(prices);
            prices = prices.Where(price => median * 0.2 < price).ToList();
            return prices.Min();
        }

        private double getMedian(IList<double> numbers){
            int numberCount = numbers.Count();
            int halfIndex = numbers.Count()/2;
            var sortedNumbers = numbers.OrderBy(n=>n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) +
                sortedNumbers.ElementAt((halfIndex - 1)))/ 2);
            } else {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }
    }
}