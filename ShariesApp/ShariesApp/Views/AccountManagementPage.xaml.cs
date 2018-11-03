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
	public partial class AccountManagementPage : ContentPage
	{
		public AccountManagementPage ()
		{
			InitializeComponent ();
		}
        private void blockAccountButtonClicked(object sender, EventArgs e)
        {
            blockAccountLabel.Text = "";
            if (Int32.TryParse(blockAccountEntry.Text, out int test)) // check if entry value is valid
            {
                // check if blocked // make this a method maybe
                var blockedAccountsList = App.Database.QueryBlockedAccountsByBlocker(Convert.ToInt32(MainPage.loggedInUser.accountNumber));
                bool blockeeAlreadyExists = false;
                foreach (var item in blockedAccountsList) 
                    if (item.blockee == Convert.ToInt32(blockAccountEntry.Text))
                        blockeeAlreadyExists = true;
                // if it doesnt, then block
                if (!blockeeAlreadyExists) 
                {
                    var blockedAccountObject = new BlockedAccounts
                    {
                        blocker = Convert.ToInt32(MainPage.loggedInUser.accountNumber),
                        blockee = Convert.ToInt32(blockAccountEntry.Text)
                    };
                    App.Database.InsertBlockedAccountsAsync(blockedAccountObject);
                    blockAccountLabel.Text += "Blocked accounts:\n";
                    foreach (var item in blockedAccountsList)
                        blockAccountLabel.Text += string.Format("{0}\n", item.blockee);
                    blockAccountLabel.Text += string.Format("{0}\n", blockAccountEntry.Text);
                    blockAccountLabel.Text = "Account blocked successfully";
                }
                else
                    blockAccountLabel.Text = "Account is already blocked";
            }
            blockAccountEntry.Text = "";
        }
        private void unblockAccountButtonClicked(object sender, EventArgs e)
        {

        }
        private void changePasswordButtonClicked(object sender, EventArgs e)
        {

        }
    }
}