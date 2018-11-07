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
            DisplayBalance();
        }
        public void DisplayBalance()
        {
            var creditResponse = App.Database.QueryCreditDataByAccountNumber(App.currentAccountNumber);
            var dataResponse = App.Database.QueryUserDataByAccountNumber(App.currentAccountNumber);
            welcomeLabel.Text = "Welcome, " + dataResponse.name;
            balanceLabel.Text = "";
            balanceLabel.Text += "Current balance:\n";
            balanceLabel.Text += string.Format("Credit: {0}\n", creditResponse.creditAmount);
            balanceLabel.Text += string.Format("Texts: {0}\n", creditResponse.textAmount);
            balanceLabel.Text += string.Format("Data: {0}\n", creditResponse.dataAmount);
            balanceLabel.Text += string.Format("Minutes: {0}\n", creditResponse.minutesAmount);
        }
        private void ReloadBalance(object sender, EventArgs e)
        {
            DisplayBalance();
        }
    }
}