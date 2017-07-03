using Microsoft.WindowsAzure.MobileServices;
using SaveOn.Models;
using SaveOn.XAML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SaveOn
{

    public partial class App : Application
    {

        static string _Token;
        public static NavigationPage _navPage;
        private CouponList _couponList;
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static string fbUserJson;
        //private FacebookConnect _fbConnect = new FacebookConnect();
        private User _user = new User();
        
        public App(Account _account)
        {
            InitializeComponent();
            string aToken = "";

            if (_account != null)
                aToken = _account.Properties["access_token"];

            MainPage = _navPage = new NavigationPage(new StartPage(aToken));
     
           // _navPage.
        }
        public static MobileServiceClient MobileService =
                new MobileServiceClient(
                    Constants.AzureMobileService
                );
        public static bool IsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }
        public static void receiveJson(string _json)
        {
            //_user.name = _name;
            fbUserJson = _json;
        }
        public static string Token
        {
            get { return _Token; }
        }

        public static void SaveToken(string token)
        {
            _Token = token;
        }

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => {
                    Debug.WriteLine("logged in");
                    _navPage.Navigation.PopModalAsync();
                    

                    //_navPage.returnPage();

                });
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
