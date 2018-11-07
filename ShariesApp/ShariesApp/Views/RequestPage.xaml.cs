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
	public partial class RequestPage : ContentPage
	{
        private static Picker requestPicker;
        private static int requestSelectedIndex;
		public RequestPage ()
		{
			InitializeComponent ();
		}
        private void RequestButtonClicked(object sender, EventArgs e)
        {
            if(App.CheckIsConvertableToInt(accountNumberEntry.Text) && App.CheckIsConvertableToDouble(requestAmountEntry.Text))
            {
                //App.Database.InsertRequestDataAsync()
            }
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
    }
}