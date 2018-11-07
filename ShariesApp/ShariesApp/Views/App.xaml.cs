using ShariesApp.Views;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ShariesApp
{
	public partial class App : Application
    {
        public static UserData loggedInUser { get; set; }
        public static SystemData limits { get; set; }
        static ShariesAzureDatabase database;
        public static bool IsUserLoggedIn { get; set; }
        public App ()
		{
			InitializeComponent();
            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
		}
        public static ShariesAzureDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ShariesAzureDatabase();
                }
                return database;
            }
        }
        public static bool checkIsConvertableToDouble(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Double.TryParse(input, out double e));
        }
        public static bool checkIsConvertableToInt(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Int32.TryParse(input, out int e));
        }
        protected override void OnStart ()
        {
            limits = Database.GetSystemData("1");
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
