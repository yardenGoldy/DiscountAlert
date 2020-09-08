using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DiscountAlert.Shared;
using MoreLinq;
using SixLabors.ImageSharp;

namespace DiscountAlert.Core
{
    public class Watcher
    {
        private IPriceFinder _priceFinder;
        public Dictionary<string, string> DomainsPopUp { get; set; }
        //*[matches(text(),'(^|\W)match($|\W)','i')]
        private IGEWebDriver _webDriver;
        public Watcher(IGEWebDriver webDriver): this(webDriver, new PriceFinder()){
        }

        public Watcher(IGEWebDriver webDriver, IPriceFinder priceFinder){
            _priceFinder = priceFinder;
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
            Thread.Sleep(10000);
            
            var elementsToWatch = _webDriver.FindElementsByClassName(classNameOfResults);
            int numberOfElements = elementsToWatch.Count();
            if(numberOfElements == 0)
                throw new Exception("cannot find any elemnet to watch for");

            else if(numberOfElements == 1) {
                var elem = elementsToWatch.First();
                var price = this._priceFinder.ExtractPrice(elem);
                watcherWebDrivers.Add(new WatcherWebDriver(){
                    Price = price,
                    Snapshot = this._webDriver.TakeScreenShot(elem),
                    Id= this._webDriver.State.Url
                });
            }
            else
            {
                var pricedElements = this._priceFinder.ExtractPrices(elementsToWatch);
                for(int i = 0; i < pricedElements.Count; i++){
                    var pricedElement = pricedElements[i];
                    watcherWebDrivers.Add(new WatcherWebDriver(){
                        Price = pricedElement.Price,
                        Snapshot = this._webDriver.TakeScreenShot(pricedElement.Element),
                        Id= this.GetIdFromElement(pricedElement.Element)
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
    }
}