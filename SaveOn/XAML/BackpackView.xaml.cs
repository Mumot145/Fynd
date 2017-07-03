using SaveOn.Azure;
using SaveOn.Models;
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
    public partial class BackpackView
    {
        string typeSelected = "";
        AzureDataService azure = new AzureDataService();
        User user = new User();
        public BackpackView(User _user)
        {
            user = _user;
            Debug.WriteLine("backpack has =>"+ _user.name);
            InitializeComponent();
        }

        void Food_Selected(object sender, EventArgs e)
        {
            typeSelected = "food";
            NavigateToPage(typeSelected);
        }
        void Apparal_Selected(object sender, EventArgs e)
        {
            typeSelected = "apparal";
            NavigateToPage(typeSelected);
        }
        void Shoes_Selected(object sender, EventArgs e)
        {
            typeSelected = "shoes";
            NavigateToPage(typeSelected);
        }
        void Electronics_Selected(object sender, EventArgs e)
        {
            typeSelected = "electronics";
            NavigateToPage(typeSelected);
        }
        void HandW_Selected(object sender, EventArgs e)
        {
            typeSelected = "health";
            NavigateToPage(typeSelected);
        }
        void Fitness_Selected(object sender, EventArgs e)
        {
            typeSelected = "fitness";
            
            NavigateToPage(typeSelected);
        }
        public async void NavigateToPage(string type)
        {
           // var inBackpack = new CouponList();
           // inBackpack.backpackType = type;
           // inBackpack.couponList = await azure.GetBackpack(user);
            //await Navigation.PushAsync(new BackpackList());
            //list.SelectedItem = null;
        }
    }
}