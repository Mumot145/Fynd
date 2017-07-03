using Newtonsoft.Json;
using SaveOn.Azure;
using SaveOn.Google;
using SaveOn.GoogleModels;
using SaveOn.Http;
using SaveOn.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Data;
using SaveOn.GPS;
using XLabs.Forms.Controls;
using AlphaChiTech.Virtualization;

namespace SaveOn.XAML
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
        string token;
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPage"/> class.
        /// </summary>
        /// 
        SocialMedia _social = new SocialMedia();
        GoogleService google;
        AzureDataService _azure = new AzureDataService();
        public StartPage(string _aToken)
        {
            InitializeComponent();

            if (_aToken == null)
                token = App.Token;
            else if (_aToken != null)
                token = _aToken;
            else
                return;
            google = GoogleService.DefaultGoogleInstance;
            FacebookButton.Clicked += ButtonClick;
            //Showing custom font in image button
            FacebookButton.Font = Font.OfSize("Open 24 Display St", 20);
            if (!VirtualizationManager.IsInitialized)
            {
                VirtualizationManager.Instance.UIThreadExcecuteAction = (a) => Xamarin.Forms.Device.BeginInvokeOnMainThread(a);
                Xamarin.Forms.Device.StartTimer(
                    TimeSpan.FromSeconds(1),
                    () => {
                        VirtualizationManager.Instance.ProcessActions(); return true;
                    });
            }

            this.BindingContext = new ButtonPageViewModel();
        }
        protected async override void OnAppearing()
        {
            FacebookUser _facebookUser = new FacebookUser();
            User _user = new User();
            if (!String.IsNullOrEmpty(token))
            {
                _facebookUser = await _social.GetFacebookProfileAsync(token);
                var gps = new GetGPS();
                var loc = await gps.updateLocation();

                //List<CouponSchedule> googledCoupons = await google.GoogleNearby(loc);
                 _user = _azure.GetUser(_facebookUser.Id,"fbId");
                
                    //_user = _azure.GetUserInfo(_facebookUser.Id);                
                    await Navigation.PushModalAsync(new MainPage(_user));
                //else if(token == null)
                //    await Navigation.PushModalAsync(new MainPage(App.Token));
                //Debug.WriteLine("token in app is =" + App.Token);
                //Debug.WriteLine("back!!");
                //token = App.Token;

            }
        }
        
        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void ButtonClick(object sender, EventArgs e)
        {
            var button = sender as ImageButton;

            var location = button.Text;
            if(location == "Facebook")
            {
                
                await Navigation.PushModalAsync(new FacebookLoginPage());
                Debug.WriteLine("just returned!");
                //await Navigation.PopModalAsync();
            }
        }
    }
    
    public class ButtonPageViewModel : ObservableObject
    {
        private bool buttonEnabled;

        public bool ButtonEnabled
        {
            get
            {
                return this.buttonEnabled;
            }
            set
            {
                if (this.SetProperty(ref this.buttonEnabled, value))
                {
                    this.NotifyPropertyChanged("EnabledButtonTitle");
                }
            }
        }

        public string EnabledButtonTitle
        {
            get
            {
                return this.buttonEnabled ? "Enabled Image" : "Disabled image";
            }
        }
    }
}
