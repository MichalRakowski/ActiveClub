using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Rabaty;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyRabatViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler? RabatSaved;
        #endregion

        #region Fields
        private readonly RabatyService _rabatyService;
        private readonly UpdateRabatDto _rabat;
        private readonly bool _isEditMode;
        private readonly int _rabatId;
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
        public NowyRabatViewModel(RabatyService rabatyService)
        {
            DisplayName = "Nowy rabat";
            _rabatyService = rabatyService ?? throw new ArgumentNullException(nameof(rabatyService));
            _isEditMode = false;
            _rabatId = 0;

            _rabat = new UpdateRabatDto
            {
                Kod = string.Empty,
                Typ = string.Empty,
                Wartosc = 0,
                DataOd = null,
                DataDo = null,
                MinimalnaKwotaZamowienia = null,
                MaksLiczbaUzyc = null,
                CzyAktywny = true
            };
        }

        public NowyRabatViewModel(RabatyService rabatyService, RabatDto dto)
        {
            DisplayName = "Edycja rabatu";
            _rabatyService = rabatyService ?? throw new ArgumentNullException(nameof(rabatyService));
            _isEditMode = true;
            _rabatId = dto.RabatId;

            _rabat = new UpdateRabatDto
            {
                Kod = dto.Kod ?? string.Empty,
                Typ = dto.Typ ?? string.Empty,
                Wartosc = dto.Wartosc,
                DataOd = dto.DataOd,
                DataDo = dto.DataDo,
                MinimalnaKwotaZamowienia = dto.MinimalnaKwotaZamowienia,
                MaksLiczbaUzyc = dto.MaksLiczbaUzyc,
                CzyAktywny = dto.CzyAktywny
            };
        }
        #endregion

        #region Properties
        public string Kod
        {
            get => _rabat.Kod ?? string.Empty;
            set
            {
                _rabat.Kod = value;
                OnPropertyChanged(nameof(Kod));
            }
        }

        public string Typ
        {
            get => _rabat.Typ ?? string.Empty;
            set
            {
                _rabat.Typ = value;
                OnPropertyChanged(nameof(Typ));
            }
        }

        public decimal Wartosc
        {
            get => _rabat.Wartosc;
            set
            {
                _rabat.Wartosc = value;
                OnPropertyChanged(nameof(Wartosc));
            }
        }

        public DateTime? DataOdView
        {
            get => _rabat.DataOd.HasValue
                ? _rabat.DataOd.Value.ToDateTime(TimeOnly.MinValue)
                : null;
            set
            {
                _rabat.DataOd = value.HasValue
                    ? DateOnly.FromDateTime(value.Value)
                    : null;

                OnPropertyChanged(nameof(DataOdView));
            }
        }

        public DateTime? DataDoView
        {
            get => _rabat.DataDo.HasValue
                ? _rabat.DataDo.Value.ToDateTime(TimeOnly.MinValue)
                : null;
            set
            {
                _rabat.DataDo = value.HasValue
                    ? DateOnly.FromDateTime(value.Value)
                    : null;

                OnPropertyChanged(nameof(DataDoView));
            }
        }

        public decimal? MinimalnaKwotaZamowienia
        {
            get => _rabat.MinimalnaKwotaZamowienia;
            set
            {
                _rabat.MinimalnaKwotaZamowienia = value;
                OnPropertyChanged(nameof(MinimalnaKwotaZamowienia));
            }
        }

        public int? MaksLiczbaUzyc
        {
            get => _rabat.MaksLiczbaUzyc;
            set
            {
                _rabat.MaksLiczbaUzyc = value;
                OnPropertyChanged(nameof(MaksLiczbaUzyc));
            }
        }

        public bool CzyAktywny
        {
            get => _rabat.CzyAktywny;
            set
            {
                _rabat.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }
        #endregion

        #region Helpers
        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Kod))
            {
                message = "Kod rabatu jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Typ))
            {
                message = "Typ rabatu jest wymagany.";
                return false;
            }

            if (Wartosc <= 0)
            {
                message = "Wartość rabatu musi być większa od 0.";
                return false;
            }

            if (_rabat.DataOd.HasValue && _rabat.DataDo.HasValue && _rabat.DataOd > _rabat.DataDo)
            {
                message = "Data od nie może być większa niż data do.";
                return false;
            }

            if (MinimalnaKwotaZamowienia.HasValue && MinimalnaKwotaZamowienia < 0)
            {
                message = "Minimalna kwota zamówienia nie może być ujemna.";
                return false;
            }

            if (MaksLiczbaUzyc.HasValue && MaksLiczbaUzyc < 0)
            {
                message = "Maksymalna liczba użyć nie może być ujemna.";
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
                    success = await _rabatyService.UpdateAsync(_rabatId, _rabat);
                }
                else
                {
                    var createDto = new CreateRabatDto
                    {
                        Kod = _rabat.Kod,
                        Typ = _rabat.Typ,
                        Wartosc = _rabat.Wartosc,
                        DataOd = _rabat.DataOd,
                        DataDo = _rabat.DataDo,
                        MinimalnaKwotaZamowienia = _rabat.MinimalnaKwotaZamowienia,
                        MaksLiczbaUzyc = _rabat.MaksLiczbaUzyc,
                        CzyAktywny = _rabat.CzyAktywny
                    };

                    success = await _rabatyService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać rabatu.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                RabatSaved?.Invoke(this, EventArgs.Empty);
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