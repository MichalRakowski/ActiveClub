using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Stanowiska;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NoweStanowiskoViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler? StanowiskoSaved;
        #endregion

        #region Fields
        private readonly StanowiskaService _stanowiskaService;
        private readonly UpdateStanowiskoDto _stanowisko;
        private readonly bool _isEditMode;
        private readonly int _stanowiskoId;
        #endregion

        #region Commands
        private BaseCommand? _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new BaseCommand(async () => await SaveAndCloseAsync());
                return _saveCommand;
            }
        }

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new BaseCommand(() => Cancel());
                return _cancelCommand;
            }
        }
        #endregion

        #region Konstruktory
        public NoweStanowiskoViewModel(StanowiskaService stanowiskaService)
        {
            DisplayName = "Nowe stanowisko";
            _stanowiskaService = stanowiskaService ?? throw new ArgumentNullException(nameof(stanowiskaService));
            _isEditMode = false;
            _stanowiskoId = 0;

            _stanowisko = new UpdateStanowiskoDto
            {
                Nazwa = string.Empty,
                Opis = string.Empty,
                CzyAktywne = true
            };
        }

        public NoweStanowiskoViewModel(StanowiskaService stanowiskaService, StanowiskoDto dto)
        {
            DisplayName = "Edycja stanowiska";
            _stanowiskaService = stanowiskaService ?? throw new ArgumentNullException(nameof(stanowiskaService));
            _isEditMode = true;
            _stanowiskoId = dto.StanowiskoId;

            _stanowisko = new UpdateStanowiskoDto
            {
                Nazwa = dto.Nazwa ?? string.Empty,
                Opis = dto.Opis,
                CzyAktywne = dto.CzyAktywne
            };
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get => _stanowisko.Nazwa ?? string.Empty;
            set
            {
                _stanowisko.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string? Opis
        {
            get => _stanowisko.Opis;
            set
            {
                _stanowisko.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public bool CzyAktywne
        {
            get => _stanowisko.CzyAktywne;
            set
            {
                _stanowisko.CzyAktywne = value;
                OnPropertyChanged(nameof(CzyAktywne));
            }
        }
        #endregion

        #region Helpers
        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa stanowiska jest wymagana.";
                return false;
            }

            if (Nazwa.Length > 80)
            {
                message = "Nazwa stanowiska może mieć maksymalnie 80 znaków.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Opis) && Opis.Length > 255)
            {
                message = "Opis może mieć maksymalnie 255 znaków.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public async Task SaveAndCloseAsync()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(msg, "Błędne dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool success;

                if (_isEditMode)
                {
                    success = await _stanowiskaService.UpdateAsync(_stanowiskoId, _stanowisko);
                }
                else
                {
                    var createDto = new CreateStanowiskoDto
                    {
                        Nazwa = _stanowisko.Nazwa,
                        Opis = _stanowisko.Opis,
                        CzyAktywne = _stanowisko.CzyAktywne
                    };

                    success = await _stanowiskaService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać stanowiska.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                StanowiskoSaved?.Invoke(this, EventArgs.Empty);
                OnRequestClose();
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
        #endregion
    }
}