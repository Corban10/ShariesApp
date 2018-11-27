using ShariesApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ShariesApp
{
	public partial class App : Application
    {
        static ShariesAzureDatabase _database;
        public static int CurrentAccountNumber { get; set; }
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
                if (_database == null)
                {
                    _database = new ShariesAzureDatabase();
                }
                return _database;
            }
        }
        public static bool IsConvertibleToDouble(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Double.TryParse(input, out double e));
        }
        public static bool IsConvertibleToInt(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Int32.TryParse(input, out int e));
        }
        protected override void OnStart ()
        {
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
