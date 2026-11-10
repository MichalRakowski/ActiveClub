using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceProvider _serviceProvider;

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _haslo = string.Empty;
        public string Haslo
        {
            get => _haslo;
            set
            {
                _haslo = value;
                OnPropertyChanged(nameof(Haslo));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(
            AuthService authService,
            ICurrentUserService currentUserService,
            IServiceProvider serviceProvider)
        {
            _authService = authService;
            _currentUserService = currentUserService;
            _serviceProvider = serviceProvider;

            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Haslo))
            {
                ErrorMessage = "Podaj login i hasło.";
                return;
            }

            try
            {
                var result = await _authService.LoginAsync(Login, Haslo);

                if (result == null || !result.Success)
                {
                    ErrorMessage = result?.ErrorMessage ?? "Nie udało się zalogować.";
                    return;
                }
                
                _currentUserService.SetCurrentUser(
     result.UserId,
     result.UserName,
     $"{result.Imie} {result.Nazwisko}",
     result.Stanowisko,
     result.Rola
 );

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                mainWindow.Show();

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is LoginWindow)
                    ?.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd logowania: {ex.Message}";
            }
        }
    }
}