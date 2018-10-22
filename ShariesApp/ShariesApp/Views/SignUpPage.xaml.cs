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
        public static bool signUpSucceeded = true;

        public SignUpPage ()
		{
			InitializeComponent ();
		}

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var user = new UserData()
            {
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                Password = passwordEntry.Text,
                Name = emailEntry.Text
            };
            // Sign up logic goes here
            AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    await App.Database.SaveUserData(user); //store details in db

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

        async void AreDetailsValid(UserData user)
        {
            var userDataTable = await App.Database.GetUserData(); // get user data from db to check if valid
            foreach (var item in userDataTable)
            {
                if (user.AccountNumber != item.AccountNumber && user.AccountNumber != 0 && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Name))
                {
                    signUpSucceeded = true;
                }
            }
        }
    }
}