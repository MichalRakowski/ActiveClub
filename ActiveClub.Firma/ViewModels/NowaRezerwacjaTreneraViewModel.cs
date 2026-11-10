using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using ActiveClub.Shared.DTOs.Trenerzy;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaRezerwacjaTreneraViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly RezerwacjeTreneraService _rezerwacjeService;
        private readonly TrenerzyService _trenerzyService;
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly KlubyService _klubyService;

        private readonly UpdateRezerwacjaTreneraDto _item;
        private readonly bool _isEditMode;
        private readonly int _id;

        private string _selectedTrenerNazwa = string.Empty;
        private string _selectedUzytkownikNazwa = string.Empty;
        private string _selectedKlubNazwa = string.Empty;

        public ObservableCollection<string> Statusy { get; } = new()
       {
     "NOWA",
    "POTWIERDZONA",
    "ANULOWANA",
    "ZAKONCZONA"
};

        public NowaRezerwacjaTreneraViewModel(
            RezerwacjeTreneraService rezerwacjeService,
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            KlubyService klubyService)
        {
            DisplayName = "Nowa rezerwacja trenera";

            _rezerwacjeService = rezerwacjeService;
            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;
            _klubyService = klubyService;

            _isEditMode = false;
            _id = 0;

            _item = new UpdateRezerwacjaTreneraDto
            {
                StartAt = DateTime.Now,
                EndAt = DateTime.Now.AddHours(1),
                Status = "NOWA"
            };
        }

        public NowaRezerwacjaTreneraViewModel(
            RezerwacjeTreneraService rezerwacjeService,
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            KlubyService klubyService,
            RezerwacjaTreneraDto dto)
        {
            DisplayName = "Edycja rezerwacji trenera";

            _rezerwacjeService = rezerwacjeService;
            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;
            _klubyService = klubyService;

            _isEditMode = true;
            _id = dto.RezerwacjaId;

            _item = new UpdateRezerwacjaTreneraDto
            {
                TrenerId = dto.TrenerId,
                UzytkownikId = dto.UzytkownikId,
                KlubId = dto.KlubId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status,
                Cena = dto.Cena
            };

            _ = LoadSelectedNamesAsync();
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

        public int SelectedTrenerId
        {
            get => _item.TrenerId;
            set
            {
                _item.TrenerId = value;
                OnPropertyChanged(nameof(SelectedTrenerId));
            }
        }

        public int SelectedUzytkownikId
        {
            get => _item.UzytkownikId;
            set
            {
                _item.UzytkownikId = value;
                OnPropertyChanged(nameof(SelectedUzytkownikId));
            }
        }

        public int? SelectedKlubId
        {
            get => _item.KlubId;
            set
            {
                _item.KlubId = value;
                OnPropertyChanged(nameof(SelectedKlubId));
            }
        }

        public DateTime DataRezerwacji
        {
            get => StartAt.Date;
            set
            {
                StartAt = value.Date.Add(StartAt.TimeOfDay);
                EndAt = value.Date.Add(EndAt.TimeOfDay);
                OnPropertyChanged(nameof(DataRezerwacji));
            }
        }

        public string GodzinaOd
        {
            get => StartAt.ToString("HH:mm");
            set
            {
                if (TimeSpan.TryParse(value, out var time))
                {
                    StartAt = StartAt.Date.Add(time);
                    OnPropertyChanged(nameof(GodzinaOd));
                }
            }
        }

        public string GodzinaDo
        {
            get => EndAt.ToString("HH:mm");
            set
            {
                if (TimeSpan.TryParse(value, out var time))
                {
                    EndAt = EndAt.Date.Add(time);
                    OnPropertyChanged(nameof(GodzinaDo));
                }
            }
        }

        public DateTime StartAt
        {
            get => _item.StartAt;
            set
            {
                _item.StartAt = value;
                OnPropertyChanged(nameof(StartAt));
                OnPropertyChanged(nameof(DataRezerwacji));
                OnPropertyChanged(nameof(GodzinaOd));
                OnPropertyChanged(nameof(HarmonogramOpis));
            }
        }

        public DateTime EndAt
        {
            get => _item.EndAt;
            set
            {
                _item.EndAt = value;
                OnPropertyChanged(nameof(EndAt));
                OnPropertyChanged(nameof(GodzinaDo));
                OnPropertyChanged(nameof(HarmonogramOpis));
            }
        }

        public string? SelectedStatus
        {
            get => _item.Status;
            set
            {
                _item.Status = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public decimal? Cena
        {
            get => _item.Cena;
            set
            {
                _item.Cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public string HarmonogramOpis =>
            $"{StartAt:dd.MM.yyyy HH:mm} - {EndAt:HH:mm}";

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzTreneraCommand;
        public ICommand WybierzTreneraCommand =>
            _wybierzTreneraCommand ??= new BaseCommand(() => _ = WybierzTrenera());

        private BaseCommand? _wybierzUzytkownikaCommand;
        public ICommand WybierzUzytkownikaCommand =>
            _wybierzUzytkownikaCommand ??= new BaseCommand(() => _ = WybierzUzytkownika());

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(() => _ = WybierzKlub());

        private BaseCommand? _edytujHarmonogramCommand;
        public ICommand EdytujHarmonogramCommand =>
            _edytujHarmonogramCommand ??= new BaseCommand(() =>
            {
                MessageBox.Show("Datę i godziny edytujesz w polach: Data, Godzina od, Godzina do.");
            });

        private async Task LoadSelectedNamesAsync()
        {
            try
            {
                if (SelectedTrenerId > 0)
                {
                    var trenerzy = await _trenerzyService.GetAllAsync();
                    var trener = trenerzy.FirstOrDefault(t => t.TrenerId == SelectedTrenerId);
                    if (trener != null)
                        ApplyTrener(trener);
                }

                if (SelectedUzytkownikId > 0)
                {
                    var users = await _uzytkownicyService.GetAllAsync();
                    var user = users.FirstOrDefault(u => u.Id == SelectedUzytkownikId);
                    if (user != null)
                        ApplyUzytkownik(user);
                }

                if (SelectedKlubId.HasValue)
                {
                    var kluby = await _klubyService.GetAllAsync();
                    var klub = kluby.FirstOrDefault(k => k.KlubId == SelectedKlubId.Value);
                    if (klub != null)
                        ApplyKlub(klub);
                }
            }
            catch
            {
            }
        }

        private async Task WybierzTrenera()
        {
            try
            {
                var items = await _trenerzyService.GetAllAsync();
                var vm = new WybierzTreneraViewModel(items);

                var window = new WybierzTreneraWindow(vm)
                {
                    Owner = Application.Current.MainWindow
                };

                if (window.ShowDialog() == true && vm.SelectedItem != null)
                    ApplyTrener(vm.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd wyboru trenera", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task WybierzUzytkownika()
        {
            try
            {
                var items = await _uzytkownicyService.GetAllAsync();
                var vm = new WybierzUzytkownikaViewModel(items);

                var window = new WybierzUzytkownikaWindow(vm)
                {
                    Owner = Application.Current.MainWindow
                };

                if (window.ShowDialog() == true && vm.SelectedUser != null)
                    ApplyUzytkownik(vm.SelectedUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd wyboru użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task WybierzKlub()
        {
            try
            {
                var items = await _klubyService.GetAllAsync();
                var vm = new WybierzKlubViewModel(items);

                var window = new WybierzKlubWindow(vm)
                {
                    Owner = Application.Current.MainWindow
                };

                if (window.ShowDialog() == true && vm.SelectedItem != null)
                    ApplyKlub(vm.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd wyboru klubu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyTrener(TrenerDto trener)
        {
            SelectedTrenerId = trener.TrenerId;
            SelectedTrenerNazwa = $"{trener.Imie} {trener.Nazwisko}".Trim();
        }

        private void ApplyUzytkownik(UzytkownikDto user)
        {
            SelectedUzytkownikId = user.Id;
            SelectedUzytkownikNazwa = user.DisplayName;
        }

        private void ApplyKlub(KlubDto klub)
        {
            SelectedKlubId = klub.KlubId;
            SelectedKlubNazwa = klub.Nazwa;
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (SelectedTrenerId <= 0)
            {
                message = "Wybierz trenera.";
                return false;
            }

            if (SelectedUzytkownikId <= 0)
            {
                message = "Wybierz użytkownika.";
                return false;
            }

            if (SelectedKlubId == null || SelectedKlubId <= 0)
            {
                message = "Wybierz klub.";
                return false;
            }

            if (EndAt <= StartAt)
            {
                message = "Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private CreateRezerwacjaTreneraDto BuildCreateDto()
        {
            return new CreateRezerwacjaTreneraDto
            {
                TrenerId = SelectedTrenerId,
                UzytkownikId = SelectedUzytkownikId,
                KlubId = SelectedKlubId,
                StartAt = StartAt,
                EndAt = EndAt,
                Status = SelectedStatus,
                Cena = Cena
            };
        }

        public async Task SaveAndClose()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(msg, "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool success = _isEditMode
                    ? await _rezerwacjeService.UpdateAsync(_id, _item)
                    : await _rezerwacjeService.CreateAsync(BuildCreateDto());

                if (!success)
                {
                    MessageBox.Show("Nie udało się zapisać rezerwacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Saved?.Invoke(this, EventArgs.Empty);
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