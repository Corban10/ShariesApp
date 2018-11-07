using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            InitializeComponent();
            var response = App.Database.QueryUserDataByAccountNumber(App.CurrentAccountNumber);
            // if admin
            if (response.accountNumber <= 20 && response.accountNumber > 0)
            {
                this.Children.Add(new AdminPage() { Title = "Admin" });
            }
            // only here as temporary fix to bug where user data query wasnt returning in time
            else if (response.accountNumber == 0)
            {
                this.Children.Add(new BalancePage() { Title = "Balance" }); 
            }
            // else is user
            else
            {
                this.Children.Add(new BalancePage() { Title = "Balance" });
                this.Children.Add(new SendPage() { Title = "Send" });
                this.Children.Add(new RequestPage() { Title = "Requests" });
                this.Children.Add(new AccountManagementPage() { Title = "Manage Account" });
            }
            // Debug.WriteLine(response.accountNumber);
        }
        async void LogOutOfSystem(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            App.CurrentAccountNumber = 0;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}