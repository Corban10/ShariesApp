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