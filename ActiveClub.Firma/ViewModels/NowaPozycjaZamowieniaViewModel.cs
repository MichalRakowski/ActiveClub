using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.PozycjeZamowienia;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaPozycjaZamowieniaViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler? PozycjaSaved;
        #endregion

        #region Fields
        private readonly PozycjeZamowieniaService _service;
        private readonly UpdatePozycjaZamowieniaDto _pozycja;
        private readonly bool _isEditMode;
        private readonly int _pozycjaId;
        #endregion

        #region Collections
        public ObservableCollection<LookupItemDto> Zamowienia { get; set; } = new();
        public ObservableCollection<LookupItemDto> Typy { get; set; } = new();
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
        public NowaPozycjaZamowieniaViewModel(PozycjeZamowieniaService service)
        {
            DisplayName = "Nowa pozycja zamówienia";
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = false;
            _pozycjaId = 0;

            _pozycja = new UpdatePozycjaZamowieniaDto
            {
                ZamowienieId = 0,
                Typ = string.Empty,
                RefId = null,
                Nazwa = string.Empty,
                Opis = string.Empty,
                Ilosc = 1,
                CenaJedn = 0,
                KwotaBrutto = 0
            };

            _ = LoadLookupsAsync();
        }

        public NowaPozycjaZamowieniaViewModel(PozycjeZamowieniaService service, PozycjaZamowieniaDto dto)
        {
            DisplayName = "Edycja pozycji zamówienia";
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = true;
            _pozycjaId = dto.PozycjaId;

            _pozycja = new UpdatePozycjaZamowieniaDto
            {
                ZamowienieId = dto.ZamowienieId,
                Typ = dto.Typ,
                RefId = dto.RefId,
                Nazwa = dto.Nazwa,
                Opis = dto.Opis,
                Ilosc = dto.Ilosc,
                CenaJedn = dto.CenaJedn,
                KwotaBrutto = dto.KwotaBrutto
            };

            _ = LoadLookupsAsync();
        }
        #endregion

        #region Properties
        public int ZamowienieId
        {
            get => _pozycja.ZamowienieId;
            set
            {
                _pozycja.ZamowienieId = value;
                OnPropertyChanged(nameof(ZamowienieId));
            }
        }

        public string Typ
        {
            get => _pozycja.Typ ?? string.Empty;
            set
            {
                _pozycja.Typ = value;
                OnPropertyChanged(nameof(Typ));
            }
        }

        public int? RefId
        {
            get => _pozycja.RefId;
            set
            {
                _pozycja.RefId = value;
                OnPropertyChanged(nameof(RefId));
            }
        }

        public string Nazwa
        {
            get => _pozycja.Nazwa ?? string.Empty;
            set
            {
                _pozycja.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string? Opis
        {
            get => _pozycja.Opis;
            set
            {
                _pozycja.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public int Ilosc
        {
            get => _pozycja.Ilosc;
            set
            {
                _pozycja.Ilosc = value;
                OnPropertyChanged(nameof(Ilosc));
                PrzeliczKwote();
            }
        }

        public decimal CenaJedn
        {
            get => _pozycja.CenaJedn;
            set
            {
                _pozycja.CenaJedn = value;
                OnPropertyChanged(nameof(CenaJedn));
                PrzeliczKwote();
            }
        }

        public decimal KwotaBrutto
        {
            get => _pozycja.KwotaBrutto;
            set
            {
                _pozycja.KwotaBrutto = value;
                OnPropertyChanged(nameof(KwotaBrutto));
            }
        }
        #endregion

        #region Helpers
        private async Task LoadLookupsAsync()
        {
            try
            {
                var formData = await _service.GetFormDataAsync();

                Zamowienia = new ObservableCollection<LookupItemDto>(formData.Zamowienia);
                Typy = new ObservableCollection<LookupItemDto>(formData.Typy);

                OnPropertyChanged(nameof(Zamowienia));
                OnPropertyChanged(nameof(Typy));
                OnPropertyChanged(nameof(ZamowienieId));
                OnPropertyChanged(nameof(Typ));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd podczas ładowania danych formularza: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void PrzeliczKwote()
        {
            if (Ilosc >= 0 && CenaJedn >= 0)
                KwotaBrutto = Ilosc * CenaJedn;
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (ZamowienieId <= 0)
            {
                message = "Wybierz zamówienie.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Typ))
            {
                message = "Typ jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa jest wymagana.";
                return false;
            }

            if (Ilosc <= 0)
            {
                message = "Ilość musi być większa od 0.";
                return false;
            }

            if (CenaJedn < 0)
            {
                message = "Cena jednostkowa nie może być ujemna.";
                return false;
            }

            if (KwotaBrutto < 0)
            {
                message = "Kwota brutto nie może być ujemna.";
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
                    success = await _service.UpdateAsync(_pozycjaId, _pozycja);
                }
                else
                {
                    var createDto = new CreatePozycjaZamowieniaDto
                    {
                        ZamowienieId = _pozycja.ZamowienieId,
                        Typ = _pozycja.Typ,
                        RefId = _pozycja.RefId,
                        Nazwa = _pozycja.Nazwa,
                        Opis = _pozycja.Opis,
                        Ilosc = _pozycja.Ilosc,
                        CenaJedn = _pozycja.CenaJedn,
                        KwotaBrutto = _pozycja.KwotaBrutto
                    };

                    success = await _service.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać pozycji zamówienia.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                PozycjaSaved?.Invoke(this, EventArgs.Empty);
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