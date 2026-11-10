using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using ActiveClub.Shared.DTOs.Kluby;
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
    public class NoweCzlonkostwoUzytkownikaViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly CzlonkostwaUzytkownikaService _service;
        private readonly UpdateCzlonkostwoUzytkownikaDto _dto;
        private readonly bool _isEditMode;
        private readonly int _id;

        private string _selectedUzytkownikNazwa = string.Empty;
        private string _selectedKlubNazwa = string.Empty;

        public ObservableCollection<LookupItemDto> Uzytkownicy { get; set; } = new();
        public ObservableCollection<LookupItemDto> OfertyKlubuCzlonkostwa { get; set; } = new();
        public ObservableCollection<LookupItemDto> RodzajeCzlonkostwa { get; set; } = new();
        public ObservableCollection<LookupItemDto> Kluby { get; set; } = new();
        public ObservableCollection<LookupItemDto> Zamowienia { get; set; } = new();
        public ObservableCollection<LookupItemDto> NadajacyUzytkownicy { get; set; } = new();
        public ObservableCollection<LookupItemDto> Statusy { get; set; } = new();

        public string SaveButtonText => _isEditMode ? "Zapisz zmiany" : "Dodaj";
        public bool IsEditMode => _isEditMode;
        public bool IsCreateMode => !_isEditMode;

        public NoweCzlonkostwoUzytkownikaViewModel(CzlonkostwaUzytkownikaService service)
        {
            DisplayName = "Nowe członkostwo użytkownika";
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = false;
            _id = 0;

            _dto = new UpdateCzlonkostwoUzytkownikaDto
            {
                UzytkownikId = 0,
                OfertaKlubuCzlonkostwoId = null,
                RodzajCzlonkostwaId = null,
                KlubId = null,
                StartAt = null,
                EndAt = null,
                Status = "AKTYWNE",
                CenaZakupu = null,
                ZamowienieId = null,
                NadanePrzezUzytkownikId = null,
                DataUtworzenia = DateTime.Now,
                DataAktywacji = null,
                DataAnulowania = null,
                PowodAnulowania = null
            };

            _ = LoadFormDataAsync();
        }

        public NoweCzlonkostwoUzytkownikaViewModel(
            CzlonkostwaUzytkownikaService service,
            CzlonkostwoUzytkownikaDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            DisplayName = "Edycja członkostwa użytkownika";
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _isEditMode = true;
            _id = dto.CzlonkostwoId;

            _dto = new UpdateCzlonkostwoUzytkownikaDto
            {
                UzytkownikId = dto.UzytkownikId,
                OfertaKlubuCzlonkostwoId = dto.OfertaKlubuCzlonkostwoId,
                RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId,
                KlubId = dto.KlubId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status switch
                {
                    "NOWE" => "Nowe",
                    "AKTYWNE" => "Aktywne",
                    "ANULOWANE" => "Anulowane",
                    "WYGASLE" => "Wygasłe",
                    _ => dto.Status
                },
                CenaZakupu = dto.CenaZakupu,
                ZamowienieId = dto.ZamowienieId,
                NadanePrzezUzytkownikId = dto.NadanePrzezUzytkownikId,
                DataUtworzenia = dto.DataUtworzenia,
                DataAktywacji = dto.DataAktywacji,
                DataAnulowania = dto.DataAnulowania,
                PowodAnulowania = dto.PowodAnulowania
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
            get => _dto.UzytkownikId;
            set
            {
                _dto.UzytkownikId = value;
                OnPropertyChanged(nameof(UzytkownikId));

                var selected = Uzytkownicy.FirstOrDefault(x => x.Id == value);
                SelectedUzytkownikNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public int? OfertaKlubuCzlonkostwoId
        {
            get => _dto.OfertaKlubuCzlonkostwoId;
            set
            {
                _dto.OfertaKlubuCzlonkostwoId = value;
                OnPropertyChanged(nameof(OfertaKlubuCzlonkostwoId));
            }
        }

        public int? RodzajCzlonkostwaId
        {
            get => _dto.RodzajCzlonkostwaId;
            set
            {
                _dto.RodzajCzlonkostwaId = value;
                OnPropertyChanged(nameof(RodzajCzlonkostwaId));
            }
        }

        public int? KlubId
        {
            get => _dto.KlubId;
            set
            {
                var changed = _dto.KlubId != value;

                _dto.KlubId = value;
                OnPropertyChanged(nameof(KlubId));

                var selected = Kluby.FirstOrDefault(x => x.Id == value);
                SelectedKlubNazwa = selected?.Nazwa ?? string.Empty;

                if (changed)
                    _ = LoadNadajacyUzytkownicyAsync();
            }
        }

        public DateTime? StartAt
        {
            get => _dto.StartAt;
            set
            {
                _dto.StartAt = value;
                OnPropertyChanged(nameof(StartAt));
            }
        }

        public DateTime? EndAt
        {
            get => _dto.EndAt;
            set
            {
                _dto.EndAt = value;
                OnPropertyChanged(nameof(EndAt));
            }
        }

        public string? Status
        {
            get => _dto.Status;
            set
            {
                _dto.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public decimal? CenaZakupu
        {
            get => _dto.CenaZakupu;
            set
            {
                _dto.CenaZakupu = value;
                OnPropertyChanged(nameof(CenaZakupu));
            }
        }

        public int? ZamowienieId
        {
            get => _dto.ZamowienieId;
            set
            {
                _dto.ZamowienieId = value;
                OnPropertyChanged(nameof(ZamowienieId));
            }
        }

        public int? NadanePrzezUzytkownikId
        {
            get => _dto.NadanePrzezUzytkownikId;
            set
            {
                _dto.NadanePrzezUzytkownikId = value;
                OnPropertyChanged(nameof(NadanePrzezUzytkownikId));
            }
        }

        public DateTime? DataUtworzenia
        {
            get => _dto.DataUtworzenia;
            set
            {
                _dto.DataUtworzenia = value;
                OnPropertyChanged(nameof(DataUtworzenia));
            }
        }

        public DateTime? DataAktywacji
        {
            get => _dto.DataAktywacji;
            set
            {
                _dto.DataAktywacji = value;
                OnPropertyChanged(nameof(DataAktywacji));
            }
        }

        public DateTime? DataAnulowania
        {
            get => _dto.DataAnulowania;
            set
            {
                _dto.DataAnulowania = value;
                OnPropertyChanged(nameof(DataAnulowania));
            }
        }

        public string? PowodAnulowania
        {
            get => _dto.PowodAnulowania;
            set
            {
                _dto.PowodAnulowania = value;
                OnPropertyChanged(nameof(PowodAnulowania));
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

                OfertyKlubuCzlonkostwa = new ObservableCollection<LookupItemDto>(
                    data.OfertyKlubuCzlonkostwa ?? Enumerable.Empty<LookupItemDto>());

                RodzajeCzlonkostwa = new ObservableCollection<LookupItemDto>(
                    data.RodzajeCzlonkostwa ?? Enumerable.Empty<LookupItemDto>());

                Kluby = new ObservableCollection<LookupItemDto>(
                    data.Kluby ?? Enumerable.Empty<LookupItemDto>());

                Zamowienia = new ObservableCollection<LookupItemDto>(
                    data.Zamowienia ?? Enumerable.Empty<LookupItemDto>());

                NadajacyUzytkownicy = new ObservableCollection<LookupItemDto>();

                Statusy = new ObservableCollection<LookupItemDto>(
                    data.Statusy ?? Enumerable.Empty<LookupItemDto>());

                OnPropertyChanged(nameof(Uzytkownicy));
                OnPropertyChanged(nameof(OfertyKlubuCzlonkostwa));
                OnPropertyChanged(nameof(RodzajeCzlonkostwa));
                OnPropertyChanged(nameof(Kluby));
                OnPropertyChanged(nameof(Zamowienia));
                OnPropertyChanged(nameof(NadajacyUzytkownicy));
                OnPropertyChanged(nameof(Statusy));

                var selectedUser = Uzytkownicy.FirstOrDefault(x => x.Id == UzytkownikId);
                SelectedUzytkownikNazwa = selectedUser?.Nazwa ?? string.Empty;

                var selectedKlub = Kluby.FirstOrDefault(x => x.Id == KlubId);
                SelectedKlubNazwa = selectedKlub?.Nazwa ?? string.Empty;

                if (KlubId.HasValue && KlubId.Value > 0)
                    await LoadNadajacyUzytkownicyAsync();
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

        private async Task LoadNadajacyUzytkownicyAsync()
        {
            try
            {
                if (!KlubId.HasValue || KlubId.Value <= 0)
                {
                    NadajacyUzytkownicy = new ObservableCollection<LookupItemDto>();
                    NadanePrzezUzytkownikId = null;
                    OnPropertyChanged(nameof(NadajacyUzytkownicy));
                    return;
                }

                var pracownicy = await _service.GetPracownicyKlubuAsync(KlubId.Value);

                NadajacyUzytkownicy = new ObservableCollection<LookupItemDto>(
                    pracownicy ?? new List<LookupItemDto>());

                OnPropertyChanged(nameof(NadajacyUzytkownicy));

                if (NadanePrzezUzytkownikId.HasValue &&
                    !NadajacyUzytkownicy.Any(x => x.Id == NadanePrzezUzytkownikId.Value))
                {
                    NadanePrzezUzytkownikId = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd ładowania pracowników klubu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (UzytkownikId <= 0)
            {
                message = "Użytkownik jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                message = "Status jest wymagany.";
                return false;
            }
            Status = Status switch
            {
                "Nowe" => "NOWE",
                "Aktywne" => "AKTYWNE",
                "Anulowane" => "ANULOWANE",
                "Wygasłe" => "WYGASLE",
                _ => Status?.Trim().ToUpperInvariant()
            };
            if (StartAt.HasValue && EndAt.HasValue && StartAt > EndAt)
            {
                message = "Data rozpoczęcia nie może być większa niż data zakończenia.";
                return false;
            }

            if (CenaZakupu.HasValue && CenaZakupu < 0)
            {
                message = "Cena zakupu nie może być ujemna.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private CreateCzlonkostwoUzytkownikaDto BuildCreateDto()
        {
            return new CreateCzlonkostwoUzytkownikaDto
            {
                UzytkownikId = _dto.UzytkownikId,
                OfertaKlubuCzlonkostwoId = _dto.OfertaKlubuCzlonkostwoId,
                RodzajCzlonkostwaId = _dto.RodzajCzlonkostwaId,
                KlubId = _dto.KlubId,
                StartAt = _dto.StartAt,
                EndAt = _dto.EndAt,
                Status = _dto.Status,
                CenaZakupu = _dto.CenaZakupu,
                ZamowienieId = _dto.ZamowienieId,
                NadanePrzezUzytkownikId = _dto.NadanePrzezUzytkownikId,
                DataUtworzenia = _dto.DataUtworzenia,
                DataAktywacji = _dto.DataAktywacji,
                DataAnulowania = _dto.DataAnulowania,
                PowodAnulowania = _dto.PowodAnulowania
            };
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
                bool success = _isEditMode
                    ? await _service.UpdateAsync(_id, _dto)
                    : await _service.CreateAsync(BuildCreateDto());

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać członkostwa.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                Saved?.Invoke(this, EventArgs.Empty);
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