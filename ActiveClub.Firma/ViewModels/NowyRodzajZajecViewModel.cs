using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyRodzajZajecViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler? RodzajZajecSaved;
        #endregion

        #region Fields
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly UpdateRodzajZajecDto _rodzajZajec;
        private readonly bool _isEditMode;
        private readonly int _rodzajZajecId;
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
        public NowyRodzajZajecViewModel(RodzajeZajecService rodzajeZajecService)
        {
            DisplayName = "Nowy rodzaj zajęć";
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _isEditMode = false;
            _rodzajZajecId = 0;

            _rodzajZajec = new UpdateRodzajZajecDto
            {
                Nazwa = string.Empty,
                Opis = string.Empty,
                CzasTrwaniaMin = null,
                CzyAktywne = true,
                CzyWidoczneOnline = true
            };
        }

        public NowyRodzajZajecViewModel(RodzajeZajecService rodzajeZajecService, RodzajZajecDto dto)
        {
            DisplayName = "Edycja rodzaju zajęć";
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _isEditMode = true;
            _rodzajZajecId = dto.RodzajZajecId;

            _rodzajZajec = new UpdateRodzajZajecDto
            {
                Nazwa = dto.Nazwa ?? string.Empty,
                Opis = dto.Opis ?? string.Empty,
                CzasTrwaniaMin = dto.CzasTrwaniaMin,
                CzyAktywne = dto.CzyAktywne,
                CzyWidoczneOnline = dto.CzyWidoczneOnline
            };
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get => _rodzajZajec.Nazwa ?? string.Empty;
            set
            {
                _rodzajZajec.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string Opis
        {
            get => _rodzajZajec.Opis ?? string.Empty;
            set
            {
                _rodzajZajec.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public int? CzasTrwaniaMin
        {
            get => _rodzajZajec.CzasTrwaniaMin;
            set
            {
                _rodzajZajec.CzasTrwaniaMin = value;
                OnPropertyChanged(nameof(CzasTrwaniaMin));
            }
        }

        public bool CzyAktywne
        {
            get => _rodzajZajec.CzyAktywne;
            set
            {
                _rodzajZajec.CzyAktywne = value;
                OnPropertyChanged(nameof(CzyAktywne));
            }
        }

        public bool CzyWidoczneOnline
        {
            get => _rodzajZajec.CzyWidoczneOnline;
            set
            {
                _rodzajZajec.CzyWidoczneOnline = value;
                OnPropertyChanged(nameof(CzyWidoczneOnline));
            }
        }
        #endregion

        #region Helpers
        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa rodzaju zajęć jest wymagana.";
                return false;
            }

            if (CzasTrwaniaMin.HasValue && CzasTrwaniaMin <= 0)
            {
                message = "Czas trwania musi być większy od 0.";
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
                    success = await _rodzajeZajecService.UpdateAsync(_rodzajZajecId, _rodzajZajec);
                }
                else
                {
                    var createDto = new CreateRodzajZajecDto
                    {
                        Nazwa = _rodzajZajec.Nazwa,
                        Opis = _rodzajZajec.Opis,
                        CzasTrwaniaMin = _rodzajZajec.CzasTrwaniaMin,
                        CzyAktywne = _rodzajZajec.CzyAktywne,
                        CzyWidoczneOnline = _rodzajZajec.CzyWidoczneOnline
                    };

                    success = await _rodzajeZajecService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać rodzaju zajęć.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                RodzajZajecSaved?.Invoke(this, EventArgs.Empty);
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