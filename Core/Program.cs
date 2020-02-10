using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using DiscountAlert.ExceptionHandler;
using DiscountAlert.RetryMechanism;
using DiscountAlert.WebDriver;

namespace DiscountAlert.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            GEWebDriver driver = new GEWebDriver(new GERetryMechanism(new GEExceptionHandler()));
            Watcher watcher = new Watcher(driver);
            // const string url = "https://www.skyscanner.co.il/transport/flights/tlv/bud/200215/200222/?adults=1&children=0&adultsv2=1&childrenv2=&infants=0&cabinclass=economy&rtn=1&preferdirects=false&outboundaltsenabled=false&inboundaltsenabled=false&ref=home";
            // const string classElement = "EcoTicketWrapper_itineraryContainer__1VGlu";
            // const string title = "flight to budapest";

            // const string url = "https://ksp.co.il/?select=.272..573..2190..11451.&kg=&list=1&sort=2&glist=0&uin=0&txt_search=&buy=&minprice=0&maxprice=0&intersect=&rintersect=&store_real=";
            // const string classElement = "lineshowbox";
            // const string title = "ksp one plus";

            
            const string url = "https://www.ivory.co.il/catalog.php?act=cat&id=2735&f=4563&fromPrice=2422&toPrice=2661";
            const string classElement = "category-list";
            const string title = "ivory one plus";

            
            var results = watcher.Watch(title, url, classElement);
            using(Image image = Image.FromStream(new MemoryStream(results.First().Snapshot.ToArray())))
            {
                image.Save(("C\\Snapshot\\" + title) , ImageFormat.Png);  // Or Png
            }

        }
    }
}
