using System.Collections.Generic;

namespace DiscountAlert.Core
{
    public class WatcherWebDriver
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public IList<byte> Snapshot { get; set; }
    }
}