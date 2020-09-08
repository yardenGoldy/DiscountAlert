using System.Collections.Generic;
using DiscountAlert.Shared;

namespace DiscountAlert.Core
{
    public interface IPriceFinder
    {
        IList<IPricedElement> ExtractPrices(IList<IGEWebElement> elements);
        double ExtractPrice(IGEWebElement element);
    }
}
