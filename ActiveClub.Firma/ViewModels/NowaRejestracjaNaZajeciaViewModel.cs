using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaRejestracjaNaZajeciaViewModel : WorkspaceViewModel
    {
        public event EventHandler? RejestracjaSaved;

        private readonly RejestracjeNaZajeciaService _service;
        private readonly UpdateRejestracjaNaZajeciaDto _rejestracja;
        private readonly bool _isEditMode;
        private readonly int _rejestracjaId;

        private ObservableCollection<LookupItemDto> _harmonogramy = new();
        private ObservableCollection<LookupItemDto> _uzytkownicy = new();
        private ObservableCollection<LookupItemDto> _statusy = new();

        private LookupItemDto? _selectedHarmonogram;
        private LookupItemDto? _selectedUzytkownik;
        private LookupItemDto? _selectedStatus;

        private string _selectedUzytkownikNazwa = string.Empty;
        private string _selectedHarmonogramNazwa = string.Empty;

        public NowaRejestracjaNaZajeciaViewModel(RejestracjeNaZajeciaService service)
        {
            DisplayName = "Nowa rezerwacja na zajęcia";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = false;
            _rejestracjaId = 0;

            _rejestracja = new UpdateRejestracjaNaZajeciaDto
            {
                HarmonogramId = 0,
                UzytkownikId = 0,
                Status = "NOWA",
                DataRejestracji = DateTime.Now,
                Zrodlo = "Firma"
            };

            _ = LoadFormDataAsync();
        }

        public NowaRejestracjaNaZajeciaViewModel(
            RejestracjeNaZajeciaService service,
            RejestracjaNaZajeciaDto dto)
        {
            DisplayName = "Edycja rezerwacji na zajęcia";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = true;
            _rejestracjaId = dto.RejestracjaId;

            _rejestracja = new UpdateRejestracjaNaZajeciaDto
            {
                HarmonogramId = dto.HarmonogramId,
                UzytkownikId = dto.UzytkownikId,
                Status = dto.Status?.Trim().ToUpperInvariant() ?? "NOWA",
                DataRejestracji = dto.DataRejestracji,
                Zrodlo = dto.Zrodlo ?? "Firma"
            };

            _ = LoadFormDataAsync();
        }

        public ObservableCollection<LookupItemDto> Harmonogramy
        {
            get => _harmonogramy;
            set
            {
                _harmonogramy = value;
                OnPropertyChanged(nameof(Harmonogramy));
            }
        }

        public ObservableCollection<LookupItemDto> Uzytkownicy
        {
            get => _uzytkownicy;
            set
            {
                _uzytkownicy = value;
                OnPropertyChanged(nameof(Uzytkownicy));
            }
        }

        public ObservableCollection<LookupItemDto> Statusy
        {
            get => _statusy;
            set
            {
                _statusy = value;
                OnPropertyChanged(nameof(Statusy));
            }
        }

        public LookupItemDto? SelectedHarmonogram
        {
            get => _selectedHarmonogram;
            set
            {
                _selectedHarmonogram = value;
                _rejestracja.HarmonogramId = value?.Id ?? 0;
                SelectedHarmonogramNazwa = value?.Nazwa ?? string.Empty;
                OnPropertyChanged(nameof(SelectedHarmonogram));
            }
        }

        public LookupItemDto? SelectedUzytkownik
        {
            get => _selectedUzytkownik;
            set
            {
                _selectedUzytkownik = value;
                _rejestracja.UzytkownikId = value?.Id ?? 0;
                SelectedUzytkownikNazwa = value?.Nazwa ?? string.Empty;
                OnPropertyChanged(nameof(SelectedUzytkownik));
            }
        }

        public string SelectedUzytkownikNazwa
        {
            get => _selectedUzytkownikNazwa;
            set
            {
                _selectedUzytkownikNazwa = value;
                OnPropertyChanged(nameof(SelectedUzytkownikNazwa));
            }
        }
        public string SelectedHarmonogramNazwa
        {
            get => _selectedHarmonogramNazwa;
            set
            {
                _selectedHarmonogramNazwa = value;
                OnPropertyChanged(nameof(SelectedHarmonogramNazwa));
            }
        }
        public LookupItemDto? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                _rejestracja.Status = value?.Nazwa ?? string.Empty;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public DateTime? DataRejestracji
        {
            get => _rejestracja.DataRejestracji;
            set
            {
                _rejestracja.DataRejestracji = value;
                OnPropertyChanged(nameof(DataRejestracji));
            }
        }

        public string Zrodlo
        {
            get => _rejestracja.Zrodlo ?? string.Empty;
            set
            {
                _rejestracja.Zrodlo = value;
                OnPropertyChanged(nameof(Zrodlo));
            }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);
        private BaseCommand? _wybierzHarmonogramCommand;
        public ICommand WybierzHarmonogramCommand =>
            _wybierzHarmonogramCommand ??= new BaseCommand(WybierzHarmonogram);
        private BaseCommand? _wybierzUzytkownikaCommand;
        public ICommand WybierzUzytkownikaCommand =>
            _wybierzUzytkownikaCommand ??= new BaseCommand(WybierzUzytkownika);

        private void WybierzUzytkownika()
        {
            var users = Uzytkownicy
                .Select(x => new UzytkownikDto
                {
                    Id = x.Id,
                    Imie = x.Nazwa,
                    Nazwisko = string.Empty,
                    Email = string.Empty
                })
                .ToList();

            var vm = new WybierzUzytkownikaViewModel(users);

            var window = new WybierzUzytkownikaWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedUser != null)
            {
                SelectedUzytkownik = Uzytkownicy.FirstOrDefault(x => x.Id == vm.SelectedUser.Id);
                SelectedUzytkownikNazwa = vm.SelectedUser.Imie;
            }
        }
        private void WybierzHarmonogram()
        {
            var vm = new WybierzHarmonogramViewModel(Harmonogramy);

            var window = new WybierzHarmonogramWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedItem != null)
            {
                SelectedHarmonogram = Harmonogramy.FirstOrDefault(x => x.Id == vm.SelectedItem.Id);
                SelectedHarmonogramNazwa = vm.SelectedItem.Nazwa;
            }
        }
        private async Task LoadFormDataAsync()
        {
            try
            {
                var data = await _service.GetFormDataAsync();

                if (data == null)
                {
                    MessageBox.Show(
                        "Nie udało się pobrać danych formularza.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                Harmonogramy = new ObservableCollection<LookupItemDto>(
                    data.Harmonogramy ?? Enumerable.Empty<LookupItemDto>());

                Uzytkownicy = new ObservableCollection<LookupItemDto>(
                    data.Uzytkownicy ?? Enumerable.Empty<LookupItemDto>());

                Statusy = new ObservableCollection<LookupItemDto>(
                    data.Statusy ?? Enumerable.Empty<LookupItemDto>());

                if (_isEditMode)
                {
                    SelectedHarmonogram = Harmonogramy
                        .FirstOrDefault(x => x.Id == _rejestracja.HarmonogramId);

                    SelectedUzytkownik = Uzytkownicy
                        .FirstOrDefault(x => x.Id == _rejestracja.UzytkownikId);

                    SelectedStatus = Statusy
     .FirstOrDefault(x =>
         string.Equals(x.Nazwa, _rejestracja.Status, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    SelectedStatus = Statusy.FirstOrDefault(x => x.Nazwa == "NOWA");
                }

                SelectedUzytkownikNazwa = SelectedUzytkownik?.Nazwa ?? string.Empty;
                SelectedHarmonogramNazwa = SelectedHarmonogram?.Nazwa ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd ładowania danych",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (_rejestracja.HarmonogramId <= 0)
            {
                message = "Wybierz harmonogram.";
                return false;
            }

            if (_rejestracja.UzytkownikId <= 0)
            {
                message = "Wybierz użytkownika.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public async Task SaveAndCloseAsync()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(
                    msg,
                    "Błędne dane",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var status = _rejestracja.Status?.Trim().ToLowerInvariant();

            _rejestracja.Status = status switch
            {
                "nowa" => "NOWA",
                "potwierdzona" => "POTWIERDZONA",
                "anulowana" => "ANULOWANA",
                "obecny" => "OBECNY",
                "nieobecny" => "NIEOBECNY",
                _ => _rejestracja.Status?.Trim().ToUpperInvariant()
            };

            try
            {
                bool success;

                if (_isEditMode)
                {
                    success = await _service.UpdateAsync(_rejestracjaId, _rejestracja);
                }
                else
                {
                    var createDto = new CreateRejestracjaNaZajeciaDto
                    {
                        HarmonogramId = _rejestracja.HarmonogramId,
                        UzytkownikId = _rejestracja.UzytkownikId,
                        Status = _rejestracja.Status,
                        DataRejestracji = _rejestracja.DataRejestracji,
                        Zrodlo = _rejestracja.Zrodlo
                    };

                    success = await _service.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać rezerwacji.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                RejestracjaSaved?.Invoke(this, EventArgs.Empty);
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
    }
}