using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using DiscountAlert.Shared;
using MoreLinq;
using SixLabors.ImageSharp;

namespace DiscountAlert.Core
{
    public class Watcher
    {
        private readonly List<string> identifiers = new List<string>(){
            "$",
            "₪",
            "price",
            "מחיר",
            "שח",
            "usd",
            "dollars",
            "us",
            "ils",
            "שקל",
            "שקלים",
        };
        public Dictionary<string, string> DomainsPopUp { get; set; }
        //*[matches(text(),'(^|\W)match($|\W)','i')]
        private IGEWebDriver _webDriver;
        public Watcher(IGEWebDriver webDriver){
            _webDriver = webDriver;
            InitDomainDictionary();
        }

        public List<WatcherWebDriver> Watch(string title, string url, string classNameOfResults){
            this._webDriver.Navigate(url);
            ClosePopUpIfNecessary(url);
            return this.ExtractDetailsFromElements(classNameOfResults);
        }

        private void ClosePopUpIfNecessary(string url)
        {
            var domain = getDomainFromUrl(url);
            if(DomainsPopUp.ContainsKey(domain)){
                var selector = DomainsPopUp[domain];
                var elements = this._webDriver.FindElementsByClassName(selector);
                for(int i = 0; i < elements.Count; i++){
                    var elem = elements[i];
                    try{
                        elem.Click();
                    }
                    catch(Exception){

                    }
                }
            }
        }

        private string getDomainFromUrl(string url){
            Uri uri = new Uri(url);
            return uri.Host.Split(".")[1];
        }

        private List<WatcherWebDriver> ExtractDetailsFromElements(string classNameOfResults){
            List<WatcherWebDriver> watcherWebDrivers = new List<WatcherWebDriver>();
            Thread.Sleep(6000);
            
            var elementsToWatch = _webDriver.FindElementsByClassName(classNameOfResults);
            int numberOfElements = elementsToWatch.Count();
            if(numberOfElements == 0)
                throw new Exception("cannot find any elemnet to watch for");

            else if(numberOfElements == 1) {
                var elem = elementsToWatch.First();
                var price = ExtractPriceFromElement(elem);
                watcherWebDrivers.Add(new WatcherWebDriver(){
                    Price = price,
                    Snapshot = this._webDriver.TakeScreenShot(elem),
                    Id= this._webDriver.State.Url
                });
            }
            else
            {
                for(int i = 0; i < elementsToWatch.Count; i++){
                    var elem = elementsToWatch[i];
                    var price = ExtractPriceFromElement(elem);
                    watcherWebDrivers.Add(new WatcherWebDriver(){
                        Price = price,
                        Snapshot = this._webDriver.TakeScreenShot(elem),
                        Id= this.GetIdFromElement(elem)
                    });
                }
            }
            
            return watcherWebDrivers;
        }

        private string GetIdFromElement(IGEWebElement element){
            var elementId = element.findUrls();
            if(elementId != null){
                return elementId;
            }
            throw new Exception("multiply ids");
        }

        private double ExtractPriceFromElement(IGEWebElement element){
            for(int i = 0; i < identifiers.Count; i++) {
                string priceIdentify = identifiers[i];
                var priceElementCandidates = element.FindElementsByContainsText(priceIdentify);
                var filteredPriceElement = priceElementCandidates.Where(x => x.Text != "").DistinctBy(x => x.Text).ToList();
                var price = this.TryGetMinPriceOfAnElement(filteredPriceElement, priceIdentify);
                if(price.HasValue){
                    return price.Value;
                }
            }

            throw new Exception("can not find any price");
        }
        private bool IgnoreText(string text){
            var result = Regex.IsMatch(text, "per|coupon", RegexOptions.IgnoreCase);
            return result;
        }
        private double? TryGetMinPriceOfAnElement(IList<IGEWebElement> elements, string priceIdentify)
        {
            double? resultPrice = null;
            List<double> prices = new List<double>();
            for(int j = 0; j < elements.Count; j++) {
                string priceText = elements[j].Text;
                if(IgnoreText(priceText)) continue;
                priceText = this.getPriceFromText(priceText, priceIdentify);
                double result;
                    
                if(this.TryGetPrice(out result, priceText))
                {
                   prices.Add(result);
                }
                else {
                    string parentPriceText = elements[j].Parent.Text;
                    parentPriceText = this.getPriceFromText(parentPriceText, priceIdentify);
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
        private string getPriceFromText(string priceText, string priceIdentify){
            if(priceIdentify.Count() == 1) priceIdentify = "\\" + priceIdentify;
                var regex = new Regex($"({priceIdentify}\\s*[0-9,]+(\\.[0-9]{{2}})?)|([0-9,]+(\\.[0-9]{{2}})?\\s*{priceIdentify})");
                var check = regex.Match(priceText);
                return check.Value;
        }

        private bool TryGetPrice(out double result, string priceText){
            result = 0;
            var elementPrice = Regex.Replace(priceText, "[^0-9.-]", "");
            // var rangePrices = elementPrice.Split("-");
            // if(rangePrices.Count() ==2)
            // {
            //     double firstNumber, secondNumber = default(double);
            //     bool isPrices = double.TryParse(rangePrices[0], out firstNumber) && 
            //                     double.TryParse(rangePrices[1], out secondNumber);
            //     if(isPrices)    result = Math.Max(firstNumber, secondNumber);
            //     return isPrices;
            // }
            return double.TryParse(elementPrice, out result);
        }
        private void ShowElement(IGEWebElement elem){
            var screenShot = this._webDriver.TakeScreenShot(elem);
            using(Image image = Image.Load(new MemoryStream(screenShot.ToArray())))
            {
                image.Save((".\\Snapshot\\" + "title" + ".png"));
            }
        }

        private void InitDomainDictionary(){
            DomainsPopUp = new Dictionary<string, string>();
            DomainsPopUp.Add("aliexpress", "next-dialog-close");
            DomainsPopUp.Add("momondo", "Button-No-Standard-Style close darkIcon large ");
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