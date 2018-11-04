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
                var newSystemData = App.Database.GetSystemData("1"); // get system data
                if (checkIsConvertableToDouble(setLimitEntry.Text)) // check if valid value
                {
                    switch (clSelectedIndex) // picker value selection
                    {
                        case 0:
                            newSystemData.creditLimit = Convert.ToDouble(setLimitEntry.Text); 
                            break;
                        case 1:
                            newSystemData.textLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                        case 2:
                            newSystemData.dataLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                        case 3:
                            newSystemData.minutesLimit = Convert.ToDouble(setLimitEntry.Text);
                            break;
                    }
                    App.Database.UpdateSystemDataAsync(newSystemData); //update row
                    nameLabel.Text = string.Format("Credit limit: {0}\nText limit: {1}\nData limit: {2}\nMinutes limit: {3}\n",
                        newSystemData.creditLimit,
                        newSystemData.textLimit,
                        newSystemData.dataLimit,
                        newSystemData.minutesLimit
                        ); //display new value(s) to label
                }
                else
                {
                    nameLabel.Text = "Invalid value";
                }
            }
            setLimitEntry.Text = "";
        }
        private bool checkIsConvertableToDouble(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Double.TryParse(input, out double e));
        }
        private bool checkIsConvertableToInt(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Int32.TryParse(input, out int e));
        }
        private void changeDetailsButtonClicked(object sender, EventArgs e)
        {
            if (checkIsConvertableToInt(oldAccountNUmber.Text)) //check if account number is number
            {
                var getUserData = App.Database.QueryUserDataById(oldAccountNUmber.Text); //get this users details
                if (!string.IsNullOrWhiteSpace(getUserData.accountNumber)) //check if returned value is not blank object
                {
                    if (checkIsConvertableToInt(newAccountNUmber.Text)) // check if new account number is valid
                    {
                        App.Database.DeleteUserDataAsync(getUserData); //delete old row
                        getUserData.accountNumber = newAccountNUmber.Text; // change account number
                        App.Database.InsertUserDataAsync(getUserData); //insert new row
                    }
                    //display user values for confirmation
                    nameLabelTwo.Text = "Account number changed successfully";
                }
                else
                {

                    nameLabelTwo.Text = "Error";
                }
            }
            else
            {
                nameLabelTwo.Text = "Invalid account number";
            }
        }
    }
}