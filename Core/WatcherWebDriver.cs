using System;
using System.Collections.Generic;
using System.Linq;
using DiscountAlert.Shared;

namespace DiscountAlert.Core
{
    public class WatcherWebDriver
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public IList<byte> Snapshot { get; set; }
    }
}