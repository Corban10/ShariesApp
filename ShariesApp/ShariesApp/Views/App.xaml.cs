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
        static ShariesDataBase database;
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
                MainPage = new NavigationPage(new ShariesApp.MainPage());
            }
		}

        public static ShariesDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ShariesDataBase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sharies.db3"));
                }
                return database;
            }
        }
        protected override void OnStart ()
		{
            //Debug.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sharies.db3"));
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
