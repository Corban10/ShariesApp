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
            if (App.CheckIsConvertableToInt(usernameEntry.Text))
            {
                var user = new UserData
                {
                    accountNumber = Convert.ToInt32(usernameEntry.Text),
                    password = passwordEntry.Text
                };
                if (user.accountNumber > 0)
                {
                    if (AreCredentialsCorrect(user))
                    {
                        App.CurrentAccountNumber = user.accountNumber;
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
            }
            else
                messageLabel.Text = "Invalid Account Number";
        }
        private bool AreCredentialsCorrect(UserData user)
        {
            var responseData = App.Database.QueryUserDataByAccountNumber(user.accountNumber);
            if (responseData.password == user.password)
            {
                return true;
            }
            return false;
        } 
    }
}
