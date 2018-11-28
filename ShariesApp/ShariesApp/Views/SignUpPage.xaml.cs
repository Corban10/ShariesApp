using ShariesApp.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShariesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {

        public SignUpPage()
        {
            InitializeComponent();
        }
        private async void SignUp(object sender, EventArgs e)
        {
            if (!App.IsConvertibleToInt(usernameEntry.Text))
            {
                messageLabel.Text = "Invalid Account Number";
                return;
            }
            var user = GetUserData();
            var userCredit = GetUserCredit();
            if (!AreDetailsValid(user))
            {
                messageLabel.Text = "Sign up failed";
                return;
            }
            InsertNewUser(user, userCredit);
            await GoToMainPage();
        }

        private UserData GetUserData()
        {
            return new UserData
            {
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                Password = passwordEntry.Text,
                Name = nameEntry.Text
            };
        }

        private UserCredit GetUserCredit()
        {
            return new UserCredit
            {
                AccountNumber = Convert.ToInt32(usernameEntry.Text),
                CreditAmount = 1000,
                TextAmount = 1000,
                DataAmount = 1000,
                MinutesAmount = 1000
            };
        }

        private bool AreDetailsValid(UserData user)
        {
            var emptyAccount = App.Database.QueryUserDataByAccountNumber(user.AccountNumber);
            if (!string.IsNullOrWhiteSpace(emptyAccount.UserId))
                return false;
            return user.AccountNumber > 0 && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Name);
        }

        private static void InsertNewUser(UserData user, UserCredit userCredit)
        {
            App.Database.InsertUserDataAsync(user);
            App.Database.InsertCreditDataAsync(userCredit);
            App.CurrentAccountNumber = user.AccountNumber;
            App.IsUserLoggedIn = true;
        }

        private async Task GoToMainPage()
        {
            var rootPage = Navigation.NavigationStack.FirstOrDefault();
            if (rootPage != null)
            {
                Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                await Navigation.PopToRootAsync();
            }
        }
    }
}