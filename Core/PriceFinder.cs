
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DiscountAlert.Shared;
using MoreLinq;

namespace DiscountAlert.Core
{
    public class PriceFinder: IPriceFinder
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
        
        public IList<IPricedElement> ExtractPrices(IList<IGEWebElement> elements){
            List<IPricedElement> result = new List<IPricedElement>(); 
            for(int i = 0; i < elements.Count; i++) {
                var element = elements[i];
                var price = this.ExtractPrice(element);
                result.Add(new PricedElement(){
                        Element = element, 
                        Price = price
                });
            }

            return result;
        }  
        
        public double ExtractPrice(IGEWebElement element){
            for(int i = 0; i < identifiers.Count; i++) {
                string priceIdentify = identifiers[i];
                var priceElementCandidates = element.FindElementsByContainsText(priceIdentify);
                var filteredPriceElement = priceElementCandidates.Where(x => x.Text != "").DistinctBy(x => x.Text).ToList();
                var price = this.TrySeparatePricesFromTexts(filteredPriceElement, priceIdentify);
                if(price.HasValue){
                    return price.Value;
                }
            }

            throw new Exception("can not find any price");
        }

        private double? TrySeparatePricesFromTexts(IList<IGEWebElement> elements, string priceIdentify)
        {
            double? resultPrice = null;
            List<double> prices = new List<double>();
            for(int j = 0; j < elements.Count; j++) {
                string priceText = elements[j].Text;
                if(IgnoreText(priceText)) continue;
                priceText = this.getPriceFromText(priceText, priceIdentify);
                double result;
                    
                if(this.TryParseTextAsPrice(out result, priceText))
                {
                   prices.Add(result);
                }
                else {
                    FindPriceOnParent(elements[j], priceIdentify, prices);
                }   
            }
            
            if(prices.Count != 0){
                resultPrice = choosePrice(prices);
            }

            return resultPrice;
        }


        private bool IgnoreText(string text){
            var result = Regex.IsMatch(text, "per|coupon|shipping|משלוח", RegexOptions.IgnoreCase);
            return result;
        }

         private string getPriceFromText(string priceText, string priceIdentify){
            if(priceIdentify.Count() == 1) priceIdentify = "\\" + priceIdentify;
                var regex = new Regex($"({priceIdentify}\\s*[0-9,]+(\\.[0-9]{{2}})?)|([0-9,]+(\\.[0-9]{{2}})?\\s*{priceIdentify})");
                var check = regex.Match(priceText);
                return check.Value;
        }

        private bool TryParseTextAsPrice(out double result, string priceText){
            result = 0;
            var elementPrice = Regex.Replace(priceText, "[^0-9.-]", "");
            return double.TryParse(elementPrice, out result);
        }

        public void FindPriceOnParent(IGEWebElement element, string priceIdentify, IList<double> prices)
        {
            double result;
            string parentPriceText = element.Parent.Text;
            parentPriceText = this.getPriceFromText(parentPriceText, priceIdentify);
            if(this.TryParseTextAsPrice(out result , parentPriceText))
            {
                prices.Add(result);
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