using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountManagementPage : ContentPage
    {
        public AccountManagementPage()
        {
            InitializeComponent();
        }

        private async void BlockAccount(object sender, EventArgs e)
        {
            if (!App.IsConvertibleToInt(blockAccountEntry.Text) || !await ConfirmationResponse("Block account?"))
            {
                blockAccountLabel.Text = "Account not blocked";
                return;
            }
            if (AccountIsBlocked(blockAccountEntry.Text))
            {
                blockAccountLabel.Text = "Account is already blocked";
                return;
            }
            App.Database.InsertBlockedAccountsAsync(GetBlockAccount());
            blockAccountLabel.Text = "Account blocked successfully";
            blockAccountEntry.Text = "";
        }

        private async Task<bool> ConfirmationResponse(string message)
        {
            return await DisplayAlert(message, "Are you sure?", "Yes", "No");
        }

        private static string _currentId = "";

        private static bool AccountIsBlocked(string blockee)
        {
            var blockedAccountsList = App.Database.QueryBlockedAccountsByBlocker(App.CurrentAccountNumber);
            foreach (var item in blockedAccountsList)
            {
                if (item.Blockee == Convert.ToInt32(blockee))
                {
                    _currentId = item.BlockId;
                    return true;
                }
            }
            return false;
        }

        private BlockedAccounts GetBlockAccount()
        {
            return new BlockedAccounts
            {
                Blocker = App.CurrentAccountNumber,
                Blockee = Convert.ToInt32(blockAccountEntry.Text)
            };
        }

        private async void UnblockAccount(object sender, EventArgs e)
        {
            if (!await ConfirmationResponse("Unblock account?") || !App.IsConvertibleToInt(unblockAccountEntry.Text))
            {
                unblockAccountLabel.Text = "Unblock unsuccessful";
                return;
            }
            if (!AccountIsBlocked(unblockAccountEntry.Text))
            {
                unblockAccountLabel.Text = "Account is not blocked";
                return;
            }
            App.Database.DeleteBlockedAccountsAsync(GetUnblockAccount());
            unblockAccountLabel.Text = "Account unblocked successfully";
            unblockAccountEntry.Text = "";
        }

        private BlockedAccounts GetUnblockAccount()
        {
            return new BlockedAccounts
            {
                BlockId = _currentId,
                Blocker = Convert.ToInt32(App.CurrentAccountNumber),
                Blockee = Convert.ToInt32(unblockAccountEntry.Text)
            };
        }

        private async void ChangePassword(object sender, EventArgs e)
        {
            if (PasswordEntryIsValid() && await ConfirmationResponse("Change password?"))
            {
                InsertNewPassword();
                changePasswordLabel.Text = "Password changed";
            }
            else
                changePasswordLabel.Text = "Password change unsuccessful";
            changePasswordOne.Text = "";
            changePasswordTwo.Text = "";
        }

        private bool PasswordEntryIsValid()
        {
            if (string.IsNullOrWhiteSpace(changePasswordOne.Text) || string.IsNullOrWhiteSpace(changePasswordTwo.Text))
                return false;
            return changePasswordOne.Text == changePasswordTwo.Text;
        }

        private void InsertNewPassword()
        {
            var currentUserData = App.Database.QueryUserDataByAccountNumber(App.CurrentAccountNumber);
            currentUserData.Password = changePasswordOne.Text;
            App.Database.UpdateUserDataAsync(currentUserData);
        }
    }
}