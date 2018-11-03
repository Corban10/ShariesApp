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
            if (Int32.TryParse(blockAccountEntry.Text, out int test))
            {
                var blockedAccountsList = App.Database.QueryBlockedAccountsByBlocker(Convert.ToInt32(MainPage.loggedInUser.accountNumber));
                bool blockeeAlreadyExists = false;
                foreach (var item in blockedAccountsList)
                {
                    if (item.blockee == Convert.ToInt32(blockAccountEntry.Text))
                    {
                        blockeeAlreadyExists = true;
                    }
                }
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
                    {
                        blockAccountLabel.Text += string.Format("{0}\n", item.blockee);
                    }
                    blockAccountLabel.Text += string.Format("{0}\n", blockAccountEntry.Text);
                }
            }
            else
            {
                blockAccountLabel.Text = "Invalid Account number";
            }
        }
    }
}