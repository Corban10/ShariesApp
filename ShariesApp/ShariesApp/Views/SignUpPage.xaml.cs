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
	public partial class SignUpPage : ContentPage
    {

        public SignUpPage ()
		{
			InitializeComponent ();
		}
        async void OnSignUpButtonClicked(object sender, EventArgs e) // bug here somewhere where signup doesnt load loggedInUser properly
        {
            if (App.CheckIsConvertableToInt(usernameEntry.Text))
            {
                var user = new UserData
                {
                    accountNumber = Convert.ToInt32(usernameEntry.Text),
                    password = passwordEntry.Text,
                    name = nameEntry.Text
                };
                var userCredit = new UserCredit
                {
                    accountNumber = Convert.ToInt32(usernameEntry.Text),
                    creditAmount = 1000,
                    textAmount = 1000,
                    dataAmount = 1000,
                    minutesAmount = 1000
                };
                if (AreDetailsValid(user))
                {
                    //create user row in db
                    App.Database.InsertUserDataAsync(user);
                    App.Database.InsertCreditDataAsync(userCredit);
                    App.CurrentAccountNumber = user.accountNumber;
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
            var responseData = App.Database.QueryUserDataByAccountNumber(user.accountNumber);
            if (string.IsNullOrWhiteSpace(responseData.id))
            {
                if (user.accountNumber > 0 && !string.IsNullOrWhiteSpace(user.password) && !string.IsNullOrWhiteSpace(user.name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}