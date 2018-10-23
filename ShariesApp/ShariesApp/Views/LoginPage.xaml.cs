﻿using System;
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
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                Password = passwordEntry.Text
            };

            AreCredentialsCorrect(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
		        MainPage.loggedInUser = user;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync(); 
                // await Navigation.PushAsync(new MainPage());
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }
        void AreCredentialsCorrect(UserData user)
        {
            var responseData = Task.Run(async () => {
                return await App.Database.GetUserDataFromPK(user.AccountNumber);
            }).Result;
            if (responseData != null)
            {
                if (responseData.Password == user.Password)
                {
                    isValid = true;
                }
            }
            /*
            var task = App.Database.GetUserDataFromPK(user.AccountNumber);
            task.Wait();
            var responseData = task.Result;
            */
        }
    }
}
