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
	public partial class AdminPage : ContentPage
    {
        private static Picker creditLimitPicker;
        private static int clSelectedIndex;
        
        public AdminPage ()
		{
			InitializeComponent ();
		}
        private void setLimitSelectedIndexChanged(object sender, EventArgs e)
        {
            creditLimitPicker = (Picker)sender;
            clSelectedIndex = creditLimitPicker.SelectedIndex;
        }
        private void setLimitButtonClicked(object sender, EventArgs e)
        {
            if (clSelectedIndex >= 0) // if picker selection valid
            {
                var limits = App.Database.GetSystemData("1"); // get system data
                if (App.CheckIsConvertableToDouble(setLimitEntry.Text)) // check if valid value
                {
                    switch (clSelectedIndex) // picker value selection
                    {
                        case 0:
                            limits.creditLimit = Convert.ToDouble(setLimitEntry.Text); 
                            break;
                        case 1:
                            limits.textLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                        case 2:
                            limits.dataLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                        case 3:
                            limits.minutesLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                    }
                    App.Database.UpdateSystemDataAsync(limits); //update row
                    nameLabel.Text = string.Format("Credit limit: {0}\nText limit: {1}\nData limit: {2}\nMinutes limit: {3}\n",
                        limits.creditLimit,
                        limits.textLimit,
                        limits.dataLimit,
                        limits.minutesLimit
                        ); //display new value(s) to label
                }
                else
                {
                    nameLabel.Text = "Invalid value";
                }
            }
            setLimitEntry.Text = "";
        }
        private void changeDetailsButtonClicked(object sender, EventArgs e) //BUG: user credit updates but not user data sometimes
        {
            if (App.CheckIsConvertableToInt(oldAccountNUmber.Text) && App.CheckIsConvertableToInt(newAccountNUmber.Text)) //check if entry text is valid number
            {
                var getUserData = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text)); //get this users details
                var getCreditData = App.Database.QueryCreditDataByAccountNumber(Convert.ToInt32(oldAccountNUmber.Text)); //get this users credit details
                bool doesAccountNumberAlreadyExist = App.Database.QueryUserDataByAccountNumber(Convert.ToInt32(newAccountNUmber.Text)).accountNumber == 0; //check if already exists
                bool oldAccountNumberIsNotAdmin = getUserData.accountNumber > 20 && getCreditData.accountNumber > 20; // check if not admin
                bool newAccountNumberIsNotAdmin = Convert.ToInt32(newAccountNUmber.Text) > 20; // check if destination account number is not admin
                if (oldAccountNumberIsNotAdmin && newAccountNumberIsNotAdmin && doesAccountNumberAlreadyExist)
                {
                    getUserData.accountNumber = Convert.ToInt32(newAccountNUmber.Text); // change account number
                    getCreditData.accountNumber = Convert.ToInt32(newAccountNUmber.Text); // change account number

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
    }
}