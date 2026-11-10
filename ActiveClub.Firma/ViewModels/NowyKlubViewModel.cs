using ActiveClub.Firma.Helper;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Firma.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyKlubViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler? KlubSaved;
        #endregion

        #region Fields
        private readonly KlubyService _klubyService;
        private readonly UpdateKlubDto _klub;
        private readonly bool _isEditMode;
        private readonly int _klubId;
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
        public NowyKlubViewModel(KlubyService klubyService)
        {
            DisplayName = "Nowy klub";
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _isEditMode = false;
            _klubId = 0;

            _klub = new UpdateKlubDto
            {
                KlubId = 0,
                Nazwa = string.Empty,
                Miasto = string.Empty,
                Ulica = string.Empty,
                KodPocztowy = string.Empty,
                Telefon = string.Empty,
                EmailKontakt = string.Empty,
                Opis = string.Empty,
                CzyAktywny = true,
                DataUtworzenia = DateTime.Now
            };
        }

        public NowyKlubViewModel(KlubyService klubyService, KlubDto dto)
        {
            DisplayName = "Edycja klubu";
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _isEditMode = true;
            _klubId = dto.KlubId;

            _klub = new UpdateKlubDto
            {
                KlubId = dto.KlubId,
                Nazwa = dto.Nazwa ?? string.Empty,
                Miasto = dto.Miasto ?? string.Empty,
                Ulica = dto.Ulica ?? string.Empty,
                KodPocztowy = dto.KodPocztowy ?? string.Empty,
                Telefon = dto.Telefon ?? string.Empty,
                EmailKontakt = dto.EmailKontakt ?? string.Empty,
                Opis = dto.Opis ?? string.Empty,
                CzyAktywny = dto.CzyAktywny,
                DataUtworzenia = dto.DataUtworzenia
            };
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get => _klub.Nazwa ?? string.Empty;
            set
            {
                _klub.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string Miasto
        {
            get => _klub.Miasto ?? string.Empty;
            set
            {
                _klub.Miasto = value;
                OnPropertyChanged(nameof(Miasto));
            }
        }

        public string Ulica
        {
            get => _klub.Ulica ?? string.Empty;
            set
            {
                _klub.Ulica = value;
                OnPropertyChanged(nameof(Ulica));
            }
        }

        public string KodPocztowy
        {
            get => _klub.KodPocztowy ?? string.Empty;
            set
            {
                _klub.KodPocztowy = value;
                OnPropertyChanged(nameof(KodPocztowy));
            }
        }

        public string Telefon
        {
            get => _klub.Telefon ?? string.Empty;
            set
            {
                _klub.Telefon = value;
                OnPropertyChanged(nameof(Telefon));
            }
        }

        public string EmailKontakt
        {
            get => _klub.EmailKontakt ?? string.Empty;
            set
            {
                _klub.EmailKontakt = value;
                OnPropertyChanged(nameof(EmailKontakt));
            }
        }

        public string Opis
        {
            get => _klub.Opis ?? string.Empty;
            set
            {
                _klub.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public bool CzyAktywny
        {
            get => _klub.CzyAktywny;
            set
            {
                _klub.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }

        public DateTime DataUtworzenia
        {
            get => _klub.DataUtworzenia;
            set
            {
                _klub.DataUtworzenia = value;
                OnPropertyChanged(nameof(DataUtworzenia));
            }
        }
        #endregion

        #region Helpers
        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa klubu jest wymagana.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(EmailKontakt))
            {
                try
                {
                    var _ = new System.Net.Mail.MailAddress(EmailKontakt);
                }
                catch
                {
                    message = "Email kontaktowy wygląda na niepoprawny.";
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(KodPocztowy))
            {
                var kp = KodPocztowy.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(kp, @"^\d{2}-\d{3}$"))
                {
                    message = "Kod pocztowy powinien mieć format 00-000.";
                    return false;
                }
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
                    _klub.KlubId = _klubId;
                    success = await _klubyService.UpdateAsync(_klub);
                }
                else
                {
                    var createDto = new CreateKlubDto
                    {
                        Nazwa = _klub.Nazwa,
                        Miasto = _klub.Miasto,
                        Ulica = _klub.Ulica,
                        KodPocztowy = _klub.KodPocztowy,
                        Telefon = _klub.Telefon,
                        EmailKontakt = _klub.EmailKontakt,
                        Opis = _klub.Opis,
                        CzyAktywny = _klub.CzyAktywny,
                        DataUtworzenia = _klub.DataUtworzenia
                    };

                    success = await _klubyService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać klubu.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                KlubSaved?.Invoke(this, EventArgs.Empty);
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