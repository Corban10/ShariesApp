using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShariesApp
{
	public partial class MainPage : ContentPage
    {
        public static UserData loggedInUser;
        public MainPage()
		{
			InitializeComponent();
		}
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            LoginPage.isValid = false;
            SignUpPage.signUpSucceeded = false;
            loggedInUser = new UserData();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}
