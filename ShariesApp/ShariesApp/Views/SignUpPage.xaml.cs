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
        public static bool signUpSucceeded = false;

        public SignUpPage ()
		{
			InitializeComponent ();
		}
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var user = new UserData
            {
                id = usernameEntry.Text,
                Password = passwordEntry.Text,
                Name = nameEntry.Text
            };
            AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.Database.InsertUserDataAsync(user); //store details in db
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
            var responseData = App.Database.QueryUserDataById(user.id);
            if (string.IsNullOrWhiteSpace(responseData.id))
            {
                if (!string.IsNullOrWhiteSpace(user.id) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Name))
                {
                    signUpSucceeded = true;
                }
            }
        }
    }
}