using Java.Lang;
using Newtonsoft.Json;
using SaveOn.GoogleModels;
using SaveOn.Models;
using SaveOn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SaveOn.Google
{
    class GoogleService
    {
        static GoogleService defaultInstance = new GoogleService();
        string nearbySearch;
        string detailSearch;
        string imageSearch;
        string defaultRange = "&radius=1000";
        private string googleKey = "&key=AIzaSyAQbsw46ULkgMsMOUYaDveKGkya0qYOlaU";

        private GoogleService()
        {
            this.nearbySearch = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=";
            this.detailSearch = "https://maps.googleapis.com/maps/api/place/details/json?placeid=";
            this.imageSearch = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=786&photoreference=";
        }

            
        
    
        
        public static GoogleService DefaultGoogleInstance
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        public async Task<CouponSchedule> GooglePlaceDetails(CouponSchedule _coupon)
        {
            var searchString = detailSearch + _coupon.thisCoupon.GoogleData.place_id + googleKey;
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(string.Format(searchString));
                var result = JsonConvert.DeserializeObject<GooglePlaces>(response);
                if (result.result != null)
                {
                    var use = result.result;
                    _coupon.thisCoupon.GoogleData = use;
                    //            Console.WriteLine("use=>" + use.place_id);
                }
            }
            return _coupon;
        }
        
        public async Task<List<CouponSchedule>> GoogleNearby(Location myLoc)
        {
            List<CouponSchedule> cpns = new List<CouponSchedule>();
            string nextPage = "";
            int i = 0;
            int o = 0;
            for(int b = 0; b < 2; b++)
            {
                using (var client = new HttpClient())
                {

                    var searchString = nearbySearch + myLoc.lat + "," + myLoc.lng + defaultRange + googleKey + "&pagetoken=" + nextPage;
                    Console.WriteLine(searchString);
                    var response = await client.GetStringAsync(string.Format(searchString));
                    var result = JsonConvert.DeserializeObject<GooglePlaces>(response);
                    nextPage = result.next_page_token;
                    foreach (var info in result.results)
                    {
                        CouponSchedule cpn = new CouponSchedule();
                        cpn.thisCoupon = new Coupon();

                        cpn.thisCoupon.GoogleData = info;
                        cpns.Add(cpn);
                    }
                    int milliseconds = 250;
                    Thread.Sleep(milliseconds);
                }
            }    
            foreach (var c in cpns)
            {
                Console.WriteLine(o + "checking this ==" + c.thisCoupon.GoogleData.name);
                o++;
            }
            return cpns;
        }
            
        
        public List<CouponSchedule> GoogleImages(List<CouponSchedule> coupons)
        {
            HttpClientHandler handler = new HttpClientHandler();
            CookieContainer cookies = new CookieContainer();
            handler.CookieContainer = cookies;
            
            int toSkip;
            foreach (var c in coupons)
            {
                var getPhotoObj = c.thisCoupon.GoogleData.photos;
                if (getPhotoObj != null)
                {
                    Random rand = new Random();
                    toSkip = rand.Next(0, getPhotoObj.Count);

                    

                    var photoRef = getPhotoObj.Skip(toSkip).Take(1).First().photo_reference;
                    string image = imageSearch + photoRef + googleKey;

                    using (var client = new HttpClient(handler, false))
                    {
                        UriImageSource uriImage = new UriImageSource() { Uri = new Uri(image), CachingEnabled = false };
                        Console.WriteLine(c.thisCoupon.GoogleData.name + "------ > is our name");
                        Console.WriteLine(c.thisCoupon.id + "------ > is our id");

                        if (uriImage != null)
                            c.thisCoupon.ImageStream = uriImage;

                    }

                }
                else
                {
                    Console.WriteLine(c.thisCoupon.GoogleData.name + " has no image source!! ERR");
                }
            }
               
            return coupons;
        }

    }
    
}
