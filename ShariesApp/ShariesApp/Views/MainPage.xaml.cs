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
        public MainPage ()
        {
            InitializeComponent();
            var isAdmin = !string.IsNullOrWhiteSpace(loggedInUser.id) ? Convert.ToInt32(loggedInUser.id) : 21;
            if (isAdmin > 15)
            {
                this.Children.Add(new SendPage() { Title = "Send" });
                this.Children.Add(new RequestPage() { Title = "Request" });
                this.Children.Add(new AccountManagementPage() { Title = "Manage Account" });
            }
            if (isAdmin <= 15 && isAdmin >= 0)
            {
                this.Children.Add(new AdminPage() { Title = "Admin" });
            }
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            LoginPage.isValid = false;
            SignUpPage.signUpSucceeded = false;
            loggedInUser = new UserData();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}