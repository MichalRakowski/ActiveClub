using ActiveClub.Firma.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ActiveClub.Firma.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Haslo = passwordBox.Password;
            }
        }
    }
}