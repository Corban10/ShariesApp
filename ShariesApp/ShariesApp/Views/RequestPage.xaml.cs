using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestPage : ContentPage
	{
        private static Picker requestPicker;
        private static int requestSelectedIndex;
        private static string currentId; // might need this when deleting requests
        private static List<RequestData> itemList;

        public RequestPage ()
		{
			InitializeComponent();
        }
        protected override void OnAppearing()
        {
            itemList = App.Database.QueryRequestDataByDestination(App.CurrentAccountNumber);
            this.BindingContext = itemList;
            base.OnAppearing();
        }
        private async void RequestCredit(object sender, EventArgs e)
        {
            requestStatusLabel.Text = "";
            // check if entry text are valid numbers
            if (App.CheckIsConvertableToInt(accountNumberEntry.Text) && App.CheckIsConvertableToDouble(requestAmountEntry.Text))
            {
                var accountDataResponse = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text));
                var creditDataResponse = App.Database.QueryCreditDataByAccountNumber(App.CurrentAccountNumber);
                var limitDataResponse = App.Database.GetSystemData("1");
                // check if valid account
                if (accountDataResponse.accountNumber > 0 && accountDataResponse.accountNumber != App.CurrentAccountNumber)
                {
                    RequestData newRequest = new RequestData
                    {
                        requestSource = App.CurrentAccountNumber,
                        requestDestination = Convert.ToInt32(accountNumberEntry.Text),
                        requestAmount = Convert.ToDouble(requestAmountEntry.Text),
                        notificationStatus = false,
                        requestType = ""
                    };
                    bool send = false;
                    switch (requestSelectedIndex)
                    {
                        case 0:
                            if (CheckRequestAmountValid(creditDataResponse.creditAmount, limitDataResponse.creditLimit))
                            {
                                newRequest.requestType = "credit";
                                send = true;
                            }
                            break;
                        case 1:
                            if (CheckRequestAmountValid(creditDataResponse.textAmount, limitDataResponse.textLimit))
                            {
                                newRequest.requestType = "text";
                                send = true;
                            }
                            break;
                        case 2:
                            if (CheckRequestAmountValid(creditDataResponse.dataAmount, limitDataResponse.dataLimit))
                            {
                                newRequest.requestType = "data";
                                send = true;
                            }
                            break;
                        case 3:
                            if (CheckRequestAmountValid(creditDataResponse.minutesAmount, limitDataResponse.minutesLimit))
                            {
                                newRequest.requestType = "minutes";
                                send = true;
                            }
                            break;
                    }
                    var confirmationResponse = await DisplayAlert("Request", "Are you sure?", "Yes", "No");
                    if (confirmationResponse && send)
                    {
                        App.Database.InsertRequestDataAsync(newRequest);
                        requestStatusLabel.Text = "Request sent";
                    }
                }
                else
                    requestStatusLabel.Text = "Invalid Account";
            }
        }
        private bool CheckRequestAmountValid(double balance, double limit)
        {
            // check if amount < than balance
            if (Convert.ToInt32(requestAmountEntry.Text) < balance)
            {
                // check if amount < than limit
                if (Convert.ToInt32(requestAmountEntry.Text) < limit)
                {
                    // check if blocked
                    if (!CheckIfAccountIsBlocked(accountNumberEntry.Text))
                    {
                        return true;
                    }
                    else
                        requestStatusLabel.Text = "That user is blocking you";
                }
                else
                    requestStatusLabel.Text = "Request is over transfer limit";
            }
            else
                requestStatusLabel.Text = "Insufficient funds";
            return false;
        }
        private bool CheckIfAccountIsBlocked(string blocker)
        {
            var blockedAccountsList = App.Database.QueryBlockedAccountsByBlocker(Convert.ToInt32(blocker));
            foreach (var item in blockedAccountsList)
            {
                if (item.blockee == App.CurrentAccountNumber)
                {
                    return true;
                }
            }
            return false;
        }
        private void RequestPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            requestPicker = (Picker)sender;
            requestSelectedIndex = requestPicker.SelectedIndex;
            if (requestSelectedIndex >= 0) // if picker selection valid
            {
                switch (requestSelectedIndex) // picker value selection
                {
                    case 0:
                        requestAmountEntry.Placeholder = "Dollars";
                        break;
                    case 1:
                        requestAmountEntry.Placeholder = "Texts";
                        break;
                    case 2:
                        requestAmountEntry.Placeholder = "Megabytes";
                        break;
                    case 3:
                        requestAmountEntry.Placeholder = "Minutes";
                        break;
                }
            }
        }
        private void AcceptRequest(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (RequestData)button.CommandParameter;
            var position = itemList.IndexOf(item);

            // Debug.WriteLine(position);
        }

        private void DeclineRequest(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (RequestData)button.CommandParameter;
            var position = itemList.IndexOf(item);

            // Debug.WriteLine(position);
        }
    }
}