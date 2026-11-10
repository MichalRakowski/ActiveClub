using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NoweUstawieniaKontaktoweViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly UstawieniaKontaktoweService _service;
        private readonly UpdateUstawieniaKontaktoweDto _item;

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(Cancel);

        public NoweUstawieniaKontaktoweViewModel(
            UstawieniaKontaktoweService service)
        {
            DisplayName = "Ustawienia kontaktowe";

            _service = service;

            _item = new UpdateUstawieniaKontaktoweDto
            {
                Email = string.Empty,
                Telefon = string.Empty,
                Adres = string.Empty,
                GodzinyKontaktu = string.Empty
            };

            _ = LoadAsync();
        }

        public string Email
        {
            get => _item.Email;
            set
            {
                _item.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Telefon
        {
            get => _item.Telefon;
            set
            {
                _item.Telefon = value;
                OnPropertyChanged(nameof(Telefon));
            }
        }

        public string Adres
        {
            get => _item.Adres;
            set
            {
                _item.Adres = value;
                OnPropertyChanged(nameof(Adres));
            }
        }

        public string GodzinyKontaktu
        {
            get => _item.GodzinyKontaktu;
            set
            {
                _item.GodzinyKontaktu = value;
                OnPropertyChanged(nameof(GodzinyKontaktu));
            }
        }

        private async Task LoadAsync()
        {
            try
            {
                var data = await _service.GetAsync();

                if (data == null)
                    return;

                Email = data.Email;
                Telefon = data.Telefon;
                Adres = data.Adres;
                GodzinyKontaktu = data.GodzinyKontaktu;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd ładowania danych kontaktowych",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                message = "Email jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Telefon))
            {
                message = "Telefon jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adres))
            {
                message = "Adres jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(GodzinyKontaktu))
            {
                message = "Godziny kontaktu są wymagane.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public async Task SaveAndClose()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(
                    msg,
                    "Brak danych",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            try
            {
                var success = await _service.UpdateAsync(_item);

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

                Saved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd zapisu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            OnRequestClose();
        }
    }
}