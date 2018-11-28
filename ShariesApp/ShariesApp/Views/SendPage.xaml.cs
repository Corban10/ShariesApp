using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendPage : ContentPage
    {
        private static Picker _senderPicker;
        private static int _senderSelectedIndex;
        public SendPage()
        {
            InitializeComponent();
        }
        private async void SendCredit(object sender, EventArgs e)
        {
            sendStatusLabel.Text = "";
            if (!await ConfirmationResponse() || !EntryIsValid())
                return;
            var myBalance = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(App.CurrentAccountNumber));
            var destinationAccount = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text));
            var limits = App.Database.GetSystemData("1");
            var amount = Convert.ToDouble(sendAmountEntry.Text);
            if (!DestinationIsValid(destinationAccount, myBalance))
                return;
            switch (_senderSelectedIndex)
            {
                case 0:
                    if (!CanSendCredit(myBalance.CreditAmount, amount, limits.CreditLimit))
                        return;
                    myBalance.CreditAmount -= amount;
                    destinationAccount.CreditAmount += amount;
                    break;
                case 1:
                    if (!CanSendCredit(myBalance.TextAmount, amount, limits.TextLimit))
                        return;
                    myBalance.TextAmount -= amount;
                    destinationAccount.TextAmount += amount;
                    break;
                case 2:
                    if (!CanSendCredit(myBalance.DataAmount, amount, limits.DataLimit))
                        return;
                    myBalance.DataAmount -= amount;
                    destinationAccount.DataAmount += amount;
                    break;
                case 3:
                    if (!CanSendCredit(myBalance.MinutesAmount, amount, limits.MinutesLimit))
                        return;
                    myBalance.MinutesAmount -= amount;
                    destinationAccount.MinutesAmount += amount;
                    break;
                default:
                    return;
            }
            TransferCredit(myBalance, destinationAccount);
        }

        private async Task<bool> ConfirmationResponse()
        {
            return await DisplayAlert("Send", "Are you sure?", "Yes", "No");
        }

        private bool EntryIsValid()
        {
            if (App.IsConvertibleToInt(accountNumberEntry.Text) && App.IsConvertibleToDouble(sendAmountEntry.Text))
                return true;
            sendStatusLabel.Text = "Invalid account number";
            return false;
        }

        private bool DestinationIsValid(UserCredit destinationAccount, UserCredit myBalance)
        {
            if (destinationAccount.AccountNumber > 0 && myBalance.AccountNumber > 0 && myBalance.AccountNumber != destinationAccount.AccountNumber)
                return true;
            sendStatusLabel.Text = "Invalid account";
            return false;
        }

        private bool CanSendCredit(double myBalance, double sendAmount, double limit)
        {
            if (sendAmount > limit)
            {
                sendStatusLabel.Text = "Request is over transfer limit";
                return false;
            }
            if (myBalance < sendAmount)
            {
                sendStatusLabel.Text = "Insufficient funds";
                return false;
            }
            return true;
        }

        private void TransferCredit(UserCredit myBalance, UserCredit destinationAccount)
        {
            App.Database.UpdateCreditDataAsync(myBalance);
            App.Database.UpdateCreditDataAsync(destinationAccount);
            sendStatusLabel.Text = "Sent";
            accountNumberEntry.Text = "";
            sendAmountEntry.Text = "";
        }

        private void SenderPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            _senderPicker = (Picker)sender;
            _senderSelectedIndex = _senderPicker.SelectedIndex;
            if (_senderSelectedIndex < 0)
                return;
            switch (_senderSelectedIndex) // picker value selection
            {
                case 0:
                    sendAmountEntry.Placeholder = "Dollars";
                    break;
                case 1:
                    sendAmountEntry.Placeholder = "Texts";
                    break;
                case 2:
                    sendAmountEntry.Placeholder = "Megabytes";
                    break;
                case 3:
                    sendAmountEntry.Placeholder = "Minutes";
                    break;
                default:
                    sendAmountEntry.Placeholder = "Dollars";
                    return;
            }
        }
    }
}