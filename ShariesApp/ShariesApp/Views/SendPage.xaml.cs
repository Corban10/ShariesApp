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
	public partial class SendPage : ContentPage
    {
        private static Picker senderPicker;
        private static int senderSelectedIndex;
        public SendPage ()
		{
			InitializeComponent ();
        }
        private void SendButtonClicked(object sender, EventArgs e)
        {
            sendStatusLabel.Text = "";
            if (App.checkIsConvertableToInt(accountNumberEntry.Text) && App.checkIsConvertableToDouble(sendAmountEntry.Text)) // check if entries are valid numbers
            {
                var myBalance = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(App.loggedInUser.accountNumber)); //query my balance
                var destinationAccount = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text)); //query destination account
                double amount = Convert.ToDouble(sendAmountEntry.Text); //convert entry to double
                bool send = false;
                if (destinationAccount.accountNumber > 0 && myBalance.accountNumber > 0)
                {
                    switch (senderSelectedIndex) // get balance based on 
                    {
                        case 0:
                            if (myBalance.creditAmount > amount && amount < App.limits.creditLimit)
                            {
                                myBalance.creditAmount -= amount;
                                destinationAccount.creditAmount += amount;
                                send = true;
                            }
                            break;
                        case 1:
                            if (myBalance.textAmount > amount && amount < App.limits.textLimit)
                            {
                                myBalance.textAmount -= amount;
                                destinationAccount.textAmount += amount;
                                send = true;
                            }
                            break;
                        case 2:
                            if (myBalance.dataAmount > amount && amount < App.limits.dataLimit)
                            {
                                myBalance.dataAmount -= amount;
                                destinationAccount.dataAmount += amount;
                                send = true;
                            }
                            break;
                        case 3:
                            if (myBalance.minutesAmount > amount && amount < App.limits.minutesLimit)
                            {
                                myBalance.minutesAmount -= amount;
                                destinationAccount.minutesAmount += amount;
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
                    else
                        sendStatusLabel.Text = "Cannot send that much";
                }
                else
                    sendStatusLabel.Text = "Account does not exist";
            }
            else
                sendStatusLabel.Text = "Invalid account number";
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