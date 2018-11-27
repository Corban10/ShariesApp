using ShariesApp.Views;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
    {

        public SignUpPage ()
		{
			InitializeComponent ();
		}
        async void SignUp(object sender, EventArgs e) // bug here somewhere where signup doesnt load loggedInUser properly
        {
            if (App.IsConvertibleToInt(usernameEntry.Text))
            {
                var user = new UserData
                {
                    AccountNumber = Convert.ToInt32(usernameEntry.Text),
                    Password = passwordEntry.Text,
                    Name = nameEntry.Text
                };
                var userCredit = new UserCredit
                {
                    AccountNumber = Convert.ToInt32(usernameEntry.Text),
                    CreditAmount = 1000,
                    TextAmount = 1000,
                    DataAmount = 1000,
                    MinutesAmount = 1000
                };
                if (AreDetailsValid(user))
                {
                    //create user row in db
                    App.Database.InsertUserDataAsync(user);
                    App.Database.InsertCreditDataAsync(userCredit);
                    App.CurrentAccountNumber = user.AccountNumber;
                    App.IsUserLoggedIn = true;
                    var rootPage = Navigation.NavigationStack.FirstOrDefault();
                    if (rootPage != null)
                    {
                        Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    messageLabel.Text = "Sign up failed";
                }
            }
            else
                messageLabel.Text = "Invalid Account Number";
        }
        private bool AreDetailsValid(UserData user)
        {
            var responseData = App.Database.QueryUserDataByAccountNumber(user.AccountNumber);
            if (string.IsNullOrWhiteSpace(responseData.UserId))
            {
                if (user.AccountNumber > 0 && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}