using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Models;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using ActiveClub.Shared.DTOs.Zamowienia;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NoweZamowienieViewModel : WorkspaceViewModel
    {
        public event EventHandler? ZamowienieSaved;

        private readonly ZamowieniaService _zamowieniaService;
        private readonly UpdateZamowienieDto _zamowienie;
        private readonly bool _isEditMode;
        private readonly int _zamowienieId;

        private ObservableCollection<ComboBoxItem> _uzytkownicy = new();
        private ObservableCollection<ComboBoxItem> _kluby = new();
        private ObservableCollection<ComboBoxItem> _metodyPlatnosci = new();
        private ObservableCollection<ComboBoxItem> _rabaty = new();
        private ObservableCollection<ComboBoxItem> _utworzylUzytkownicy = new();

        private string _selectedUzytkownikNazwa = string.Empty;
        private string _selectedKlubNazwa = string.Empty;
        private string _selectedUtworzylUzytkownikNazwa = string.Empty;

        public NoweZamowienieViewModel(ZamowieniaService zamowieniaService)
        {
            DisplayName = "Nowe zamówienie";

            _zamowieniaService = zamowieniaService ?? throw new ArgumentNullException(nameof(zamowieniaService));
            _isEditMode = false;
            _zamowienieId = 0;

            _zamowienie = new UpdateZamowienieDto
            {
                NumerZamowienia = string.Empty,
                UzytkownikId = 0,
                KlubId = null,
                Status = "Nowe",
                SumaBrutto = 0,
                KwotaRabatu = 0,
                Waluta = "PLN",
                MetodaPlatnosciId = null,
                RabatId = null,
                TypKanalu = "Panel",
                DataUtworzenia = DateTime.Now,
                DataOplacenia = null,
                UtworzylUzytkownikId = null
            };

            _ = LoadFormDataAsync();
        }

        public NoweZamowienieViewModel(ZamowieniaService zamowieniaService, ZamowienieDto dto)
        {
            DisplayName = "Edycja zamówienia";

            _zamowieniaService = zamowieniaService ?? throw new ArgumentNullException(nameof(zamowieniaService));
            _isEditMode = true;
            _zamowienieId = dto.ZamowienieId;

            _zamowienie = new UpdateZamowienieDto
            {
                NumerZamowienia = dto.NumerZamowienia ?? string.Empty,
                UzytkownikId = dto.UzytkownikId,
                KlubId = dto.KlubId,
                Status = dto.Status ?? "Nowe",
                SumaBrutto = dto.SumaBrutto,
                KwotaRabatu = dto.KwotaRabatu,
                Waluta = dto.Waluta ?? "PLN",
                MetodaPlatnosciId = dto.MetodaPlatnosciId,
                RabatId = dto.RabatId,
                TypKanalu = dto.TypKanalu ?? "Panel",
                DataUtworzenia = dto.DataUtworzenia,
                DataOplacenia = dto.DataOplacenia,
                UtworzylUzytkownikId = dto.UtworzylUzytkownikId
            };

            _ = LoadFormDataAsync();
        }

        public ObservableCollection<ComboBoxItem> Uzytkownicy
        {
            get => _uzytkownicy;
            set
            {
                _uzytkownicy = value;
                OnPropertyChanged(nameof(Uzytkownicy));
            }
        }

        public ObservableCollection<ComboBoxItem> Kluby
        {
            get => _kluby;
            set
            {
                _kluby = value;
                OnPropertyChanged(nameof(Kluby));
            }
        }

        public ObservableCollection<ComboBoxItem> MetodyPlatnosci
        {
            get => _metodyPlatnosci;
            set
            {
                _metodyPlatnosci = value;
                OnPropertyChanged(nameof(MetodyPlatnosci));
            }
        }

        public ObservableCollection<ComboBoxItem> Rabaty
        {
            get => _rabaty;
            set
            {
                _rabaty = value;
                OnPropertyChanged(nameof(Rabaty));
            }
        }

        public ObservableCollection<ComboBoxItem> UtworzylUzytkownicy
        {
            get => _utworzylUzytkownicy;
            set
            {
                _utworzylUzytkownicy = value;
                OnPropertyChanged(nameof(UtworzylUzytkownicy));
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

        public string SelectedKlubNazwa
        {
            get => _selectedKlubNazwa;
            set
            {
                _selectedKlubNazwa = value;
                OnPropertyChanged(nameof(SelectedKlubNazwa));
            }
        }

        public string SelectedUtworzylUzytkownikNazwa
        {
            get => _selectedUtworzylUzytkownikNazwa;
            set
            {
                _selectedUtworzylUzytkownikNazwa = value;
                OnPropertyChanged(nameof(SelectedUtworzylUzytkownikNazwa));
            }
        }

        public string NumerZamowienia
        {
            get => _zamowienie.NumerZamowienia;
            set
            {
                _zamowienie.NumerZamowienia = value;
                OnPropertyChanged(nameof(NumerZamowienia));
            }
        }

        public int UzytkownikId
        {
            get => _zamowienie.UzytkownikId;
            set
            {
                _zamowienie.UzytkownikId = value;
                OnPropertyChanged(nameof(UzytkownikId));

                var selected = Uzytkownicy.FirstOrDefault(x => x.Id == value);
                SelectedUzytkownikNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public int? KlubId
        {
            get => _zamowienie.KlubId ?? 0;
            set
            {
                _zamowienie.KlubId = value == null || value == 0 ? null : value;
                OnPropertyChanged(nameof(KlubId));

                var selected = Kluby.FirstOrDefault(x => x.Id == value);
                SelectedKlubNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public string Status
        {
            get => _zamowienie.Status;
            set
            {
                _zamowienie.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public decimal SumaBrutto
        {
            get => _zamowienie.SumaBrutto;
            set
            {
                _zamowienie.SumaBrutto = value;
                OnPropertyChanged(nameof(SumaBrutto));
            }
        }

        public decimal KwotaRabatu
        {
            get => _zamowienie.KwotaRabatu;
            set
            {
                _zamowienie.KwotaRabatu = value;
                OnPropertyChanged(nameof(KwotaRabatu));
            }
        }

        public string Waluta
        {
            get => _zamowienie.Waluta;
            set
            {
                _zamowienie.Waluta = value;
                OnPropertyChanged(nameof(Waluta));
            }
        }

        public int? MetodaPlatnosciId
        {
            get => _zamowienie.MetodaPlatnosciId ?? 0;
            set
            {
                _zamowienie.MetodaPlatnosciId = value == null || value == 0 ? null : value;
                OnPropertyChanged(nameof(MetodaPlatnosciId));
            }
        }

        public int? RabatId
        {
            get => _zamowienie.RabatId ?? 0;
            set
            {
                _zamowienie.RabatId = value == null || value == 0 ? null : value;
                OnPropertyChanged(nameof(RabatId));
            }
        }

        public string TypKanalu
        {
            get => _zamowienie.TypKanalu;
            set
            {
                _zamowienie.TypKanalu = value;
                OnPropertyChanged(nameof(TypKanalu));
            }
        }

        public int? UtworzylUzytkownikId
        {
            get => _zamowienie.UtworzylUzytkownikId ?? 0;
            set
            {
                _zamowienie.UtworzylUzytkownikId = value == null || value == 0 ? null : value;
                OnPropertyChanged(nameof(UtworzylUzytkownikId));

                var selected = UtworzylUzytkownicy.FirstOrDefault(x => x.Id == value);
                SelectedUtworzylUzytkownikNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public DateTime DataUtworzenia
        {
            get => _zamowienie.DataUtworzenia;
            set
            {
                _zamowienie.DataUtworzenia = value;
                OnPropertyChanged(nameof(DataUtworzenia));
                OnPropertyChanged(nameof(DataUtworzeniaDate));
            }
        }

        public DateTime? DataOplacenia
        {
            get => _zamowienie.DataOplacenia;
            set
            {
                _zamowienie.DataOplacenia = value;
                OnPropertyChanged(nameof(DataOplacenia));
                OnPropertyChanged(nameof(DataOplaceniaDate));
            }
        }

        public DateTime? DataUtworzeniaDate
        {
            get => DataUtworzenia;
            set
            {
                if (value.HasValue)
                {
                    DataUtworzenia = value.Value;
                    OnPropertyChanged(nameof(DataUtworzeniaDate));
                }
            }
        }

        public DateTime? DataOplaceniaDate
        {
            get => DataOplacenia;
            set
            {
                DataOplacenia = value;
                OnPropertyChanged(nameof(DataOplaceniaDate));
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

        private BaseCommand? _wybierzUtworzylUzytkownikaCommand;
        public ICommand WybierzUtworzylUzytkownikaCommand =>
            _wybierzUtworzylUzytkownikaCommand ??= new BaseCommand(WybierzUtworzylUzytkownika);

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
                .Where(x => x.Id > 0)
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

        private void WybierzUtworzylUzytkownika()
        {
            var users = UtworzylUzytkownicy
                .Where(x => x.Id > 0)
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
                UtworzylUzytkownikId = vm.SelectedUser.Id;
                SelectedUtworzylUzytkownikNazwa = vm.SelectedUser.Imie;
            }
        }

        private ObservableCollection<ComboBoxItem> MapToComboBoxItems(System.Collections.Generic.IEnumerable<LookupItemDto> items)
        {
            return new ObservableCollection<ComboBoxItem>(
                items.Select(x => new ComboBoxItem
                {
                    Id = x.Id,
                    Nazwa = x.Nazwa
                }));
        }

        private async Task LoadFormDataAsync()
        {
            try
            {
                var data = await _zamowieniaService.GetFormDataAsync();

                if (data == null)
                    return;

                Uzytkownicy = MapToComboBoxItems(data.Uzytkownicy);

                Kluby = MapToComboBoxItems(data.Kluby);
                Kluby.Insert(0, new ComboBoxItem { Id = 0, Nazwa = "-- brak --" });

                MetodyPlatnosci = MapToComboBoxItems(data.MetodyPlatnosci);
                MetodyPlatnosci.Insert(0, new ComboBoxItem { Id = 0, Nazwa = "-- brak --" });

                Rabaty = MapToComboBoxItems(data.Rabaty);
                Rabaty.Insert(0, new ComboBoxItem { Id = 0, Nazwa = "-- brak --" });

                UtworzylUzytkownicy = MapToComboBoxItems(data.UtworzylUzytkownicy);
                UtworzylUzytkownicy.Insert(0, new ComboBoxItem { Id = 0, Nazwa = "-- brak --" });

                OnPropertyChanged(nameof(Uzytkownicy));
                OnPropertyChanged(nameof(Kluby));
                OnPropertyChanged(nameof(MetodyPlatnosci));
                OnPropertyChanged(nameof(Rabaty));
                OnPropertyChanged(nameof(UtworzylUzytkownicy));

                SelectedUzytkownikNazwa =
                    Uzytkownicy.FirstOrDefault(x => x.Id == UzytkownikId)?.Nazwa ?? string.Empty;

                SelectedKlubNazwa =
                    Kluby.FirstOrDefault(x => x.Id == KlubId)?.Nazwa ?? string.Empty;

                SelectedUtworzylUzytkownikNazwa =
                    UtworzylUzytkownicy.FirstOrDefault(x => x.Id == UtworzylUzytkownikId)?.Nazwa ?? string.Empty;

                OnPropertyChanged(nameof(UzytkownikId));
                OnPropertyChanged(nameof(KlubId));
                OnPropertyChanged(nameof(MetodaPlatnosciId));
                OnPropertyChanged(nameof(RabatId));
                OnPropertyChanged(nameof(UtworzylUzytkownikId));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Waluta));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się załadować danych formularza: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(NumerZamowienia))
            {
                message = "Numer zamówienia jest wymagany.";
                return false;
            }

            if (UzytkownikId <= 0)
            {
                message = "Musisz wybrać użytkownika.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                message = "Status jest wymagany.";
                return false;
            }

            if (SumaBrutto < 0)
            {
                message = "Suma brutto nie może być ujemna.";
                return false;
            }

            if (KwotaRabatu < 0)
            {
                message = "Kwota rabatu nie może być ujemna.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Waluta) || Waluta.Trim().Length != 3)
            {
                message = "Waluta musi mieć dokładnie 3 znaki.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(TypKanalu))
            {
                message = "Typ kanału jest wymagany.";
                return false;
            }

            if (DataOplacenia.HasValue && DataOplacenia < DataUtworzenia)
            {
                message = "Data opłacenia nie może być wcześniejsza niż data utworzenia.";
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
                    success = await _zamowieniaService.UpdateAsync(_zamowienieId, _zamowienie);
                }
                else
                {
                    var createDto = new CreateZamowienieDto
                    {
                        NumerZamowienia = _zamowienie.NumerZamowienia,
                        UzytkownikId = _zamowienie.UzytkownikId,
                        KlubId = _zamowienie.KlubId,
                        Status = _zamowienie.Status,
                        SumaBrutto = _zamowienie.SumaBrutto,
                        KwotaRabatu = _zamowienie.KwotaRabatu,
                        Waluta = _zamowienie.Waluta,
                        MetodaPlatnosciId = _zamowienie.MetodaPlatnosciId,
                        RabatId = _zamowienie.RabatId,
                        TypKanalu = _zamowienie.TypKanalu,
                        DataUtworzenia = _zamowienie.DataUtworzenia,
                        DataOplacenia = _zamowienie.DataOplacenia,
                        UtworzylUzytkownikId = _zamowienie.UtworzylUzytkownikId
                    };

                    success = await _zamowieniaService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać zamówienia.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                ZamowienieSaved?.Invoke(this, EventArgs.Empty);
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