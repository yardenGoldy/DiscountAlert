using System.Collections.Generic;
using DiscountAlert.Shared;

namespace DiscountAlert.Core
{
    public class PricedElement : IPricedElement
    {
        public IGEWebElement Element { get; set; }
        public double Price { get; set; }
    }

    public interface IPricedElement
    {
        IGEWebElement Element { get; set; }
        double Price { get; set; }
    }
}