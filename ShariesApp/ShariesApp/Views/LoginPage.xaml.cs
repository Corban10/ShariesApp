using ShariesApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void LoginToSystem(object sender, EventArgs e)
        {
            if (!AreCredentialsCorrect(GetUserData()) || !App.IsConvertibleToInt(usernameEntry.Text))
            {
                messageLabel.Text = "Login failed";
                return;
            }
            passwordEntry.Text = string.Empty;
            App.CurrentAccountNumber = GetUserData().AccountNumber;
            App.IsUserLoggedIn = true;
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync();
        }

        private UserData GetUserData()
        {
            return new UserData
            {
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                Password = passwordEntry.Text
            };
        }

        private static bool AreCredentialsCorrect(UserData user)
        {
            var responseData = App.Database.QueryUserDataByAccountNumber(user.AccountNumber);
            return (responseData.Password == user.Password && user.AccountNumber > 0);
        }
    }
}
