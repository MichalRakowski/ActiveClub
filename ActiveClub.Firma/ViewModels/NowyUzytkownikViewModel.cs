using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Models;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using ActiveClub.Shared.DTOs.Stanowiska;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyUzytkownikViewModel : WorkspaceViewModel
    {
        public event EventHandler? UserSaved;

        private readonly UzytkownicyService _uzytkownicyService;
        private readonly KlubyService _klubyService;
        private readonly TrenerzyService _trenerzyService;
        private readonly StanowiskaService _stanowiskaService;
        private readonly CzlonkostwaUzytkownikaService _czlonkostwaService;
        private readonly HarmonogramZajecService _harmonogramService;
        private readonly ZamowieniaService _zamowieniaService;
        private readonly PlatnosciService _platnosciService;
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly RodzajeCzlonkostwaService _rodzajeCzlonkostwaService;
        private readonly SaleService _saleService;
        private readonly Action<WorkspaceViewModel>? _openWorkspace;

        private readonly UpdateUzytkownikDto _uzytkownik;
        private readonly bool _isEditMode;
        private readonly int _id;

        private string _haslo = string.Empty;
        private string _selectedRola = "Klient";
        private int? _klubId;
        private int? _stanowiskoId;
        private int? _selectedCzlonkostwoId;

        private string _selectedKlubNazwa = string.Empty;
        private string _selectedTrenerNazwa = string.Empty;
        private string _selectedHarmonogramNazwa = "Brak";
        private string _zamowieniaPodsumowanie = "Brak danych";
        private string _platnosciPodsumowanie = "Brak danych";

        public ObservableCollection<StanowiskoDto> Stanowiska { get; set; } = new();
        public ObservableCollection<ComboBoxItem> Czlonkostwa { get; set; } = new();

        public NowyUzytkownikViewModel(
    UzytkownicyService uzytkownicyService,
    KlubyService klubyService,
    TrenerzyService trenerzyService,
    StanowiskaService stanowiskaService,
    CzlonkostwaUzytkownikaService czlonkostwaService,
    RodzajeCzlonkostwaService rodzajeCzlonkostwaService,
    HarmonogramZajecService harmonogramService,
    ZamowieniaService zamowieniaService,
    PlatnosciService platnosciService,
    RodzajeZajecService rodzajeZajecService,
    SaleService saleService,
    Action<WorkspaceViewModel>? openWorkspace = null)
        {
            DisplayName = "Nowy użytkownik";

            _uzytkownicyService = uzytkownicyService;
            _rodzajeCzlonkostwaService = rodzajeCzlonkostwaService;
            _klubyService = klubyService;
            _trenerzyService = trenerzyService;
            _stanowiskaService = stanowiskaService;
            _czlonkostwaService = czlonkostwaService;
            _harmonogramService = harmonogramService;
            _zamowieniaService = zamowieniaService;
            _platnosciService = platnosciService;
            _rodzajeZajecService = rodzajeZajecService;
            _saleService = saleService;
            _openWorkspace = openWorkspace;

            _isEditMode = false;
            _id = 0;

            AvailableRoles = new List<string>
            {
                "Klient",
                "Pracownik",
                "Trener",
                "Admin"
            };

            _uzytkownik = new UpdateUzytkownikDto
            {
                CzyAktywny = true,
                Rola = "Klient"
            };

            _ = LoadStanowiskaAsync();
            _ = LoadCzlonkostwaAsync();
        }

        public NowyUzytkownikViewModel(
     UzytkownicyService uzytkownicyService,
     KlubyService klubyService,
     TrenerzyService trenerzyService,
     StanowiskaService stanowiskaService,
     CzlonkostwaUzytkownikaService czlonkostwaService,
     RodzajeCzlonkostwaService rodzajeCzlonkostwaService,
     HarmonogramZajecService harmonogramService,
     ZamowieniaService zamowieniaService,
     PlatnosciService platnosciService,
     RodzajeZajecService rodzajeZajecService,
     SaleService saleService,
     UzytkownikDto dto,
     Action<WorkspaceViewModel>? openWorkspace = null)
        {
            DisplayName = "Edycja użytkownika";

            _uzytkownicyService = uzytkownicyService;
            _klubyService = klubyService;
            _trenerzyService = trenerzyService;
            _stanowiskaService = stanowiskaService;
            _czlonkostwaService = czlonkostwaService;
            _rodzajeCzlonkostwaService = rodzajeCzlonkostwaService;
            _harmonogramService = harmonogramService;
            _zamowieniaService = zamowieniaService;
            _platnosciService = platnosciService;
            _rodzajeZajecService = rodzajeZajecService;
            _saleService = saleService;
            _openWorkspace = openWorkspace;

            _isEditMode = true;
            _id = dto.Id;

            AvailableRoles = new List<string>
    {
        "Klient",
        "Pracownik",
        "Trener",
        "Admin"
    };

            var rola = string.IsNullOrWhiteSpace(dto.Rola) ? "Klient" : dto.Rola;

            _uzytkownik = new UpdateUzytkownikDto
            {
                Imie = dto.Imie,
                Nazwisko = dto.Nazwisko,
                Email = dto.Email,
                Telefon = dto.Telefon,
                CzyAktywny = dto.CzyAktywny,
                KlubDomyslnyId = dto.KlubDomyslnyId,
                Uwagi = dto.Uwagi,
                Rola = rola,
                KlubId = dto.KlubId,
                StanowiskoId = dto.StanowiskoId
            };

            _selectedRola = rola;
            _klubId = dto.KlubId;
            _stanowiskoId = dto.StanowiskoId;

            _ = LoadStanowiskaAsync();
            _ = LoadEditDataAsync();

            if (dto.KlubId.HasValue)
                _ = LoadSelectedKlubNameAsync(dto.KlubId.Value);
        }

        public List<string> AvailableRoles { get; }

        public string SelectedRola
        {
            get => _selectedRola;
            set
            {
                var newValue = string.IsNullOrWhiteSpace(value) ? "Klient" : value;

                _selectedRola = newValue;
                _uzytkownik.Rola = newValue;

                OnPropertyChanged(nameof(SelectedRola));
                OnPropertyChanged(nameof(RequiresEmployeeData));

                if (!RequiresEmployeeData)
                {
                    KlubId = null;
                    StanowiskoId = null;
                    SelectedKlubNazwa = string.Empty;
                    SelectedTrenerNazwa = string.Empty;
                }
            }
        }

        public bool RequiresEmployeeData =>
            SelectedRola == "Pracownik" ||
            SelectedRola == "Trener" ||
            SelectedRola == "Admin";

        public string Haslo
        {
            get => _haslo;
            set
            {
                _haslo = value ?? string.Empty;
                OnPropertyChanged(nameof(Haslo));
            }
        }

        public int? KlubId
        {
            get => _klubId;
            set
            {
                _klubId = value;
                _uzytkownik.KlubId = value;
                OnPropertyChanged(nameof(KlubId));
            }
        }

        public int? StanowiskoId
        {
            get => _stanowiskoId;
            set
            {
                _stanowiskoId = value;
                _uzytkownik.StanowiskoId = value;
                OnPropertyChanged(nameof(StanowiskoId));
            }
        }

        public int? SelectedCzlonkostwoId
        {
            get => _selectedCzlonkostwoId;
            set
            {
                _selectedCzlonkostwoId = value;
                OnPropertyChanged(nameof(SelectedCzlonkostwoId));
            }
        }

        public string Imie
        {
            get => _uzytkownik.Imie;
            set
            {
                _uzytkownik.Imie = value ?? string.Empty;
                OnPropertyChanged(nameof(Imie));
            }
        }

        public string Nazwisko
        {
            get => _uzytkownik.Nazwisko;
            set
            {
                _uzytkownik.Nazwisko = value ?? string.Empty;
                OnPropertyChanged(nameof(Nazwisko));
            }
        }

        public string Email
        {
            get => _uzytkownik.Email;
            set
            {
                _uzytkownik.Email = value ?? string.Empty;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string? Telefon
        {
            get => _uzytkownik.Telefon;
            set
            {
                _uzytkownik.Telefon = value;
                OnPropertyChanged(nameof(Telefon));
            }
        }

        public bool CzyAktywny
        {
            get => _uzytkownik.CzyAktywny;
            set
            {
                _uzytkownik.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }

        public int? KlubDomyslnyId
        {
            get => _uzytkownik.KlubDomyslnyId;
            set
            {
                _uzytkownik.KlubDomyslnyId = value;
                OnPropertyChanged(nameof(KlubDomyslnyId));
            }
        }

        public string? Uwagi
        {
            get => _uzytkownik.Uwagi;
            set
            {
                _uzytkownik.Uwagi = value;
                OnPropertyChanged(nameof(Uwagi));
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

        public string SelectedTrenerNazwa
        {
            get => _selectedTrenerNazwa;
            set
            {
                _selectedTrenerNazwa = value;
                OnPropertyChanged(nameof(SelectedTrenerNazwa));
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

        public string ZamowieniaPodsumowanie
        {
            get => _zamowieniaPodsumowanie;
            set
            {
                _zamowieniaPodsumowanie = value;
                OnPropertyChanged(nameof(ZamowieniaPodsumowanie));
            }
        }

        public string PlatnosciPodsumowanie
        {
            get => _platnosciPodsumowanie;
            set
            {
                _platnosciPodsumowanie = value;
                OnPropertyChanged(nameof(PlatnosciPodsumowanie));
            }
        }

        public string SaveButtonText => _isEditMode ? "Zapisz zmiany" : "Dodaj";

        public bool IsCreateMode => !_isEditMode;
        public bool IsEditMode => _isEditMode;

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(() => _ = WybierzKlub());

        private BaseCommand? _wybierzTreneraCommand;
        public ICommand WybierzTreneraCommand =>
            _wybierzTreneraCommand ??= new BaseCommand(() => _ = WybierzTrenera());

        private BaseCommand? _wybierzHarmonogramCommand;
        public ICommand WybierzHarmonogramCommand =>
            _wybierzHarmonogramCommand ??= new BaseCommand(OtworzHarmonogram);

        private BaseCommand? _wybierzCzlonkostwoCommand;
        public ICommand WybierzCzlonkostwoCommand =>
            _wybierzCzlonkostwoCommand ??= new BaseCommand(OtworzCzlonkostwo);

        private BaseCommand? _pokazHistorieZamowienCommand;
        public ICommand PokazHistorieZamowienCommand =>
            _pokazHistorieZamowienCommand ??= new BaseCommand(PokazZamowienia);

        private BaseCommand? _pokazHistoriePlatnosciCommand;
        public ICommand PokazHistoriePlatnosciCommand =>
            _pokazHistoriePlatnosciCommand ??= new BaseCommand(PokazPlatnosci);

        private async Task LoadStanowiskaAsync()
        {
            try
            {
                var list = await _stanowiskaService.GetAllAsync();

                Stanowiska = new ObservableCollection<StanowiskoDto>(
                    list.Where(x => x.CzyAktywne));

                OnPropertyChanged(nameof(Stanowiska));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd ładowania stanowisk: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task LoadCzlonkostwaAsync()
        {
            try
            {
                var list = await _rodzajeCzlonkostwaService.GetAllAsync();

                Czlonkostwa = new ObservableCollection<ComboBoxItem>(
                    list
                        .Where(x => x.CzyAktywny)
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new ComboBoxItem
                        {
                            Id = x.RodzajCzlonkostwaId,
                            Nazwa = x.Nazwa
                        }));

                if (!_isEditMode)
                {
                    SelectedCzlonkostwoId = Czlonkostwa.FirstOrDefault()?.Id;
                }

                OnPropertyChanged(nameof(Czlonkostwa));
            }
            catch
            {
                Czlonkostwa = new ObservableCollection<ComboBoxItem>();
                OnPropertyChanged(nameof(Czlonkostwa));
            }
        }
        private async Task LoadEditDataAsync()
        {
            await LoadCzlonkostwaAsync();
            await LoadUserContextAsync();
        }
        private async Task LoadUserContextAsync()
        {
            if (!_isEditMode || _id <= 0)
                return;

            try
            {
                var memberships = await _czlonkostwaService.GetAllAsync();
                var userMemberships = memberships.Where(x => x.UzytkownikId == _id).ToList();

                if (userMemberships.Any())
                {
                    var active = userMemberships
                        .OrderByDescending(x => x.CzlonkostwoId)
                        .First();

                    SelectedCzlonkostwoId = active.RodzajCzlonkostwaId;
                }

                var orders = await _zamowieniaService.GetAllAsync();
                var userOrders = orders.Where(x => x.UzytkownikId == _id).ToList();

                ZamowieniaPodsumowanie = userOrders.Any()
                    ? $"{userOrders.Count} zamówień, suma: {userOrders.Sum(x => x.SumaBrutto):0.00}"
                    : "Brak zamówień";

                var payments = await _platnosciService.GetAllAsync();
                var orderIds = userOrders.Select(x => x.ZamowienieId).ToHashSet();

                var userPayments = payments
                    .Where(x => orderIds.Contains(x.ZamowienieId))
                    .ToList();

                PlatnosciPodsumowanie = userPayments.Any()
                    ? $"{userPayments.Count} płatności, suma: {userPayments.Sum(x => x.Kwota):0.00}"
                    : "Brak płatności";
            }
            catch
            {
                ZamowieniaPodsumowanie = "Brak danych";
                PlatnosciPodsumowanie = "Brak danych";
            }
        }

        private async Task WybierzKlub()
        {
            try
            {
                var kluby = await _klubyService.GetAllAsync();

                var vm = new WybierzKlubViewModel(kluby);

                var window = new WybierzKlubWindow(vm)
                {
                    Owner = Application.Current.MainWindow
                };

                if (window.ShowDialog() == true && vm.SelectedItem != null)
                {
                    KlubId = vm.SelectedItem.KlubId;
                    KlubDomyslnyId = vm.SelectedItem.KlubId;
                    SelectedKlubNazwa = vm.SelectedItem.Nazwa;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd wyboru klubu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task WybierzTrenera()
        {
            try
            {
                var trenerzy = await _trenerzyService.GetAllAsync();

                var vm = new WybierzTreneraViewModel(trenerzy);

                var window = new WybierzTreneraWindow(vm)
                {
                    Owner = Application.Current.MainWindow
                };

                if (window.ShowDialog() == true && vm.SelectedItem != null)
                {
                    SelectedTrenerNazwa = vm.SelectedItem.ImieNazwisko;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd wyboru trenera",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task LoadSelectedKlubNameAsync(int klubId)
        {
            try
            {
                var kluby = await _klubyService.GetAllAsync();
                var klub = kluby.FirstOrDefault(k => k.KlubId == klubId);

                SelectedKlubNazwa = klub?.Nazwa ?? string.Empty;
            }
            catch
            {
                SelectedKlubNazwa = string.Empty;
            }
        }

        private bool EnsureUserSaved()
        {
            if (!_isEditMode || _id <= 0)
            {
                MessageBox.Show(
                    "Najpierw zapisz użytkownika. Dopiero potem możesz zarządzać karnetem, harmonogramem, zamówieniami i płatnościami.",
                    "Użytkownik nie jest zapisany",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return false;
            }

            if (_openWorkspace == null)
            {
                MessageBox.Show(
                    "Nie można otworzyć dodatkowego widoku z tego miejsca.",
                    "Brak kontekstu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private void OtworzCzlonkostwo()
        {
            if (!EnsureUserSaved())
                return;

            var vm = new NoweCzlonkostwoUzytkownikaViewModel(_czlonkostwaService)
            {
                UzytkownikId = _id,
                KlubId = KlubId
            };

            vm.Saved += async (_, _) =>
            {
                await LoadCzlonkostwaAsync();
                await LoadUserContextAsync();
            };

            _openWorkspace?.Invoke(vm);
        }

        private void OtworzHarmonogram()
        {
            if (!EnsureUserSaved())
                return;

            var vm = new NowyHarmonogramZajecViewModel(
                _harmonogramService,
                _klubyService,
                _rodzajeZajecService,
                _saleService,
                _trenerzyService,
                _uzytkownicyService);

            if (KlubId.HasValue)
                vm.KlubId = KlubId.Value;

            vm.UtworzylUzytkownikId = _id;

            vm.HarmonogramSaved += (_, _) =>
            {
                SelectedHarmonogramNazwa = "Dodano / edytowano harmonogram";
            };

            _openWorkspace?.Invoke(vm);
        }

        private void PokazZamowienia()
        {
            if (!EnsureUserSaved())
                return;

            var vm = new WszystkieZamowieniaViewModel(_zamowieniaService, _openWorkspace!);
            vm.FilterByUzytkownikId(_id);

            _openWorkspace?.Invoke(vm);
        }

        private void PokazPlatnosci()
        {
            if (!EnsureUserSaved())
                return;

            var vm = new WszystkiePlatnosciViewModel(
                _platnosciService,
                _openWorkspace!,
                _zamowieniaService);

            vm.FilterByUzytkownikId(_id);

            _openWorkspace?.Invoke(vm);
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Imie))
            {
                message = "Imię jest wymagane.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Nazwisko))
            {
                message = "Nazwisko jest wymagane.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                message = "Email jest wymagany.";
                return false;
            }

            if (!Email.Contains("@") || !Email.Contains("."))
            {
                message = "Podaj poprawny adres email.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedRola))
            {
                message = "Musisz wybrać rolę.";
                return false;
            }

            if (!_isEditMode && string.IsNullOrWhiteSpace(Haslo))
            {
                message = "Hasło jest wymagane przy tworzeniu użytkownika.";
                return false;
            }

            if (RequiresEmployeeData)
            {
                if (!KlubId.HasValue)
                {
                    message = "Dla tej roli musisz wybrać klub.";
                    return false;
                }

                if (!StanowiskoId.HasValue)
                {
                    message = "Dla tej roli musisz wybrać stanowisko.";
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private CreateUzytkownikDto BuildCreateDto()
        {
            return new CreateUzytkownikDto
            {
                Imie = Imie.Trim(),
                Nazwisko = Nazwisko.Trim(),
                Email = Email.Trim(),
                Telefon = string.IsNullOrWhiteSpace(Telefon) ? null : Telefon.Trim(),
                Haslo = Haslo,
                CzyAktywny = CzyAktywny,
                KlubDomyslnyId = KlubDomyslnyId,
                Uwagi = string.IsNullOrWhiteSpace(Uwagi) ? null : Uwagi.Trim(),
                Rola = SelectedRola,
                KlubId = RequiresEmployeeData ? KlubId : null,
                StanowiskoId = RequiresEmployeeData ? StanowiskoId : null
            };
        }

        private void PrepareUpdateDto()
        {
            _uzytkownik.Imie = Imie.Trim();
            _uzytkownik.Nazwisko = Nazwisko.Trim();
            _uzytkownik.Email = Email.Trim();
            _uzytkownik.Telefon = string.IsNullOrWhiteSpace(Telefon) ? null : Telefon.Trim();
            _uzytkownik.Uwagi = string.IsNullOrWhiteSpace(Uwagi) ? null : Uwagi.Trim();
            _uzytkownik.Rola = SelectedRola;
            _uzytkownik.KlubDomyslnyId = KlubDomyslnyId;
            _uzytkownik.KlubId = RequiresEmployeeData ? KlubId : null;
            _uzytkownik.StanowiskoId = RequiresEmployeeData ? StanowiskoId : null;
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
                bool success;

                if (_isEditMode)
                {
                    PrepareUpdateDto();
                    success = await _uzytkownicyService.UpdateAsync(_id, _uzytkownik);
                }
                else
                {
                    success = await _uzytkownicyService.CreateAsync(BuildCreateDto());
                }

                if (!success)
                {
                    MessageBox.Show(
                        _isEditMode
                            ? "Nie udało się zapisać zmian użytkownika."
                            : "Nie udało się zapisać użytkownika.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                if (_isEditMode && SelectedCzlonkostwoId.HasValue)
                {
                    var memberships = await _czlonkostwaService.GetAllAsync();

                    var activeMembership = memberships
                        .Where(x => x.UzytkownikId == _id)
                        .OrderByDescending(x => x.CzlonkostwoId)
                        .FirstOrDefault();

                    if (activeMembership != null)
                    {
                        var updateDto = new UpdateCzlonkostwoUzytkownikaDto
                        {
                            UzytkownikId = _id,
                            OfertaKlubuCzlonkostwoId = activeMembership.OfertaKlubuCzlonkostwoId,
                            RodzajCzlonkostwaId = SelectedCzlonkostwoId.Value,
                            KlubId = activeMembership.KlubId,
                            StartAt = activeMembership.StartAt,
                            EndAt = activeMembership.EndAt,
                            Status = activeMembership.Status,
                            CenaZakupu = activeMembership.CenaZakupu,
                            ZamowienieId = activeMembership.ZamowienieId,
                            NadanePrzezUzytkownikId = activeMembership.NadanePrzezUzytkownikId,
                            DataUtworzenia = activeMembership.DataUtworzenia,
                            DataAktywacji = activeMembership.DataAktywacji,
                            DataAnulowania = activeMembership.DataAnulowania,
                            PowodAnulowania = activeMembership.PowodAnulowania
                        };

                        await _czlonkostwaService.UpdateAsync(activeMembership.CzlonkostwoId, updateDto);
                    }
                    else
                    {
                        var createDto = new CreateCzlonkostwoUzytkownikaDto
                        {
                            UzytkownikId = _id,
                            RodzajCzlonkostwaId = SelectedCzlonkostwoId.Value,
                            KlubId = KlubId ?? KlubDomyslnyId,
                            StartAt = DateTime.Today,
                            Status = "AKTYWNE",
                            DataUtworzenia = DateTime.Now,
                            DataAktywacji = DateTime.Now
                        };

                        await _czlonkostwaService.CreateAsync(createDto);
                    }
                }

                UserSaved?.Invoke(this, EventArgs.Empty);
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