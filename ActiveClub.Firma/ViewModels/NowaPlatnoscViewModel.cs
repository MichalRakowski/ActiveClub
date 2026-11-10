using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Platnosci;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaPlatnoscViewModel : WorkspaceViewModel
    {
        public event EventHandler? PlatnoscSaved;

        private readonly PlatnosciService _platnosciService;
        private readonly UpdatePlatnoscDto _platnosc;
        private readonly bool _isEditMode;
        private readonly int _platnoscId;

        private int _selectedZamowienieId;
        private int _selectedMetodaPlatnosciId;
        private LookupItemDto? _selectedStatus;
        private LookupItemDto? _selectedWaluta;

        public ObservableCollection<LookupItemDto> Zamowienia { get; set; } = new();
        public ObservableCollection<LookupItemDto> MetodyPlatnosci { get; set; } = new();
        public ObservableCollection<LookupItemDto> Statusy { get; set; } = new();
        public ObservableCollection<LookupItemDto> Waluty { get; set; } = new();

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());
                return _saveCommand;
            }
        }

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                _cancelCommand ??= new BaseCommand(() => Cancel());
                return _cancelCommand;
            }
        }

        public NowaPlatnoscViewModel(PlatnosciService platnosciService)
        {
            DisplayName = "Nowa płatność";
            _platnosciService = platnosciService ?? throw new ArgumentNullException(nameof(platnosciService));
            _isEditMode = false;
            _platnoscId = 0;

            _platnosc = new UpdatePlatnoscDto
            {
                ZamowienieId = 0,
                MetodaPlatnosciId = 0,
                Kwota = 0,
                Waluta = "PLN",
                Status = "Nowa",
                Provider = string.Empty,
                ProviderTransactionId = string.Empty,
                DataPlatnosci = null,
                DataUtworzenia = DateTime.Now,
                Uwagi = string.Empty
            };

            LoadFormDataAsync();
        }

        public NowaPlatnoscViewModel(PlatnosciService platnosciService, PlatnoscDto dto)
        {
            DisplayName = "Edycja płatności";
            _platnosciService = platnosciService ?? throw new ArgumentNullException(nameof(platnosciService));
            _isEditMode = true;
            _platnoscId = dto.PlatnoscId;

            _platnosc = new UpdatePlatnoscDto
            {
                ZamowienieId = dto.ZamowienieId,
                MetodaPlatnosciId = dto.MetodaPlatnosciId,
                Kwota = dto.Kwota,
                Waluta = dto.Waluta ?? "PLN",
                Status = dto.Status ?? "Nowa",
                Provider = dto.Provider ?? string.Empty,
                ProviderTransactionId = dto.ProviderTransactionId ?? string.Empty,
                DataPlatnosci = dto.DataPlatnosci,
                DataUtworzenia = dto.DataUtworzenia,
                Uwagi = dto.Uwagi ?? string.Empty
            };

            _selectedZamowienieId = dto.ZamowienieId;
            _selectedMetodaPlatnosciId = dto.MetodaPlatnosciId;

            LoadFormDataAsync();
        }

        public int SelectedZamowienieId
        {
            get => _selectedZamowienieId;
            set
            {
                _selectedZamowienieId = value;
                _platnosc.ZamowienieId = value;
                OnPropertyChanged(nameof(SelectedZamowienieId));
            }
        }

        public int SelectedMetodaPlatnosciId
        {
            get => _selectedMetodaPlatnosciId;
            set
            {
                _selectedMetodaPlatnosciId = value;
                _platnosc.MetodaPlatnosciId = value;
                OnPropertyChanged(nameof(SelectedMetodaPlatnosciId));
            }
        }

        public LookupItemDto? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                _platnosc.Status = value?.Nazwa ?? string.Empty;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public LookupItemDto? SelectedWaluta
        {
            get => _selectedWaluta;
            set
            {
                _selectedWaluta = value;
                _platnosc.Waluta = value?.Nazwa ?? string.Empty;
                OnPropertyChanged(nameof(SelectedWaluta));
            }
        }

        public decimal Kwota
        {
            get => _platnosc.Kwota;
            set
            {
                _platnosc.Kwota = value;
                OnPropertyChanged(nameof(Kwota));
            }
        }

        public string Provider
        {
            get => _platnosc.Provider ?? string.Empty;
            set
            {
                _platnosc.Provider = value;
                OnPropertyChanged(nameof(Provider));
            }
        }

        public string ProviderTransactionId
        {
            get => _platnosc.ProviderTransactionId ?? string.Empty;
            set
            {
                _platnosc.ProviderTransactionId = value;
                OnPropertyChanged(nameof(ProviderTransactionId));
            }
        }

        public DateTime? DataPlatnosci
        {
            get => _platnosc.DataPlatnosci;
            set
            {
                _platnosc.DataPlatnosci = value;
                OnPropertyChanged(nameof(DataPlatnosci));
            }
        }

        public DateTime? DataUtworzenia
        {
            get => _platnosc.DataUtworzenia;
            set
            {
                _platnosc.DataUtworzenia = value;
                OnPropertyChanged(nameof(DataUtworzenia));
            }
        }

        public string Uwagi
        {
            get => _platnosc.Uwagi ?? string.Empty;
            set
            {
                _platnosc.Uwagi = value;
                OnPropertyChanged(nameof(Uwagi));
            }
        }

        private async void LoadFormDataAsync()
        {
            try
            {
                var formData = await _platnosciService.GetFormDataAsync();

                Zamowienia.Clear();
                foreach (var item in formData.Zamowienia)
                    Zamowienia.Add(item);

                MetodyPlatnosci.Clear();
                foreach (var item in formData.MetodyPlatnosci)
                    MetodyPlatnosci.Add(item);

                Statusy.Clear();
                foreach (var item in formData.Statusy)
                    Statusy.Add(item);

                Waluty.Clear();
                foreach (var item in formData.Waluty)
                    Waluty.Add(item);

                OnPropertyChanged(nameof(Zamowienia));
                OnPropertyChanged(nameof(MetodyPlatnosci));
                OnPropertyChanged(nameof(Statusy));
                OnPropertyChanged(nameof(Waluty));

                if (_isEditMode)
                {
                    SelectedStatus = Statusy.FirstOrDefault(x => x.Nazwa == _platnosc.Status);
                    SelectedWaluta = Waluty.FirstOrDefault(x => x.Nazwa == _platnosc.Waluta);
                }
                else
                {
                    SelectedStatus = Statusy.FirstOrDefault(x => x.Nazwa == "Nowa");
                    SelectedWaluta = Waluty.FirstOrDefault(x => x.Nazwa == "PLN");
                }

                OnPropertyChanged(nameof(SelectedZamowienieId));
                OnPropertyChanged(nameof(SelectedMetodaPlatnosciId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd ładowania danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (_platnosc.ZamowienieId <= 0)
            {
                message = "Zamówienie jest wymagane.";
                return false;
            }

            if (_platnosc.MetodaPlatnosciId <= 0)
            {
                message = "Metoda płatności jest wymagana.";
                return false;
            }

            if (_platnosc.Kwota <= 0)
            {
                message = "Kwota musi być większa od 0.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_platnosc.Waluta))
            {
                message = "Waluta jest wymagana.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_platnosc.Status))
            {
                message = "Status jest wymagany.";
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
                    success = await _platnosciService.UpdateAsync(_platnoscId, _platnosc);
                }
                else
                {
                    var createDto = new CreatePlatnoscDto
                    {
                        ZamowienieId = _platnosc.ZamowienieId,
                        MetodaPlatnosciId = _platnosc.MetodaPlatnosciId,
                        Kwota = _platnosc.Kwota,
                        Waluta = _platnosc.Waluta,
                        Status = _platnosc.Status,
                        Provider = _platnosc.Provider,
                        ProviderTransactionId = _platnosc.ProviderTransactionId,
                        DataPlatnosci = _platnosc.DataPlatnosci,
                        DataUtworzenia = _platnosc.DataUtworzenia,
                        Uwagi = _platnosc.Uwagi
                    };

                    success = await _platnosciService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show("Nie udało się zapisać płatności.", "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                PlatnoscSaved?.Invoke(this, EventArgs.Empty);
                OnRequestClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            OnRequestClose();
        }
    }
}