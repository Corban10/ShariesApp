using System;
using System.Collections.Generic;
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
            if (App.IsConvertibleToInt(accountNumberEntry.Text) && App.IsConvertibleToDouble(requestAmountEntry.Text))
            {
                var accountDataResponse = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text));
                var creditDataResponse = App.Database.QueryCreditDataByAccountNumber(App.CurrentAccountNumber);
                var limitDataResponse = App.Database.GetSystemData("1");
                // check if valid account
                if (accountDataResponse.AccountNumber > 0 && accountDataResponse.AccountNumber != App.CurrentAccountNumber)
                {
                    RequestData newRequest = new RequestData
                    {
                        RequestSource = App.CurrentAccountNumber,
                        RequestDestination = Convert.ToInt32(accountNumberEntry.Text),
                        RequestAmount = Convert.ToDouble(requestAmountEntry.Text),
                        RequestType = ""
                    };
                    bool send = false;
                    switch (requestSelectedIndex)
                    {
                        case 0:
                            if (CheckRequestAmountValid(creditDataResponse.CreditAmount, limitDataResponse.CreditLimit))
                            {
                                newRequest.RequestType = "credit";
                                send = true;
                            }
                            break;
                        case 1:
                            if (CheckRequestAmountValid(creditDataResponse.TextAmount, limitDataResponse.TextLimit))
                            {
                                newRequest.RequestType = "text";
                                send = true;
                            }
                            break;
                        case 2:
                            if (CheckRequestAmountValid(creditDataResponse.DataAmount, limitDataResponse.DataLimit))
                            {
                                newRequest.RequestType = "data";
                                send = true;
                            }
                            break;
                        case 3:
                            if (CheckRequestAmountValid(creditDataResponse.MinutesAmount, limitDataResponse.MinutesLimit))
                            {
                                newRequest.RequestType = "minutes";
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
                if (item.Blockee == App.CurrentAccountNumber)
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
        private async void AcceptRequest(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (RequestData)button.CommandParameter;
            var position = itemList.IndexOf(item);
            var requestType = item.RequestType;

            // get requesters balance
            var requestersCredit = App.Database.QueryCreditDataByAccountNumber(item.RequestSource);
            // get requestee's balance (current user)
            var requesteeCredit = App.Database.QueryCreditDataByAccountNumber(item.RequestDestination);
            // check type
            switch (requestType) // picker value selection
            {
                case "credit":
                    if (requesteeCredit.CreditAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeCredit.CreditAmount -= item.RequestAmount;
                        requestersCredit.CreditAmount += item.RequestAmount;
                    }
                    break;
                case "text":
                    if (requesteeCredit.TextAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeCredit.TextAmount -= item.RequestAmount;
                        requestersCredit.TextAmount += item.RequestAmount;
                    }
                    break;
                case "data":
                    if (requesteeCredit.DataAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeCredit.DataAmount -= item.RequestAmount;
                        requestersCredit.DataAmount += item.RequestAmount;
                    }
                    break;
                case "minutes":
                    if (requesteeCredit.MinutesAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeCredit.MinutesAmount -= item.RequestAmount;
                        requestersCredit.MinutesAmount += item.RequestAmount;
                    }
                    break;
            }
            var confirmationResponse = await DisplayAlert("Accept Request", "Are you sure?", "Yes", "No");
            if (confirmationResponse)
            {
                // send
                App.Database.UpdateCreditData(requesteeCredit);
                App.Database.UpdateCreditData(requestersCredit);
                // delete request data row
                App.Database.DeleteRequestDataAsync(itemList[position]);
                // refresh binding context
                OnAppearing();
            }
        }

        private async void DeclineRequest(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (RequestData)button.CommandParameter;
            var position = itemList.IndexOf(item);

            var confirmationResponse = await DisplayAlert("Decline Request", "Are you sure?", "Yes", "No");
            if (confirmationResponse)
            {
                // delete request data row
                App.Database.DeleteRequestDataAsync(itemList[position]);
                // refresh binding context
                OnAppearing();
            }
        }
    }
}