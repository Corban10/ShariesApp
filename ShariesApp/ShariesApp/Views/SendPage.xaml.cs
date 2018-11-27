using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendPage : ContentPage
    {
        private static Picker senderPicker;
        private static int senderSelectedIndex;
        public SendPage ()
		{
			InitializeComponent ();
        }
        private async void SendCredit(object sender, EventArgs e)
        {
            sendStatusLabel.Text = "";
            var confirmationResponse = await DisplayAlert("Send", "Are you sure?", "Yes", "No");
            if (confirmationResponse)
            {
                if (App.IsConvertibleToInt(accountNumberEntry.Text) && App.IsConvertibleToDouble(sendAmountEntry.Text)) // check if entries are valid numbers
                {
                    var myBalance = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(App.CurrentAccountNumber)); //query my balance
                    var destinationAccount = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text)); //query destination account
                    var limits = App.Database.GetSystemData("1");
                    double amount = Convert.ToDouble(sendAmountEntry.Text); //convert entry to double
                    bool send = false;
                    if (destinationAccount.AccountNumber > 0 && myBalance.AccountNumber > 0 && myBalance.AccountNumber != destinationAccount.AccountNumber)
                    {
                        switch (senderSelectedIndex) // get balance based on 
                        {
                            case 0:
                                if (myBalance.CreditAmount > amount && amount < limits.CreditLimit)
                                {
                                    myBalance.CreditAmount -= amount;
                                    destinationAccount.CreditAmount += amount;
                                    send = true;
                                }
                                break;
                            case 1:
                                if (myBalance.TextAmount > amount && amount < limits.TextLimit)
                                {
                                    myBalance.TextAmount -= amount;
                                    destinationAccount.TextAmount += amount;
                                    send = true;
                                }
                                break;
                            case 2:
                                if (myBalance.DataAmount > amount && amount < limits.DataLimit)
                                {
                                    myBalance.DataAmount -= amount;
                                    destinationAccount.DataAmount += amount;
                                    send = true;
                                }
                                break;
                            case 3:
                                if (myBalance.MinutesAmount > amount && amount < limits.MinutesLimit)
                                {
                                    myBalance.MinutesAmount -= amount;
                                    destinationAccount.MinutesAmount += amount;
                                    send = true;
                                }
                                break;
                        }
                        if (send)
                        {
                            App.Database.UpdateCreditDataAsync(myBalance);
                            App.Database.UpdateCreditDataAsync(destinationAccount);
                            sendStatusLabel.Text = "Sent";
                        }
                    }
                    else
                        sendStatusLabel.Text = "Invalid account";
                }
                else
                    sendStatusLabel.Text = "Invalid account number";
            }
            accountNumberEntry.Text = "";
            sendAmountEntry.Text = "";
        }
        private void SenderPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            senderPicker = (Picker)sender;
            senderSelectedIndex = senderPicker.SelectedIndex;
            if (senderSelectedIndex >= 0) // if picker selection valid
            {
                switch (senderSelectedIndex) // picker value selection
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
                }
            }
        }
    }
}