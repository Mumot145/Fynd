using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SaveOn.XAML
{
	public partial class FacebookLoginPage : ContentPage
	{
		public FacebookLoginPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnDisappearing()
        {
            //await Navigation.PushModalAsync(new MainPage(App.Token));
        }

    }
}
