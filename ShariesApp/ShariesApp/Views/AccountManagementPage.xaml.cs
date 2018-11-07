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
        private static string currentId = "";
		public AccountManagementPage ()
		{
			InitializeComponent ();
		}
        public static bool checkIfAccountIsBlocked(string text)
        {
            var blockedAccountsList = App.Database.QueryBlockedAccountsByBlocker(App.CurrentAccountNumber);
            foreach (var item in blockedAccountsList)
            {
                if (item.blockee == Convert.ToInt32(text))
                {
                    currentId = item.id;
                    return true;
                }
            }
            return false;
        }
        private void blockAccountButtonClicked(object sender, EventArgs e)
        {
            blockAccountLabel.Text = "";
            if (Int32.TryParse(blockAccountEntry.Text, out int test)) // check if entry value is valid
            {
                if (!checkIfAccountIsBlocked(blockAccountEntry.Text)) 
                {
                    var blockedAccountObject = new BlockedAccounts
                    {
                        blocker = App.CurrentAccountNumber,
                        blockee = Convert.ToInt32(blockAccountEntry.Text)
                    };
                    App.Database.InsertBlockedAccountsAsync(blockedAccountObject);
                    blockAccountLabel.Text = "Account blocked successfully";
                }
                else
                    blockAccountLabel.Text = "Account is already blocked";
            }
            blockAccountEntry.Text = "";
        }
        private void unblockAccountButtonClicked(object sender, EventArgs e)
        {
            unblockAccountLabel.Text = "";
            if (Int32.TryParse(unblockAccountEntry.Text, out int test)) // check if entry value is valid
            {
                if (checkIfAccountIsBlocked(unblockAccountEntry.Text))
                {
                    var blockedAccountObject = new BlockedAccounts
                    {
                        id = currentId,
                        blocker = Convert.ToInt32(App.CurrentAccountNumber),
                        blockee = Convert.ToInt32(unblockAccountEntry.Text)
                    };
                    App.Database.DeleteBlockedAccountsAsync(blockedAccountObject);
                    unblockAccountLabel.Text = "Account unblocked successfully";
                }
                else
                    unblockAccountLabel.Text = "Account is not blocked";
            }
            unblockAccountEntry.Text = "";
        }
        private void changePasswordButtonClicked(object sender, EventArgs e)
        {
            changePasswordLabel.Text = "";
            if (!string.IsNullOrWhiteSpace(changePasswordOne.Text) && !string.IsNullOrWhiteSpace(changePasswordTwo.Text))
            {
                if (changePasswordOne.Text == changePasswordTwo.Text)
                {
                    var response = App.Database.QueryUserDataByAccountNumber(App.CurrentAccountNumber);
                    response.password = changePasswordOne.Text;
                    App.Database.UpdateUserDataAsync(response);
                    changePasswordLabel.Text = "Password changed";
                }
                else
                    changePasswordLabel.Text = "Passwords do not match";
            }
            changePasswordOne.Text = "";
            changePasswordTwo.Text = "";
        }
    }
}