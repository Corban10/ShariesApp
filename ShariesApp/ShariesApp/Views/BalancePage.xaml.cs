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
	public partial class BalancePage : ContentPage
	{
		public BalancePage ()
		{
			InitializeComponent ();
            DisplayWelcome();
            DisplayBalance();
        }
        public void DisplayWelcome()
        {
            welcomeLabel.Text = "Welcome, " + App.loggedInUser.name;
        }
        public void DisplayBalance()
        {
            var response = App.Database.QueryCreditDataByAccountNumber(App.loggedInUser.accountNumber);
            balanceLabel.Text = "";
            balanceLabel.Text += "Current balance:\n";
            balanceLabel.Text += string.Format("Credit: {0}\n", response.creditAmount);
            balanceLabel.Text += string.Format("Texts: {0}\n", response.textAmount);
            balanceLabel.Text += string.Format("Data: {0}\n", response.dataAmount);
            balanceLabel.Text += string.Format("Minutes: {0}\n", response.minutesAmount);
        }
        private void ReloadBalance(object sender, EventArgs e)
        {
            DisplayBalance();
        }
    }
}