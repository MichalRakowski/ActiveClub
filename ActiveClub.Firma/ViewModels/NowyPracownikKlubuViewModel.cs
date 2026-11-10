using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.PracownicyKlubu;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyPracownikKlubuViewModel : WorkspaceViewModel
    {
        public event EventHandler? PracownikKlubuSaved;

        private readonly PracownicyKlubuService _service;
        private readonly UpdatePracownikKlubuDto _model;
        private readonly bool _isEditMode;
        private readonly int _id;

        private string _selectedUzytkownikNazwa = string.Empty;
        private string _selectedKlubNazwa = string.Empty;

        public ObservableCollection<LookupItemDto> Uzytkownicy { get; set; } = new();
        public ObservableCollection<LookupItemDto> Kluby { get; set; } = new();
        public ObservableCollection<LookupItemDto> Stanowiska { get; set; } = new();

        public NowyPracownikKlubuViewModel(PracownicyKlubuService service)
        {
            DisplayName = "Nowy pracownik klubu";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = false;
            _id = 0;

            _model = new UpdatePracownikKlubuDto
            {
                CzyAktywny = true
            };

            _ = LoadFormDataAsync();
        }

        public NowyPracownikKlubuViewModel(
            PracownicyKlubuService service,
            PracownikKlubuDto dto)
        {
            DisplayName = "Edycja pracownika klubu";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = true;
            _id = dto.PracownikKlubId;

            _model = new UpdatePracownikKlubuDto
            {
                UzytkownikId = dto.UzytkownikId,
                KlubId = dto.KlubId,
                StanowiskoId = dto.StanowiskoId,
                DataOd = dto.DataOd,
                DataDo = dto.DataDo,
                CzyAktywny = dto.CzyAktywny,
                Uwagi = dto.Uwagi
            };

            _ = LoadFormDataAsync();
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

        public string SelectedKlubNazwa
        {
            get => _selectedKlubNazwa;
            set
            {
                _selectedKlubNazwa = value;
                OnPropertyChanged(nameof(SelectedKlubNazwa));
            }
        }

        public int UzytkownikId
        {
            get => _model.UzytkownikId;
            set
            {
                _model.UzytkownikId = value;
                OnPropertyChanged(nameof(UzytkownikId));

                var selected = Uzytkownicy.FirstOrDefault(x => x.Id == value);
                SelectedUzytkownikNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public int KlubId
        {
            get => _model.KlubId;
            set
            {
                _model.KlubId = value;
                OnPropertyChanged(nameof(KlubId));

                var selected = Kluby.FirstOrDefault(x => x.Id == value);
                SelectedKlubNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public int StanowiskoId
        {
            get => _model.StanowiskoId;
            set
            {
                _model.StanowiskoId = value;
                OnPropertyChanged(nameof(StanowiskoId));
            }
        }

        public DateTime? DataOd
        {
            get => _model.DataOd;
            set
            {
                _model.DataOd = value;
                OnPropertyChanged(nameof(DataOd));
            }
        }

        public DateTime? DataDo
        {
            get => _model.DataDo;
            set
            {
                _model.DataDo = value;
                OnPropertyChanged(nameof(DataDo));
            }
        }

        public bool CzyAktywny
        {
            get => _model.CzyAktywny;
            set
            {
                _model.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }

        public string? Uwagi
        {
            get => _model.Uwagi;
            set
            {
                _model.Uwagi = value;
                OnPropertyChanged(nameof(Uwagi));
            }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzUzytkownikaCommand;
        public ICommand WybierzUzytkownikaCommand =>
            _wybierzUzytkownikaCommand ??= new BaseCommand(WybierzUzytkownika);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(WybierzKlub);

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
                UzytkownikId = vm.SelectedUser.Id;
                SelectedUzytkownikNazwa = vm.SelectedUser.Imie;
            }
        }

        private void WybierzKlub()
        {
            var kluby = Kluby
                .Select(x => new KlubDto
                {
                    KlubId = x.Id,
                    Nazwa = x.Nazwa
                })
                .ToList();

            var vm = new WybierzKlubViewModel(kluby);

            var window = new WybierzKlubWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedItem != null)
            {
                KlubId = vm.SelectedItem.KlubId;
                SelectedKlubNazwa = vm.SelectedItem.Nazwa;
            }
        }

        private async Task LoadFormDataAsync()
        {
            try
            {
                var data = await _service.GetFormDataAsync();

                if (data == null)
                    return;

                Uzytkownicy = new ObservableCollection<LookupItemDto>(
                    data.Uzytkownicy ?? Enumerable.Empty<LookupItemDto>());

                Kluby = new ObservableCollection<LookupItemDto>(
                    data.Kluby ?? Enumerable.Empty<LookupItemDto>());

                Stanowiska = new ObservableCollection<LookupItemDto>(
                    data.Stanowiska ?? Enumerable.Empty<LookupItemDto>());

                OnPropertyChanged(nameof(Uzytkownicy));
                OnPropertyChanged(nameof(Kluby));
                OnPropertyChanged(nameof(Stanowiska));

                SelectedUzytkownikNazwa =
                    Uzytkownicy.FirstOrDefault(x => x.Id == UzytkownikId)?.Nazwa ?? string.Empty;

                SelectedKlubNazwa =
                    Kluby.FirstOrDefault(x => x.Id == KlubId)?.Nazwa ?? string.Empty;

                OnPropertyChanged(nameof(UzytkownikId));
                OnPropertyChanged(nameof(KlubId));
                OnPropertyChanged(nameof(StanowiskoId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd ładowania danych formularza",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (UzytkownikId <= 0)
            {
                message = "Wybierz użytkownika.";
                return false;
            }

            if (KlubId <= 0)
            {
                message = "Wybierz klub.";
                return false;
            }

            if (StanowiskoId <= 0)
            {
                message = "Wybierz stanowisko.";
                return false;
            }

            if (DataOd.HasValue && DataDo.HasValue && DataOd > DataDo)
            {
                message = "Data od nie może być większa niż data do.";
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

            try
            {
                bool success;

                if (_isEditMode)
                {
                    success = await _service.UpdateAsync(_id, _model);
                }
                else
                {
                    var createDto = new CreatePracownikKlubuDto
                    {
                        UzytkownikId = _model.UzytkownikId,
                        KlubId = _model.KlubId,
                        StanowiskoId = _model.StanowiskoId,
                        DataOd = _model.DataOd,
                        DataDo = _model.DataDo,
                        CzyAktywny = _model.CzyAktywny,
                        Uwagi = _model.Uwagi
                    };

                    success = await _service.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać pracownika klubu.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                PracownikKlubuSaved?.Invoke(this, EventArgs.Empty);
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