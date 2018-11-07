using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public static UserData loggedInUser;
        public static SystemData limits;
        public MainPage ()
        {
            InitializeComponent();
            bool isAdmin = loggedInUser.accountNumber <= 20;
            limits = App.Database.GetSystemData("1");
            if (isAdmin)
            {
                this.Children.Add(new AdminPage() { Title = "Admin" });
            }
            else
            {
                // this.Children.Add(new BalancePage() { Title = "Balance" });
                this.Children.Add(new SendPage() { Title = "Send" });
                this.Children.Add(new RequestPage() { Title = "Requests" });
                this.Children.Add(new AccountManagementPage() { Title = "Manage Account" });
            }
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            loggedInUser = new UserData();
            limits = new SystemData();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}