using ShariesApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
        public LoginPage ()
		{
			InitializeComponent ();
		}
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
        async void LoginToSystem(object sender, EventArgs e)
        {
            if (App.IsConvertibleToInt(usernameEntry.Text))
            {
                var user = new UserData
                {
                    AccountNumber = Convert.ToInt32(usernameEntry.Text),
                    Password = passwordEntry.Text
                };
                if (AreCredentialsCorrect(user))
                {
                    App.CurrentAccountNumber = user.AccountNumber;
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    messageLabel.Text = "Login failed";
                    passwordEntry.Text = string.Empty;
                }
            }
            else
                messageLabel.Text = "Invalid Account Number";
        }
        private bool AreCredentialsCorrect(UserData user)
        {
            var responseData = App.Database.QueryUserDataByAccountNumber(user.AccountNumber);
            if (responseData.Password == user.Password && user.AccountNumber > 0)
            {
                return true;
            }
            return false;
        } 
    }
}
