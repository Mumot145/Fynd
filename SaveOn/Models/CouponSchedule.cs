using MR.Gestures;
using Newtonsoft.Json;
using SaveOn.GoogleModels;
using SaveOn.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static SaveOn.CustomEventArgs;

namespace SaveOn.Models
{
    

    public class CouponSchedule
    {
        public delegate void FindCouponHandler(object sender, FindEventArgs e);
        public event FindCouponHandler OnFindCoupon;
        public ICommand TappedCommand { get; protected set; }
        private string _id;
        private Location _location = new Location();
        private string _placeId;
        private int _couponId;
        private DateTime _startDate;
        private DateTime _endDate;
        
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty(PropertyName = "placeId")]
        public string PlaceId
        {
            get { return _placeId; }
            set { _placeId = value; }
        }
        [JsonProperty(PropertyName = "couponId")]
        public int CouponId
        {
            get { return _couponId; }
            set { _couponId = value; }
        }
        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        public Location Loc
        {
            get { return _location; }
            set { _location = value; }
        }

        public Coupon thisCoupon { get; set; }
        public CouponSchedule()
        {
            TappedCommand = new Command<TapEventArgs>(OnTapped);
        }
        protected virtual void OnTapped(TapEventArgs e)
        {
            Console.WriteLine("TAPPING LOCATION");
            Console.WriteLine(e.Sender);
            Console.WriteLine(Loc.lat);
            Console.WriteLine(Loc.lng);
            // Console.WriteLine(Coordinates.lat);
            // Console.WriteLine(Coordinates.lng);
            Console.WriteLine(_placeId);
            if (OnFindCoupon == null) return;

            FindEventArgs args = new FindEventArgs(this);
            OnFindCoupon(this, args);
            ///OnFindCoupon(new FindEventArgs(_location));
            //AddText(TapInfo("Tapped", e));
        }
        public bool notInSystem { get; set; }
        //public event MyEventDelegate OnFindLocation;
    }
}
