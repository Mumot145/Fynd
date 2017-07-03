using MR.Gestures;
using Newtonsoft.Json;
using Plugin.Geolocator;
using SaveOn.Azure;
using SaveOn.GoogleModels;
using SaveOn.Models;
using SaveOn.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using static SaveOn.CustomEventArgs;
using Plugin.Geolocator.Abstractions;
using SaveOn.Google;
using SaveOn.GPS;

namespace SaveOn.XAML
{
    public partial class ShoppingList
    {
        
        private List<CouponSchedule> masterCoupons = null;
        private CustomEventArgs args;
        private List<CouponSchedule> fixedCoupons = new List<CouponSchedule>();
        public List<Coupon> savedCoupons = new List<Coupon>();
        private FacebookUser facebookUser = new FacebookUser();
        private CouponList couponList = new CouponList();
        private User user = new User();
        private User.FoodPreferences foodPreferences = new User.FoodPreferences();
        AzureCouponManager _azureCoupons;
        GoogleService _google;
        List<ImageSource> couponsImages = new List<ImageSource>();
        AzureDataService azure = new AzureDataService();

        Map map = new Map();
        private string limeridge = "43.217703,-79.861937";
        private string rad = "800";
        string nextPage;
        string RestUrl;
        private string name;
        
        private Plugin.Geolocator.Abstractions.Position position;

        public ShoppingList(User _user)
        {
            //TappedCommand = new Command<TapEventArgs>(OnTapped);
            _azureCoupons = AzureCouponManager.DefaultManager;
            //couponList.currentUser = user;
            _google = GoogleService.DefaultGoogleInstance;
            this.BindingContext = new ShoppingListViewModel();
            user = _user;
            Debug.WriteLine("Shopping List Has ="+user.name);
            //OnDownloadImageButtonClicked();
            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            if(masterCoupons == null)
            {
                refreshInfo();
            }     
        }
        private async void refreshInfo()
        {
            absLayout.ItemsSource = null;
            masterCoupons = new List<CouponSchedule>();
            GetGPS _gps = new GetGPS();
            Location myLocation = await _gps.updateLocation();
            if (myLocation == null)
            {
                myLocation.lat = 43.217703;
                myLocation.lng = -79.861937;
            }

                masterCoupons = await _google.GoogleNearby(myLocation);
                //masterCoupons = filterCoupons(masterCoupons);
                couponList.couponList = masterCoupons;
                int i = 0;
                masterCoupons = _google.GoogleImages(masterCoupons);
                foreach (var n in masterCoupons)
                {

                    n.OnFindCoupon += new CouponSchedule.FindCouponHandler(FindThisPlace);
                    Console.WriteLine("TypeLink  ====>  " + n.thisCoupon.TypeLink);
                    if (n.thisCoupon.ImageStream != null)
                        fixedCoupons.Add(n);
                    
                    i++;
                }
            
                absLayout.ItemsSource = fixedCoupons;
                BindingContext = fixedCoupons;
            
        }
        
        private void absLayout_Refreshing(object sender, EventArgs e)
        {
            refreshInfo();
            absLayout.IsRefreshing = false;
        }
       

        private List<CouponSchedule> filterCoupons(List<CouponSchedule> couponList)
        {
            List<string> locationNames = new List<string>();
            List<CouponSchedule> fixedList = new List<CouponSchedule>();
            foreach (var _coupon in couponList)
            {
                if (!locationNames.Contains(_coupon.thisCoupon.ImageUrl))
                {
                    locationNames.Add(_coupon.thisCoupon.ImageUrl);
                    fixedList.Add(_coupon);
                }
            }
            return fixedList;
        }
        void ListView_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
        }


        
        public async Task<List<CouponSchedule>> RefreshDataAsync(Location myLoc)
        {
            List<CouponSchedule> couponInfo = new List<CouponSchedule>();
            List<CouponSchedule> cpns = new List<CouponSchedule>();
            List<String> couponUrls = new List<String>();
            List<GoogleModels.Location> coordinates = new List<GoogleModels.Location>();
            List<String> top5List = new List<string>();
            //cpns = await getGoogleInfo(myLoc);
            var getT = await _azureCoupons.GetTodoItemsAsync(cpns);
            
                couponInfo = azure.GetCoupons(getT.ToList());
                //couponInfo = await azure.GetCoupons(couponUrls);           
            return couponInfo;
        }
    //    private async Task<List<CouponSchedule>> getGoogleInfo(Location loc)
    //    {
    //        List<CouponSchedule> cpns = new List<CouponSchedule>();
    //        int j = 0;
    //       // string oneRestUrl = setLocation(loc);
    //        using (var client = new HttpClient())
    //        {
    //           // var response = await client.GetStringAsync(string.Format(oneRestUrl));
    //            var result = JsonConvert.DeserializeObject<GooglePlaces>(response);
    //            foreach (var info in result.results)
    //            {
    //                CouponSchedule cpn = new CouponSchedule();
    //                 cpn.thisCoupon = new Coupon();
    //                Console.WriteLine("pat look here =="+info.name);
    //                cpn.thisCoupon.GoogleData = info;                    
    //                cpns.Add(cpn);
    //                j++;
                    
    //            }
    //        }
    //    return cpns;
    //}
        
        async void FindThisPlace(object sender, FindEventArgs e)
        {
            Console.WriteLine("in Group Details - AddNewMEssages");
            
            //BindingContext = _currentChatGroup;
            //_currentChatGroup.ToDoList = schdmsg.provideDates();
            CouponSchedule lookingFor = e.Data;

            lookingFor = await _google.GooglePlaceDetails(lookingFor);

            FindPlacePage findPlace = new FindPlacePage(lookingFor, position);
            await Navigation.PushAsync(findPlace);
           // toDo.AddToList(addmsg.returnMessages());
            //toDo.RefreshItems(true);
        }
        public List<String> getTop5(User.FoodPreferences foodPreferences)
        {
            String[,] food = new String[18, 2];
            List<String> Top5Food = new List<string>();
            int highestRating;
            int lastRating;
            food[0, 0] = "cafe";
            food[0, 1] = String.Format("{0}", foodPreferences.Cafe);
            food[1, 0] = "canadian";
            food[1, 1] = String.Format("{0}", foodPreferences.Canadian);
            food[2, 0] = "chinese";
            food[2, 1] = String.Format("{0}", foodPreferences.Chinese);
            food[3, 0] = "dessert";
            food[3, 1] = String.Format("{0}", foodPreferences.Dessert);
            food[4, 0] = "fastfood";
            food[4, 1] = String.Format("{0}", foodPreferences.FastFood);
            food[5, 0] = "french";
            food[5, 1] = String.Format("{0}", foodPreferences.French);
            food[6, 0] = "glutenfree";
            food[6, 1] = String.Format("{0}", foodPreferences.GlutenFree);
            food[7, 0] = "greek";
            food[7, 1] = String.Format("{0}", foodPreferences.Greek);
            food[8, 0] = "indian";
            food[8, 1] = String.Format("{0}", foodPreferences.Indian);
            food[9, 0] = "italian";
            food[9, 1] = String.Format("{0}", foodPreferences.Italian);
            food[10, 0] = "japanese";
            food[10, 1] = String.Format("{0}", foodPreferences.Japanese);
            food[11, 0] = "mediterranean";
            food[11, 1] = String.Format("{0}", foodPreferences.Mediterranean);
            food[12, 0] = "mexican";
            food[12, 1] = String.Format("{0}", foodPreferences.Mexican);
            food[13, 0] = "organic";
            food[13, 1] = String.Format("{0}", foodPreferences.Organic);
            food[14, 0] = "thai";
            food[14, 1] = String.Format("{0}", foodPreferences.Thai);
            food[15, 0] = "vegan";
            food[15, 1] = String.Format("{0}", foodPreferences.Vegan);
            food[16, 0] = "vegetarian";
            food[16, 1] = String.Format("{0}", foodPreferences.Vegetarian);
            food[17, 0] = "spanish";
            food[17, 1] = String.Format("{0}", foodPreferences.Spanish);


            //var cafe =;

            return LargestSum(food, 10);
        }
        public List<String> LargestSum(String[,] array, int returnAmount)
        {
            List<int> foodRanking = new List<int>();
            List<String> topfoodTypes = new List<string>();
            List<int> topRanking = new List<int>();
            for (int x = 0; x < array.GetLength(0); x++)
            {
                int rank = int.Parse(array[x, 1]);
                foodRanking.Add(rank);

            }

            for (int j = 0; j < returnAmount; j++)
            {
                int maxfood = foodRanking.Max();
                int index = foodRanking.IndexOf(maxfood);
                foodRanking.Remove(maxfood);
                topfoodTypes.Add(array[index, 0]);
                Console.WriteLine("maxfood" + maxfood);
                Console.WriteLine("index" + index);
            }
            return topfoodTypes;
        }  

        private void Image_OnSwiped(object sender, SwipeEventArgs e)
        {
            var imageCoupon = (MR.Gestures.Image)sender;
            var _coupon = couponList.couponList.FirstOrDefault(c => c.thisCoupon.ImageStream == imageCoupon.Source);
            if (e.Direction == Direction.Right)
            {
                Debug.WriteLine("ADDING - " + _coupon.thisCoupon.ImageUrl);
                //savedCoupons.Add(_coupon);
                azure.AddToBackpack(_coupon, user);

            }
            else if (e.Direction == Direction.Left)
            {
                Debug.WriteLine("DELETING - " + _coupon.thisCoupon.ImageUrl);

            }
            couponList.couponList.Remove(_coupon);

            foreach (var c in couponList.couponList)
            {
                Debug.WriteLine("CHECKING - " + c.thisCoupon.ImageUrl);
            }
            absLayout.ItemsSource = null;
            absLayout.ItemsSource = couponList.couponList;
        }

        private async void couponTapped(object sender, TapEventArgs e)
        {
            var cpn = (MR.Gestures.Image)sender;
            var _coupon = couponList.couponList.FirstOrDefault(c => c.thisCoupon.ImageStream == cpn.Source);

            //string fixedurl = getSpecificGoogleInfo(_coupon.PlaceId);
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetStringAsync(string.Format(fixedurl));
            //    var result = JsonConvert.DeserializeObject<GooglePlaces>(response);
            //    if (result.result != null)
            //    {
            //        var use = result.result;
            _coupon = await _google.GooglePlaceDetails(_coupon); ;
                    Console.WriteLine("use=>" + _coupon.thisCoupon.GoogleData.name);
               // }
           // }

        }
        
        //private string getSpecificGoogleInfo(string placeId)
        //{
        //    string fixedUrl = detailSearch + placeId + "&key=" + googleKey;
            

        //    return fixedUrl;
        //}
        
        private void Image_Tapped(object sender, TapEventArgs e)
        {
            var cpn = (MR.Gestures.Image)sender;
            var abs = cpn.Parent.Id;
           // Debug.WriteLine("WERE TAPPING OTHER ====" + abs);
            var _coupon = couponList.couponList.FirstOrDefault(c => c.thisCoupon.ImageStream == cpn.Source);
            if(_coupon != null)
            {
                Debug.WriteLine("WERE TAPPING ICON ===" + _coupon.Loc);
               // Debug.WriteLine("WERE TAPPING ICON ===" + _coupon.Longitude);
              //  var findplace = new FindPlacePage(_coupon.Latitude, _coupon.Longitude);
               // await Navigation.PushAsync(findplace);
            }
            
            //Debug.WriteLine("WERE TAPPING ICON");
        }

        
    }
}
    //int firstLargest = 0, secondLargest = 0, thirdLargest = 0, fourthLargest = 0, fifthLargest = 0;
    //List<String> favouriteFood = new List<String>();
    //String firstType = "", secondType = "", thirdType = "", fourthType = "", fifthType = "";
    //for (int x = 0; x < array.GetLength(0); x++)
    //{
    //    int value = Int32.Parse(String.Format("{0}", array[x, 1]));
    //    String type = array[x, 0];
    //    if (value > fifthLargest)
    //    {
    //        if (value > fourthLargest)
    //        {
    //            if (value > thirdLargest)
    //            {
    //                if (value > secondLargest)
    //                {
    //                    if (value > firstLargest)
    //                    {
    //                        fifthType = fourthType;
    //                        fifthLargest = fourthLargest;
    //                        fourthType = thirdType;
    //                        fourthLargest = thirdLargest;
    //                        thirdType = secondType;
    //                        thirdLargest = secondLargest;
    //                        secondType = firstType;
    //                        secondLargest = firstLargest;
    //                        firstType = type;
    //                        firstLargest = value;
    //                    }
    //                    else
    //                    {
    //                        fifthType = fourthType;
    //                        fifthLargest = fourthLargest;
    //                        fourthType = thirdType;
    //                        fourthLargest = thirdLargest;
    //                        thirdType = secondType;
    //                        thirdLargest = secondLargest;
    //                        secondType = type;
    //                        secondLargest = value;
    //                    }
    //                }
    //                else
    //                {
    //                    fifthType = fourthType;
    //                    fifthLargest = fourthLargest;
    //                    fourthType = thirdType;
    //                    fourthLargest = thirdLargest;
    //                    thirdType = type;
    //                    thirdLargest = value;
    //                }

    //            }
    //            else
    //            {
    //                fifthType = fourthType;
    //                fifthLargest = fourthLargest;
    //                fourthType = type;
    //                fourthLargest = value;
    //            }
    //        }
    //        else
    //        {
    //            fifthType = type;
    //            fifthLargest = value;
    //        }
    //    }
    //}

    //favouriteFood.Add(firstType);
    //favouriteFood.Add(secondType);
    //favouriteFood.Add(thirdType);
    //favouriteFood.Add(fourthType);
    //favouriteFood.Add(fifthType);
    //return favouriteFood;

