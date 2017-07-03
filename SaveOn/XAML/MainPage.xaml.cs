using AlphaChiTech.Virtualization;
using Newtonsoft.Json;
using SaveOn.Azure;
using SaveOn.GoogleModels;
using SaveOn.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SaveOn.XAML
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
        private readonly AzureDataService _azure;
        private NavigationPage _navPage;
        //private StartPage _startPage;
        private GoogleModels.Location vm;
        //WebView webView = new WebView();
        //private CouponList couponList = new CouponList();
        //private FacebookUser facebookUser = new FacebookUser();
        private User User = new User();
        //private FacebookUser facebookUser = new FacebookUser();
        private string ClientId = "173756816475288";
        private FacebookUser fbUser;
        
        public MainPage(User _User)
        {
            User = _User;
            Debug.Write("openinig up mainpage");
            //var navigationPage = new NavigationPage(new BackpackMain());
            
            Children.Add(new NavigationPage(new ShoppingList(User)));
            Children.Add(new NavigationPage (new BackpackView(User)));
            //Navigation.

            InitializeComponent();
            
            //WebViewOnNavigated();
        }     
        
    }
}
