using Plugin.Geolocator;
using SaveOn.GoogleModels;
using SaveOn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using SaveOn.Models;

namespace SaveOn.XAML
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindPlacePage : ContentPage
	{
        
        Map _map = new Map();
		public FindPlacePage (CouponSchedule couponInfo, Plugin.Geolocator.Abstractions.Position pos)
        {
           
            InitializeComponent ();
            double Lat = pos.Latitude;
            double Long = pos.Longitude;
            var googleInfo = couponInfo.thisCoupon.GoogleData;
            // double myLat = _myLocation.lat;
            //  double myLong = _myLocation.lng;
            // locator = CrossGeolocator.Current.;
            StackLayout mainStack = new StackLayout() { Margin=10 };
            StackLayout infoStack = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest=300 };
            Label phoneNumber = new Label() { Text = googleInfo.phone_number };
            Label openNow = new Label() { Text = googleInfo.opening_hours.open_now.ToString() };
            infoStack.Children.Add(phoneNumber);
            infoStack.Children.Add(openNow);
            // List<Label> openHours = new List<Label>() { }
            var position = new Position(googleInfo.geometry.location.lat, googleInfo.geometry.location.lng); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = couponInfo.thisCoupon.GoogleData.name,
                Address = couponInfo.thisCoupon.GoogleData.address
            };
            _map.Pins.Add(pin);
            _map.IsShowingUser = true;
           
            _map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                    new Position(Lat, Long),  // latlng
                    14d, // zoom
                    30d, // rotation
                    60d));
            
            mainStack.Children.Add(_map);
            mainStack.Children.Add(infoStack);
            // mainStack.Children.Add();
            Content = mainStack;


        }
        protected async override void OnAppearing()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();
            //locator.PositionChanged += (sender, e) => {
            //    Debug.WriteLine("WE ARE AT :"+e.Position.Latitude +"*"+e.Position.Longitude);
            //};
            Console.WriteLine("WE ARE ALSO AAAAAAAAAAAAT:"+position.Latitude+"&"+position.Longitude);
           // _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
              //                               Distance.FromMiles(1)));
        }

    }
}
