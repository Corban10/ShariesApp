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
        public static bool signUpSucceeded = false;

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
                    MainPage.loggedInUser = user;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = "Sign up failed";
            }
        }

        void AreDetailsValid(UserData user)
        {
            var task = App.Database.GetUserDataFromPK(user.AccountNumber);
            task.Wait();
            var responseData = task.Result;
            if (responseData == null)
            {
                if (user.AccountNumber != 0 && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Name))
                {
                    signUpSucceeded = true;
                }
            }
        }
    }
}