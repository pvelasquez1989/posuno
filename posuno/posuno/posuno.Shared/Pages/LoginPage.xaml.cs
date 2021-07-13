using posuno.Components;
using posuno.Helpers;
using posuno.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace posuno.Pages
{
   
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            EmailTextBox.Text = "pedro@yopmail.com";
            PasswordPasswordBox.Password = "1234567";
        }



        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidForm();
            if (!isValid)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.LoginAsync(new LoginRequest
            {
                Email = EmailTextBox.Text,
                Password = PasswordPasswordBox.Password
            });
            loader.Close();

            MessageDialog messageDialog;
            if (!response.IsSuccess)
            {
                messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            User user = (User)response.Result;
            if (user == null)
            {
                messageDialog = new MessageDialog("Usuario o contraseña incorrectos", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Frame.Navigate(typeof(MainPage), user);


        }
        private async Task<bool> ValidForm()
        {
            MessageDialog messageDialog;

            if(string.IsNullOrEmpty(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar tu email", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email valido.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (PasswordPasswordBox.Password.Length < 7 )
            {
                messageDialog = new MessageDialog("Debes ingresar tu contraseña de al menos siete (7) carácteres. ", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }
    }
}
