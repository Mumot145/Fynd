using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using System;
using XLabs.Forms;
using Android.Gms.Maps;
using Microsoft.WindowsAzure.MobileServices;

namespace SaveOn.Droid
{
    [Activity(Label = "GestureSample", Icon = "@drawable/icon", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity :  XFormsApplicationDroid
    {


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            IEnumerable<Account> accounts = AccountStore.Create(Forms.Context).FindAccountsForService("Facebook");
            
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            MR.Gestures.Android.Settings.LicenseKey = "ALZ9-BPVU-XQ35-CEBG-5ZRR-URJQ-ED5U-TSY8-6THP-3GVU-JW8Z-RZGE-CQW6";
            Console.WriteLine(accounts.FirstOrDefault());
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            CurrentPlatform.Init();
            
            if (accounts != null)
            {
                LoadApplication(new App(accounts.FirstOrDefault()));
            } else
            {
                LoadApplication(new App(null));
            }
            
        }
        
    }
}

