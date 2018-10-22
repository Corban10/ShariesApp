using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
        public static bool isValid = false;
	public static UserData loggedInUser = new UserData();
        public LoginPage ()
		{
			InitializeComponent ();
		}
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());  //this means go to signuppage
        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new UserData
            {
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                Password = passwordEntry.Text
            };

            AreCredentialsCorrect(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
		loggedInUser = user;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync(); //this means go back a page
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }
        async void AreCredentialsCorrect(UserData user)
        {
            var userDataTable = await App.Database.GetUserData(); // get user data from db to check if valid

            foreach (var item in userDataTable)
            {
                if (item.AccountNumber == user.AccountNumber && item.Password == user.Password && user.AccountNumber != 0 && user.Password != null)
                {
                    isValid = true;
                }
            }
        }
    }
}
