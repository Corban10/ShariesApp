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

        private static Picker cDetailsPicker;
        private static int cDetailsSelectedIndex;

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
            if (clSelectedIndex >= 0)
            {
                var newSystemData = App.Database.GetSystemData("1");
                switch (clSelectedIndex)
                {
                    case 0:
                        newSystemData.creditLimit = checkIsConvertableToDouble(setLimitEntry.Text) ? 
                            Convert.ToDouble(setLimitEntry.Text) : 
                            newSystemData.creditLimit;
                        break;
                    case 1:
                        newSystemData.textLimit = checkIsConvertableToDouble(setLimitEntry.Text) ?
                            Convert.ToDouble(setLimitEntry.Text) :
                            newSystemData.textLimit;
                        break;
                    case 2:
                        newSystemData.dataLimit = checkIsConvertableToDouble(setLimitEntry.Text) ?
                            Convert.ToDouble(setLimitEntry.Text) :
                            newSystemData.dataLimit;
                        break;
                    case 3:
                        newSystemData.minutesLimit = checkIsConvertableToDouble(setLimitEntry.Text) ?
                            Convert.ToDouble(setLimitEntry.Text) :
                            newSystemData.minutesLimit;
                        break;
                }
                App.Database.UpdateSystemDataAsync(newSystemData);
                nameLabel.Text = string.Format("Credit limit: {0}\nText limit: {1}\nData limit: {2}\nMinutes limit: {3}\n",
                    newSystemData.creditLimit,
                    newSystemData.textLimit,
                    newSystemData.dataLimit,
                    newSystemData.minutesLimit
                    );
                setLimitEntry.Text = "";
            }
        }
        private bool checkIsConvertableToDouble(string input)
        {
            return (!string.IsNullOrWhiteSpace(input) && Double.TryParse(input, out double e));
        }
        private void cDetailsSelectedIndexChanged(object sender, EventArgs e)
        {
            cDetailsPicker = (Picker)sender;
            cDetailsSelectedIndex = cDetailsPicker.SelectedIndex;
        }
        private void changeDetailsButtonClicked(object sender, EventArgs e)
        {
            if (clSelectedIndex >= 0)
            {
                // check exists

                // change to new
                // nameLabelTwo.Text = string.Format("{0} {1}, {2}", cDetailsPicker.Items[cDetailsSelectedIndex], oldDetails.Text, newDetails.Text);
            }
        }
    }
}