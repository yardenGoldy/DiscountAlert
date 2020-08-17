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

            
            const string url = "https://www.ivory.co.il/catalog.php?act=cat&id=2735&f=4563&fromPrice=3303";
            const string classElement = "category-list";
            const string title = "ivory one plus";

            //const string url = "https://www.trivago.co.il/?aDateRange%5Barr%5D=2020-04-08&aDateRange%5Bdep%5D=2020-04-13&aPriceRange%5Bfrom%5D=0&aPriceRange%5Bto%5D=11301&iRoomType=7&aRooms%5B0%5D%5Badults%5D=2&cpt2=25084%2F200&iViewType=0&bIsSeoPage=0&sortingId=1&slideoutsPageItemId=&iGeoDistanceLimit=20000&address=&addressGeoCode=&offset=0&ra=";
            //const string classElement = "item__wrapper";
            //const string title = "trivago roma";

            // const string url = "https://www.asics.com/us/en-us/search/?q=ASICS%20ff%20novak&prefn1=size&prefv1=11";
            // const string classElement = "product-tile";
            // const string title = "asics";

            // const string url = "https://www.booking.com/searchresults.he.html?aid=376388&label=booking-name-he-ZjzRvp_RD_yeZ9lEt5OinQS267777970027%3Apl%3Ata%3Ap1%3Ap22%2C563%2C000%3Aac%3Aap%3Aneg%3Afi%3Atikwd-65526620%3Alp1007981%3Ali%3Adec%3Adm%3Appccp%3DUmFuZG9tSVYkc2RlIyh9YcX_GyndjDE1z6LWmEwkC5A&lang=he&sid=5cfbb662542671edeacd0a330a831271&sb=1&src=searchresults&src_elem=sb&error_url=https%3A%2F%2Fwww.booking.com%2Fsearchresults.he.html%3Faid%3D376388%3Blabel%3Dbooking-name-he-ZjzRvp_RD_yeZ9lEt5OinQS267777970027%253Apl%253Ata%253Ap1%253Ap22%252C563%252C000%253Aac%253Aap%253Aneg%253Afi%253Atikwd-65526620%253Alp1007981%253Ali%253Adec%253Adm%253Appccp%253DUmFuZG9tSVYkc2RlIyh9YcX_GyndjDE1z6LWmEwkC5A%3Bsid%3D5cfbb662542671edeacd0a330a831271%3Btmpl%3Dsearchresults%3Bac_click_type%3Db%3Bac_position%3D0%3Bcheckin_month%3D8%3Bcheckin_monthday%3D19%3Bcheckin_year%3D2020%3Bcheckout_month%3D8%3Bcheckout_monthday%3D23%3Bcheckout_year%3D2020%3Bcity%3D-779626%3Bclass_interval%3D1%3Bdest_id%3D275801%3Bdest_type%3Dhotel%3Bdtdisc%3D0%3Bfrom_sf%3D1%3Bgroup_adults%3D2%3Bgroup_children%3D0%3Bhighlighted_hotels%3D275801%3Binac%3D0%3Bindex_postcard%3D0%3Blabel_click%3Dundef%3Bno_rooms%3D1%3Boffset%3D0%3Bpostcard%3D0%3Braw_dest_type%3Dhotel%3Broom1%3DA%252CA%3Bsb_price_type%3Dtotal%3Bsearch_selected%3D1%3Bshw_aparth%3D1%3Bslp_r_match%3D0%3Bsrc%3Dsearchresults%3Bsrc_elem%3Dsb%3Bsrpvid%3D4cce4e10697200e3%3Bss%3D%25D7%25A7%25D7%25A8%25D7%2590%25D7%2595%25D7%259F%2520%25D7%25A4%25D7%259C%25D7%2596%25D7%2594%2520%25D7%2590%25D7%2599%25D7%259C%25D7%25AA%252C%2520%25D7%2590%25D7%2599%25D7%259C%25D7%25AA%252C%2520%25D7%2593%25D7%25A8%25D7%2595%25D7%259D%2520%25D7%2594%25D7%2590%25D7%25A8%25D7%25A5%252C%2520%25D7%2599%25D7%25A9%25D7%25A8%25D7%2590%25D7%259C%3Bss_all%3D0%3Bss_raw%3D%25D7%25A7%25D7%25A8%25D7%2590%25D7%2595%25D7%259F%2520%25D7%25A4%3Bssb%3Dempty%3Bsshis%3D0%3Bssne%3D%25D7%2590%25D7%2599%25D7%259C%25D7%25AA%3Bssne_untouched%3D%25D7%2590%25D7%2599%25D7%259C%25D7%25AA%3Btop_ufis%3D1%26%3B&ss=%D7%A7%D7%9C%D7%90%D7%91+%D7%90%D7%99%D7%9F+%D7%90%D7%99%D7%9C%D7%AA+-+Coral+Beach+Villa+Resort%2C+%D7%90%D7%99%D7%9C%D7%AA%2C+%D7%93%D7%A8%D7%95%D7%9D+%D7%94%D7%90%D7%A8%D7%A5%2C+%D7%99%D7%A9%D7%A8%D7%90%D7%9C&is_ski_area=&ssne=%D7%90%D7%99%D7%9C%D7%AA&ssne_untouched=%D7%90%D7%99%D7%9C%D7%AA&city=-779626&checkin_year=2020&checkin_month=8&checkin_monthday=19&checkout_year=2020&checkout_month=8&checkout_monthday=23&group_adults=2&group_children=0&no_rooms=1&from_sf=1&ss_raw=%D7%A7%D7%9C%D7%90%D7%91+%D7%94%D7%95%D7%98%D7%9C&ac_position=3&ac_langcode=he&ac_click_type=b&dest_id=281822&dest_type=hotel&place_id_lat=29.5162177548089&place_id_lon=34.9219080805779&search_pageview_id=4cce4e10697200e3&search_selected=true&search_pageview_id=4cce4e10697200e3&ac_suggestion_list_length=4&ac_suggestion_theme_list_length=0";
            // const string classElement = "sr_item  sr_item_new sr_item_default sr_property_block   sr_flex_layout";
            // const string title = "booking";

            // const string url = "https://www.gomobile.co.il/25135-%D7%9E%D7%9B%D7%A9%D7%99%D7%A8%D7%99%D7%9D-%D7%A0%D7%99%D7%99%D7%93%D7%99%D7%9D/2200,2800,top?q=one%20plus";
            // const string classElement = "layout_list_item css_class_25135  col-xs-12 col-sm-6 col-md-4 col-lg-3";
            // const string title = "go mobile";

            // const string url = "https://www.aliexpress.com/item/10000000795100.html?spm=a2g0o.productlist.0.0.a45f13aaFRhUZt&algo_pvid=2b57e825-a6b6-4744-a093-a0c752b32f09&algo_expid=2b57e825-a6b6-4744-a093-a0c752b32f09-2&btsid=0ab6f81615976629977315422e6687&ws_ab_test=searchweb0_0,searchweb201602_,searchweb201603_";
            // const string classElement = "product-info";
            // const string title = "aliexpress";   

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
