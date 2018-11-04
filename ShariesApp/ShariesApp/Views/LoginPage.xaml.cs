using ShariesApp.Views;
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
        public LoginPage ()
		{
			InitializeComponent ();
		}
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new UserData
            {
                accountNumber = usernameEntry.Text,
                password = passwordEntry.Text
            };
            if (!string.IsNullOrWhiteSpace(user.accountNumber))
            {
                AreCredentialsCorrect(user);
            }
            if (isValid)
            {
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }
        void AreCredentialsCorrect(UserData user)
        {
            var responseData = App.Database.QueryUserDataById(user.accountNumber); // App.Database.GetUserData(user.id);

            if (responseData.password == user.password)
            {
                MainPage.loggedInUser = responseData;
                isValid = true;
            }
        } 
    }
}
