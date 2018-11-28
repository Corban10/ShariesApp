using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestPage : ContentPage
    {
        private static Picker _requestPicker;
        private static int _requestSelectedIndex;
        private static List<RequestData> _itemList;

        public RequestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _itemList = App.Database.QueryRequestDataByDestination(App.CurrentAccountNumber);
            this.BindingContext = _itemList;
            base.OnAppearing();
        }

        private async void RequestCredit(object sender, EventArgs e)
        {
            if (!EntryTextIsValid())
                return;
            var accountDataResponse = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(accountNumberEntry.Text));
            var creditDataResponse = App.Database.QueryCreditDataByAccountNumber(App.CurrentAccountNumber);
            var limitDataResponse = App.Database.GetSystemData("1");
            if (!AccountNumberIsValid(accountDataResponse))
                return;
            var newRequest = GetRequestData();
            switch (_requestSelectedIndex)
            {
                case 0:
                    if (!RequestAmountValid(creditDataResponse.CreditAmount, limitDataResponse.CreditLimit))
                        return;
                    newRequest.RequestType = "credit";
                    break;
                case 1:
                    if (!RequestAmountValid(creditDataResponse.TextAmount, limitDataResponse.TextLimit))
                        return;
                    newRequest.RequestType = "text";
                    break;
                case 2:
                    if (!RequestAmountValid(creditDataResponse.DataAmount, limitDataResponse.DataLimit))
                        return;
                    newRequest.RequestType = "data";
                    break;
                case 3:
                    if (!RequestAmountValid(creditDataResponse.MinutesAmount, limitDataResponse.MinutesLimit))
                        return;
                    newRequest.RequestType = "minutes";
                    break;
                default:
                    return;
            }
            if (!await ConfirmationResponse("Request?"))
            {
                requestStatusLabel.Text = "Request not sent";
                return;
            }
            App.Database.InsertRequestDataAsync(newRequest);
            requestStatusLabel.Text = "Request sent";
        }

        private bool EntryTextIsValid()
        {
            if (!App.IsConvertibleToInt(accountNumberEntry.Text))
            {
                requestStatusLabel.Text = "Invalid Account";
                return false;
            }
            if (!App.IsConvertibleToDouble(requestAmountEntry.Text))
            {
                requestStatusLabel.Text = "Invalid Amount";
                return false;
            }
            return true;
        }

        private static bool AccountNumberIsValid(UserData accountDataResponse)
        {
            return accountDataResponse.AccountNumber > 0 && accountDataResponse.AccountNumber != App.CurrentAccountNumber;
        }

        private RequestData GetRequestData()
        {
            RequestData newRequest = new RequestData
            {
                RequestSource = App.CurrentAccountNumber,
                RequestDestination = Convert.ToInt32(accountNumberEntry.Text),
                RequestAmount = Convert.ToDouble(requestAmountEntry.Text),
                RequestType = ""
            };
            return newRequest;
        }

        private async Task<bool> ConfirmationResponse(string message)
        {
            return await DisplayAlert(message, "Are you sure?", "Yes", "No");
        }

        private bool RequestAmountValid(double balance, double limit)
        {
            if (!RequestIsUnderLimit(limit))
            {
                requestStatusLabel.Text = "Request is over transfer limit";
                return false;
            }
            if (!SufficientFunds(balance))
            {
                requestStatusLabel.Text = "Insufficient funds";
                return false;
            }
            if (AccountIsBlocked(accountNumberEntry.Text))
            {
                requestStatusLabel.Text = "That user is blocking you";
                return false;
            }
            return true;
        }

        private bool SufficientFunds(double balance)
        {
            return Convert.ToInt32(requestAmountEntry.Text) < balance;
        }

        private bool RequestIsUnderLimit(double limit)
        {
            return Convert.ToInt32(requestAmountEntry.Text) < limit;
        }

        private static bool AccountIsBlocked(string blocker)
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
            _requestPicker = (Picker)sender;
            _requestSelectedIndex = _requestPicker.SelectedIndex;
            if (_requestSelectedIndex >= 0) // if picker selection valid
            {
                switch (_requestSelectedIndex) // picker value selection
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
            var position = _itemList.IndexOf(item);
            var requestType = item.RequestType;

            var requesterBalance = App.Database.QueryCreditDataByAccountNumber(item.RequestSource);
            var requesteeBalance = App.Database.QueryCreditDataByAccountNumber(item.RequestDestination);
            switch (requestType) // picker value selection
            {
                case "credit":
                    if (requesteeBalance.CreditAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeBalance.CreditAmount -= item.RequestAmount;
                        requesterBalance.CreditAmount += item.RequestAmount;
                    }
                    break;
                case "text":
                    if (requesteeBalance.TextAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeBalance.TextAmount -= item.RequestAmount;
                        requesterBalance.TextAmount += item.RequestAmount;
                    }
                    break;
                case "data":
                    if (requesteeBalance.DataAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeBalance.DataAmount -= item.RequestAmount;
                        requesterBalance.DataAmount += item.RequestAmount;
                    }
                    break;
                case "minutes":
                    if (requesteeBalance.MinutesAmount > item.RequestAmount) // check if we have enough
                    {
                        requesteeBalance.MinutesAmount -= item.RequestAmount;
                        requesterBalance.MinutesAmount += item.RequestAmount;
                    }
                    break;
            }
            if (await ConfirmationResponse("Accept Request?"))
            {
                TransferCredit(requesteeBalance, requesterBalance, position);
                OnAppearing();
            }
        }

        private static void TransferCredit(UserCredit requesteeCredit, UserCredit requesterCredit, int position)
        {
            App.Database.UpdateCreditData(requesteeCredit);
            App.Database.UpdateCreditData(requesterCredit);
            App.Database.DeleteRequestDataAsync(_itemList[position]);
        }

        private async void DeclineRequest(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (RequestData)button.CommandParameter;
            var position = _itemList.IndexOf(item);
            if (await ConfirmationResponse("Decline Request?"))
            {
                App.Database.DeleteRequestDataAsync(_itemList[position]);
                OnAppearing();
            }
        }
    }
}