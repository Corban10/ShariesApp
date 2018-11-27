using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminPage : ContentPage
    {
        private static Picker creditLimitPicker;
        private static int clSelectedIndex;
        
        public AdminPage ()
		{
			InitializeComponent ();
		}
        private void SetLimitSelectedIndexChanged(object sender, EventArgs e)
        {
            creditLimitPicker = (Picker)sender;
            clSelectedIndex = creditLimitPicker.SelectedIndex;
        }
        private async void SetNewCreditLimit(object sender, EventArgs e)
        {
            var confirmationResponse = await DisplayAlert("Set new limit", "Are you sure?", "Yes", "No");
            if (confirmationResponse)
            {
                if (clSelectedIndex >= 0) // if picker selection valid
                {
                    var limits = App.Database.GetSystemData("1"); // get system data
                    if (App.IsConvertibleToDouble(setLimitEntry.Text)) // check if valid value
                    {
                        switch (clSelectedIndex) // picker value selection
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
                        App.Database.UpdateSystemDataAsync(limits); //update row
                        nameLabel.Text = string.Format("Credit limit: {0}\nText limit: {1}\nData limit: {2}\nMinutes limit: {3}\n",
                            limits.CreditLimit,
                            limits.TextLimit,
                            limits.DataLimit,
                            limits.MinutesLimit
                            ); //display new value(s) to label  
                    }
                    else
                        nameLabel.Text = "Invalid value";
                }
            }
            setLimitEntry.Text = "";
        }
        private async void ChangeAccountNumber(object sender, EventArgs e) //BUG: user credit updates but not user data sometimes
        {
            var confirmationResponse = await DisplayAlert("Change account number", "Are you sure?", "Yes", "No");
            if (confirmationResponse)
            {
                if (App.IsConvertibleToInt(oldAccountNUmber.Text) && App.IsConvertibleToInt(newAccountNUmber.Text)) //check if entry text is valid number
                {
                    var getUserData = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text)); //get this users details
                    var getCreditData = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text)); //get this users credit details

                    bool doesAccountNumberAlreadyExist = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(newAccountNUmber.Text)).AccountNumber == 0; //check if already exists
                    bool oldAccountNumberIsNotAdmin = getUserData.AccountNumber > 20 && getCreditData.AccountNumber > 20; // check if not admin
                    bool newAccountNumberIsNotAdmin = Convert.ToInt32(newAccountNUmber.Text) > 20; // check if destination account number is not admin

                    if (oldAccountNumberIsNotAdmin && newAccountNumberIsNotAdmin && doesAccountNumberAlreadyExist)
                    {
                        getUserData.AccountNumber = Convert.ToInt32(newAccountNUmber.Text); // change account number
                        getCreditData.AccountNumber = Convert.ToInt32(newAccountNUmber.Text); // change account number

                        App.Database.UpdateUserDataAsync(getUserData);
                        App.Database.UpdateCreditDataAsync(getCreditData);
                        nameLabelTwo.Text = "Account number changed successfully";
                    }
                    else
                        nameLabelTwo.Text = "Error";
                }
                else
                    nameLabelTwo.Text = "Invalid account number";
            }
            else
                nameLabelTwo.Text = "Account number not changed";
            oldAccountNUmber.Text = "";
            newAccountNUmber.Text = "";
        }
    }
}