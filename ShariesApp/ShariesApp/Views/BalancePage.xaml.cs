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
        }
        protected override void OnAppearing()
        {
            DisplayBalance();
            base.OnAppearing();
        }
        public void DisplayBalance()
        {
            var creditResponse = App.Database.QueryCreditDataByAccountNumber(App.CurrentAccountNumber);
            var dataResponse = App.Database.QueryUserDataByAccountNumber(App.CurrentAccountNumber);
            welcomeLabel.Text = "Welcome, " + dataResponse.Name;
            balanceLabel.Text = "";
            balanceLabel.Text += "Current balance:\n";
            balanceLabel.Text += string.Format("Credit: {0}\n", creditResponse.CreditAmount);
            balanceLabel.Text += string.Format("Texts: {0}\n", creditResponse.TextAmount);
            balanceLabel.Text += string.Format("Data: {0}\n", creditResponse.DataAmount);
            balanceLabel.Text += string.Format("Minutes: {0}\n", creditResponse.MinutesAmount);
        }
    }
}