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
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            if (Int32.TryParse(usernameEntry.Text, out int test))
            {
                var user = new UserData
                {
                    accountNumber = Convert.ToInt32(usernameEntry.Text),
                    password = passwordEntry.Text,
                    name = nameEntry.Text
                };
                if (AreDetailsValid(user))
                {
                    var rootPage = Navigation.NavigationStack.FirstOrDefault();
                    if (rootPage != null)
                    {
                        App.Database.InsertUserDataAsync(user); //store details in db
                        MainPage.loggedInUser = App.Database.QueryUserDataById(user.accountNumber);
                        App.IsUserLoggedIn = true;
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
            var responseData = App.Database.QueryUserDataById(user.accountNumber);
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