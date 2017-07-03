using MR.Gestures;
using SaveOn.GoogleModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SaveOn.ViewModels
{
    public class Coupon : TransformViewModel
    {


        //public string[] images = new[] { "Flusi1.jpg", "Flusi2.jpg", "Flusi3.jpg" };
        //protected string[] images = new[] { "Pic1.png", "Pic2.png", "Pic3.png", "Pic4.png" };
        //protected int currentImage = 0;
        public ICommand TappedCommand { get; protected set; }
        public int id { get; set; }
        public string ImageUrl { get; set; }
        public ImageSource ImageStream { get; set; }
        public string CouponType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string jpgImageUrl { get { return ImageUrl + ".jpg"; } }
        
        public Result GoogleData { get; set; }
        protected override void OnSwiped(MR.Gestures.SwipeEventArgs e)
        {
            base.OnSwiped(e);
            //ShoppingList mainInfo = new ShoppingList();
            Debug.WriteLine("direction---" + e.Direction);
            Debug.WriteLine("direction---" + e.Center);
            if (e.Direction == MR.Gestures.Direction.Right)
            {
                // mainInfo.addCoupon(this);          
                Debug.WriteLine("right");
            }
            else if (e.Direction == MR.Gestures.Direction.Left)
            {
                Debug.WriteLine("left");
                //mainInfo.deleteCoupon(this);

            }

        }
        public string TypeLink { get
            {
                var t = GoogleData.types.First();
                if (t != null)
                    return t+".png";
                else
                    return "";
            }
        }
        protected virtual void OnTapped(TapEventArgs e)
        {
            Console.WriteLine("TAPPING COUPON");
            Console.WriteLine(e.Sender);
            Console.WriteLine(id);
            Console.WriteLine(ImageUrl);
            //AddText(TapInfo("Tapped", e));
        }

        public Coupon()
            : base()
        {
            TappedCommand = new Command<TapEventArgs>(OnTapped);
        }
    }
}
