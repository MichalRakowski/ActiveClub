using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace ActiveClub.Firma.ViewModels
{
    public partial class UstawieniaKontaktoweViewModel : ObservableObject
    {
        private readonly UstawieniaKontaktoweService _service;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string telefon = string.Empty;

        [ObservableProperty]
        private string adres = string.Empty;

        [ObservableProperty]
        private string godzinyKontaktu = string.Empty;

        public UstawieniaKontaktoweViewModel(
            UstawieniaKontaktoweService service)
        {
            _service = service;
        }

        public async Task LoadAsync()
        {
            var data = await _service.GetAsync();

            if (data == null)
                return;

            Email = data.Email;
            Telefon = data.Telefon;
            Adres = data.Adres;
            GodzinyKontaktu = data.GodzinyKontaktu;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var dto = new UpdateUstawieniaKontaktoweDto
            {
                Email = Email,
                Telefon = Telefon,
                Adres = Adres,
                GodzinyKontaktu = GodzinyKontaktu
            };

            var success = await _service.UpdateAsync(dto);

            if (!success)
            {
                MessageBox.Show(
                    "Nie udało się zapisać danych kontaktowych.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            MessageBox.Show(
                "Dane kontaktowe zostały zapisane.",
                "Sukces",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}