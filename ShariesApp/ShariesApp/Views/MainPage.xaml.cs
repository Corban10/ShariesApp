using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var userData = App.Database.QueryUserDataByAccountNumber(App.CurrentAccountNumber);
            if (IsAdmin(userData))
            {
                this.Children.Add(new AdminPage() { Title = "Admin" });
            }
            else if (IsCustomer(userData))
            {
                this.Children.Add(new BalancePage() { Title = "Balance" });
                this.Children.Add(new SendPage() { Title = "Send" });
                this.Children.Add(new RequestPage() { Title = "Requests" });
                this.Children.Add(new AccountManagementPage() { Title = "Manage Account" });
            }
            base.OnAppearing();
        }

        private static bool IsAdmin(UserData response)
        {
            return response.AccountNumber <= 20 && response.AccountNumber > 0;
        }

        private static bool IsCustomer(UserData response)
        {
            return response.AccountNumber > 20;
        }

        private async void LogOutOfSystem(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            App.CurrentAccountNumber = 0;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}