using System.IO;
using System.Linq;
using DiscountAlert.ExceptionHandler;
using DiscountAlert.RetryMechanism;
using DiscountAlert.WebDriver;
using SixLabors.ImageSharp;

namespace DiscountAlert.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            GEWebDriver driver = new GEWebDriver(new GERetryMechanism(new GEExceptionHandler()));
            Watcher watcher = new Watcher(driver);
            // const string url = "https://www.skyscanner.co.il/transport/flights/tlv/bud/200820/200827/?adults=1&adultsv2=1&cabinclass=economy&children=0&childrenv2=&inboundaltsenabled=false&infants=0&outboundaltsenabled=false&preferdirects=false&preferflexible=false&ref=home&rtn=1";
            // const string classElement = "EcoTicketWrapper_itineraryContainer__1VGlu";
            // const string title = "flight to budapest";

            // const string url = "https://ksp.co.il/?select=.272..573..2190..11451.&kg=&list=1&sort=2&glist=0&uin=0&txt_search=&buy=&minprice=0&maxprice=0&intersect=&rintersect=&store_real=";
            // const string classElement = "lineshowbox";
            // const string title = "ksp one plus";

            
            // const string url = "https://www.ivory.co.il/catalog.php?act=cat&id=2735&f=4563&fromPrice=3303";
            // const string classElement = "category-list";
            // const string title = "ivory one plus";

            //const string url = "https://www.trivago.co.il/?aDateRange%5Barr%5D=2020-04-08&aDateRange%5Bdep%5D=2020-04-13&aPriceRange%5Bfrom%5D=0&aPriceRange%5Bto%5D=11301&iRoomType=7&aRooms%5B0%5D%5Badults%5D=2&cpt2=25084%2F200&iViewType=0&bIsSeoPage=0&sortingId=1&slideoutsPageItemId=&iGeoDistanceLimit=20000&address=&addressGeoCode=&offset=0&ra=";
            //const string classElement = "item__wrapper";
            //const string title = "trivago roma";

            const string url = "https://www.asics.com/us/en-us/search/?q=ASICS%20ff%20novak&prefn1=size&prefv1=11";
            const string classElement = "product-tile";
            const string title = "asics";

            // const string url = "https://www.momondo.com/flight-search/TLV-BUD/2020-09-15/2020-09-22?sort=bestflight_a";
            // const string classElement = "Base-Results-HorizonResult";
            // const string title = "momndo";
            
            var results = watcher.Watch(title, url, classElement);
            using(Image image = Image.Load(new MemoryStream(results.First().Snapshot.ToArray())))
            {
                image.Save((".\\Snapshot\\" + title + ".png"));
            }

        }
    }
}
