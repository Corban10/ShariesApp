using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        private static Picker _creditLimitPicker;
        private static int _clSelectedIndex;

        public AdminPage()
        {
            InitializeComponent();
        }

        private void SetLimitSelectedIndexChanged(object sender, EventArgs e)
        {
            _creditLimitPicker = (Picker)sender;
            _clSelectedIndex = _creditLimitPicker.SelectedIndex;
        }

        private async void SetNewCreditLimit(object sender, EventArgs e)
        {
            if (!await ConfirmationResponse("Set new limit?") || _clSelectedIndex < 0)
                return;
            if (!App.IsConvertibleToDouble(setLimitEntry.Text))
            {
                nameLabel.Text = "Invalid value";
                return;
            }
            var limits = SetNewLimit();
            App.Database.UpdateSystemDataAsync(limits);
            DisplayCreditLimit(limits);
            setLimitEntry.Text = "";
        }

        private async Task<bool> ConfirmationResponse(string message)
        {
            return await DisplayAlert(message, "Are you sure?", "Yes", "No");
        }

        private SystemData SetNewLimit()
        {
            var limits = App.Database.GetSystemData("1");
            switch (_clSelectedIndex)
            {
                case 0:
                    limits.CreditLimit = Convert.ToDouble(setLimitEntry.Text);
                    break;
                case 1:
                    limits.TextLimit = Convert.ToDouble(setLimitEntry.Text);
                    break;
                case 2:
                    limits.DataLimit = Convert.ToDouble(setLimitEntry.Text);
                    break;
                case 3:
                    limits.MinutesLimit = Convert.ToDouble(setLimitEntry.Text);
                    break;
            }
            return limits;
        }

        private void DisplayCreditLimit(SystemData limits)
        {
            nameLabel.Text = string.Format("Credit limit: {0}\nText limit: {1}\nData limit: {2}\nMinutes limit: {3}\n",
                limits.CreditLimit,
                limits.TextLimit,
                limits.DataLimit,
                limits.MinutesLimit
            );
        }

        private async void ChangeAccountNumber(object sender, EventArgs e)
        {
            if (!await ConfirmationResponse("Change account number?"))
                return;
            var userData = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text));
            var userCredit = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text));
            if (!AccountNumberCanChange(userData, userCredit))
            {
                nameLabelTwo.Text = "Change unsuccessful";
                return;
            }
            UpdateAccountNumber(userData, userCredit);
            nameLabelTwo.Text = "Change successful";
            oldAccountNUmber.Text = "";
            newAccountNUmber.Text = "";
        }

        private bool AccountNumberCanChange(UserData userData, UserCredit userCredit)
        {
            if (!App.IsConvertibleToInt(oldAccountNUmber.Text) || !App.IsConvertibleToInt(newAccountNUmber.Text))
                return false;
            bool accountNumberExists = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(newAccountNUmber.Text)).AccountNumber == 0; //check if already exists
            bool oldAccNumIsNotAdmin = userData.AccountNumber > 20 && userCredit.AccountNumber > 20; // check if not admin
            bool newAccNumIsNotAdmin = Convert.ToInt32(newAccountNUmber.Text) > 20; // check if destination account number is not admin
            return (oldAccNumIsNotAdmin && newAccNumIsNotAdmin && accountNumberExists);
        }

        private void UpdateAccountNumber(UserData userData, UserCredit userCredit)
        {
            userData.AccountNumber = Convert.ToInt32(newAccountNUmber.Text);
            userCredit.AccountNumber = Convert.ToInt32(newAccountNUmber.Text);
            App.Database.UpdateUserDataAsync(userData);
            App.Database.UpdateCreditDataAsync(userCredit);
        }
    }
}