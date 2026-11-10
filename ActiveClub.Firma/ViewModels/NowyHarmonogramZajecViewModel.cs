using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.HarmonogramZajec;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using ActiveClub.Shared.DTOs.Sale;
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
    public class NowyHarmonogramZajecViewModel : WorkspaceViewModel
    {
        public event EventHandler? HarmonogramSaved;

        private readonly HarmonogramZajecService _harmonogramService;
        private readonly KlubyService _klubyService;
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly SaleService _saleService;
        private readonly TrenerzyService _trenerzyService;
        private readonly UzytkownicyService _uzytkownicyService;

        private readonly UpdateHarmonogramZajecDto _harmonogram;
        private readonly bool _isEditMode;
        private readonly int _harmonogramId;

        private RodzajZajecDto? _selectedRodzajZajec;
        private SalaDto? _selectedSala;

        private string _selectedKlubNazwa = string.Empty;
        private string _selectedTrenerNazwa = string.Empty;
        private string _selectedUtworzylUzytkownikNazwa = string.Empty;

        private DateTime _dataOd = DateTime.Today;
        private DateTime _dataDo = DateTime.Today.AddMonths(1);
        private string _godzinaStart = "18:00";
        private string _godzinaKoniec = "19:00";

        private bool _pon;
        private bool _wt;
        private bool _sr;
        private bool _czw;
        private bool _pt;
        private bool _sob;
        private bool _nd;

        public ObservableCollection<RodzajZajecDto> RodzajeZajec { get; set; } = new();
        public ObservableCollection<SalaDto> Sale { get; set; } = new();
        public ObservableCollection<string> Statusy { get; } = new()
{
    "AKTYWNE",
    "ANULOWANE",
    "ZAKONCZONE"
};
        public NowyHarmonogramZajecViewModel(
            HarmonogramZajecService harmonogramService,
            KlubyService klubyService,
            RodzajeZajecService rodzajeZajecService,
            SaleService saleService,
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService)
        {
            DisplayName = "Nowy harmonogram zajęć";

            _harmonogramService = harmonogramService ?? throw new ArgumentNullException(nameof(harmonogramService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _trenerzyService = trenerzyService ?? throw new ArgumentNullException(nameof(trenerzyService));
            _uzytkownicyService = uzytkownicyService ?? throw new ArgumentNullException(nameof(uzytkownicyService));

            _isEditMode = false;
            _harmonogramId = 0;

            _harmonogram = new UpdateHarmonogramZajecDto
            {
                KlubId = 0,
                RodzajZajecId = 0,
                SalaId = null,
                TrenerId = null,
                StartAt = DateTime.Today.AddHours(18),
                EndAt = DateTime.Today.AddHours(19),
                LimitMiejsc = null,
                Status = "AKTYWNE",
                CzyWidoczneOnline = true,
                DataUtworzenia = DateTime.Now,
                UtworzylUzytkownikId = null
            };

            _ = LoadDictionariesAsync();
        }

        public NowyHarmonogramZajecViewModel(
            HarmonogramZajecService harmonogramService,
            KlubyService klubyService,
            RodzajeZajecService rodzajeZajecService,
            SaleService saleService,
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            HarmonogramZajecDto dto)
        {
            DisplayName = "Edycja harmonogramu zajęć";

            _harmonogramService = harmonogramService ?? throw new ArgumentNullException(nameof(harmonogramService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _trenerzyService = trenerzyService ?? throw new ArgumentNullException(nameof(trenerzyService));
            _uzytkownicyService = uzytkownicyService ?? throw new ArgumentNullException(nameof(uzytkownicyService));

            _isEditMode = true;
            _harmonogramId = dto.HarmonogramId;

            _harmonogram = new UpdateHarmonogramZajecDto
            {
                KlubId = dto.KlubId,
                RodzajZajecId = dto.RodzajZajecId,
                SalaId = dto.SalaId,
                TrenerId = dto.TrenerId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                LimitMiejsc = dto.LimitMiejsc,
                Status = dto.Status ?? "AKTYWNE",
                CzyWidoczneOnline = dto.CzyWidoczneOnline,
                DataUtworzenia = dto.DataUtworzenia,
                UtworzylUzytkownikId = dto.UtworzylUzytkownikId
            };

            DataOd = dto.StartAt.Date;
            DataDo = dto.StartAt.Date;
            GodzinaStart = dto.StartAt.ToString("HH:mm");
            GodzinaKoniec = dto.EndAt.ToString("HH:mm");

            Pon = dto.StartAt.DayOfWeek == DayOfWeek.Monday;
            Wt = dto.StartAt.DayOfWeek == DayOfWeek.Tuesday;
            Sr = dto.StartAt.DayOfWeek == DayOfWeek.Wednesday;
            Czw = dto.StartAt.DayOfWeek == DayOfWeek.Thursday;
            Pt = dto.StartAt.DayOfWeek == DayOfWeek.Friday;
            Sob = dto.StartAt.DayOfWeek == DayOfWeek.Saturday;
            Nd = dto.StartAt.DayOfWeek == DayOfWeek.Sunday;

            _ = LoadDictionariesAsync();
        }

        public DateTime DataOd
        {
            get => _dataOd;
            set
            {
                _dataOd = value;
                OnPropertyChanged(nameof(DataOd));
            }
        }

        public DateTime DataDo
        {
            get => _dataDo;
            set
            {
                _dataDo = value;
                OnPropertyChanged(nameof(DataDo));
            }
        }

        public string GodzinaStart
        {
            get => _godzinaStart;
            set
            {
                _godzinaStart = value ?? string.Empty;
                OnPropertyChanged(nameof(GodzinaStart));
            }
        }

        public string GodzinaKoniec
        {
            get => _godzinaKoniec;
            set
            {
                _godzinaKoniec = value ?? string.Empty;
                OnPropertyChanged(nameof(GodzinaKoniec));
            }
        }

        public bool Pon { get => _pon; set { _pon = value; OnPropertyChanged(nameof(Pon)); } }
        public bool Wt { get => _wt; set { _wt = value; OnPropertyChanged(nameof(Wt)); } }
        public bool Sr { get => _sr; set { _sr = value; OnPropertyChanged(nameof(Sr)); } }
        public bool Czw { get => _czw; set { _czw = value; OnPropertyChanged(nameof(Czw)); } }
        public bool Pt { get => _pt; set { _pt = value; OnPropertyChanged(nameof(Pt)); } }
        public bool Sob { get => _sob; set { _sob = value; OnPropertyChanged(nameof(Sob)); } }
        public bool Nd { get => _nd; set { _nd = value; OnPropertyChanged(nameof(Nd)); } }

        public string SelectedKlubNazwa
        {
            get => _selectedKlubNazwa;
            set { _selectedKlubNazwa = value; OnPropertyChanged(nameof(SelectedKlubNazwa)); }
        }

        public string SelectedTrenerNazwa
        {
            get => _selectedTrenerNazwa;
            set { _selectedTrenerNazwa = value; OnPropertyChanged(nameof(SelectedTrenerNazwa)); }
        }

        public string SelectedUtworzylUzytkownikNazwa
        {
            get => _selectedUtworzylUzytkownikNazwa;
            set { _selectedUtworzylUzytkownikNazwa = value; OnPropertyChanged(nameof(SelectedUtworzylUzytkownikNazwa)); }
        }

        public RodzajZajecDto? SelectedRodzajZajec
        {
            get => _selectedRodzajZajec;
            set
            {
                _selectedRodzajZajec = value;
                _harmonogram.RodzajZajecId = value?.RodzajZajecId ?? 0;
                OnPropertyChanged(nameof(SelectedRodzajZajec));
                OnPropertyChanged(nameof(RodzajZajecId));
            }
        }

        public SalaDto? SelectedSala
        {
            get => _selectedSala;
            set
            {
                _selectedSala = value;
                _harmonogram.SalaId = value?.SalaId;
                OnPropertyChanged(nameof(SelectedSala));
                OnPropertyChanged(nameof(SalaId));
            }
        }

        public int KlubId
        {
            get => _harmonogram.KlubId;
            set { _harmonogram.KlubId = value; OnPropertyChanged(nameof(KlubId)); }
        }

        public int RodzajZajecId
        {
            get => _harmonogram.RodzajZajecId;
            set
            {
                _harmonogram.RodzajZajecId = value;
                _selectedRodzajZajec = RodzajeZajec.FirstOrDefault(x => x.RodzajZajecId == value);
                OnPropertyChanged(nameof(RodzajZajecId));
                OnPropertyChanged(nameof(SelectedRodzajZajec));
            }
        }

        public int? SalaId
        {
            get => _harmonogram.SalaId;
            set
            {
                _harmonogram.SalaId = value;
                _selectedSala = Sale.FirstOrDefault(x => x.SalaId == value);
                OnPropertyChanged(nameof(SalaId));
                OnPropertyChanged(nameof(SelectedSala));
            }
        }

        public int? TrenerId
        {
            get => _harmonogram.TrenerId;
            set { _harmonogram.TrenerId = value; OnPropertyChanged(nameof(TrenerId)); }
        }

        public int? LimitMiejsc
        {
            get => _harmonogram.LimitMiejsc;
            set { _harmonogram.LimitMiejsc = value; OnPropertyChanged(nameof(LimitMiejsc)); }
        }

        public string Status
        {
            get => _harmonogram.Status ?? string.Empty;
            set { _harmonogram.Status = value; OnPropertyChanged(nameof(Status)); }
        }

        public bool CzyWidoczneOnline
        {
            get => _harmonogram.CzyWidoczneOnline;
            set { _harmonogram.CzyWidoczneOnline = value; OnPropertyChanged(nameof(CzyWidoczneOnline)); }
        }

        public DateTime DataUtworzenia
        {
            get => _harmonogram.DataUtworzenia;
            set { _harmonogram.DataUtworzenia = value; OnPropertyChanged(nameof(DataUtworzenia)); }
        }

        public int? UtworzylUzytkownikId
        {
            get => _harmonogram.UtworzylUzytkownikId;
            set { _harmonogram.UtworzylUzytkownikId = value; OnPropertyChanged(nameof(UtworzylUzytkownikId)); }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(async () => await WybierzKlubAsync());

        private BaseCommand? _wybierzTreneraCommand;
        public ICommand WybierzTreneraCommand =>
            _wybierzTreneraCommand ??= new BaseCommand(async () => await WybierzTreneraAsync());

        private BaseCommand? _wybierzUtworzylUzytkownikaCommand;
        public ICommand WybierzUtworzylUzytkownikaCommand =>
            _wybierzUtworzylUzytkownikaCommand ??= new BaseCommand(async () => await WybierzUtworzylUzytkownikaAsync());

        private async Task WybierzKlubAsync()
        {
            var kluby = await _klubyService.GetAllAsync();
            var vm = new WybierzKlubViewModel(kluby);

            var window = new WybierzKlubWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedItem != null)
                ApplyKlub(vm.SelectedItem);
        }

        private async Task WybierzTreneraAsync()
        {
            var trenerzy = await _trenerzyService.GetAllAsync();
            var vm = new WybierzTreneraViewModel(trenerzy);

            var window = new WybierzTreneraWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedItem != null)
                ApplyTrener(vm.SelectedItem);
        }

        private async Task WybierzUtworzylUzytkownikaAsync()
        {
            var users = await _uzytkownicyService.GetAllAsync();
            var vm = new WybierzUzytkownikaViewModel(users);

            var window = new WybierzUzytkownikaWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedUser != null)
                ApplyUzytkownik(vm.SelectedUser);
        }

        private void ApplyKlub(KlubDto klub)
        {
            KlubId = klub.KlubId;
            SelectedKlubNazwa = klub.Nazwa;
        }

        private void ApplyTrener(TrenerDto trener)
        {
            TrenerId = trener.TrenerId;
            SelectedTrenerNazwa = trener.ImieNazwisko;
        }

        private void ApplyUzytkownik(UzytkownikDto user)
        {
            UtworzylUzytkownikId = user.Id;
            SelectedUtworzylUzytkownikNazwa = $"{user.Imie} {user.Nazwisko}".Trim();

            if (string.IsNullOrWhiteSpace(SelectedUtworzylUzytkownikNazwa))
                SelectedUtworzylUzytkownikNazwa = user.Email;
        }

        private async Task LoadDictionariesAsync()
        {
            try
            {
                var rodzaje = await _rodzajeZajecService.GetAllAsync();
                RodzajeZajec = new ObservableCollection<RodzajZajecDto>(rodzaje);
                OnPropertyChanged(nameof(RodzajeZajec));

                var sale = await _saleService.GetAllAsync();
                Sale = new ObservableCollection<SalaDto>(sale);
                OnPropertyChanged(nameof(Sale));

                SelectedRodzajZajec = RodzajeZajec.FirstOrDefault(x => x.RodzajZajecId == _harmonogram.RodzajZajecId);
                SelectedSala = Sale.FirstOrDefault(x => x.SalaId == _harmonogram.SalaId);

                if (!_isEditMode)
                {
                    if (RodzajeZajec.Any() && SelectedRodzajZajec == null)
                        SelectedRodzajZajec = RodzajeZajec.First();

                    if (Sale.Any() && SelectedSala == null)
                        SelectedSala = Sale.First();
                }

                await LoadSelectedNamesAsync();
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

        private async Task LoadSelectedNamesAsync()
        {
            try
            {
                if (KlubId > 0)
                {
                    var kluby = await _klubyService.GetAllAsync();
                    var klub = kluby.FirstOrDefault(x => x.KlubId == KlubId);

                    if (klub != null)
                        ApplyKlub(klub);
                }

                if (TrenerId.HasValue && TrenerId.Value > 0)
                {
                    var trenerzy = await _trenerzyService.GetAllAsync();
                    var trener = trenerzy.FirstOrDefault(x => x.TrenerId == TrenerId.Value);

                    if (trener != null)
                        ApplyTrener(trener);
                }

                if (UtworzylUzytkownikId.HasValue && UtworzylUzytkownikId.Value > 0)
                {
                    var user = await _uzytkownicyService.GetByIdAsync(UtworzylUzytkownikId.Value);

                    if (user != null)
                        ApplyUzytkownik(user);
                }
            }
            catch
            {
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (KlubId <= 0)
            {
                message = "Klub jest wymagany.";
                return false;
            }

            if (RodzajZajecId <= 0)
            {
                message = "Rodzaj zajęć jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                message = "Status jest wymagany.";
                return false;
            }

            if (DataOd.Date > DataDo.Date)
            {
                message = "Data od nie może być późniejsza niż data do.";
                return false;
            }

            if (!TimeSpan.TryParse(GodzinaStart, out var godzinaStart))
            {
                message = "Godzina rozpoczęcia musi być w formacie HH:mm, np. 18:00.";
                return false;
            }

            if (!TimeSpan.TryParse(GodzinaKoniec, out var godzinaKoniec))
            {
                message = "Godzina zakończenia musi być w formacie HH:mm, np. 19:00.";
                return false;
            }

            if (godzinaStart >= godzinaKoniec)
            {
                message = "Godzina rozpoczęcia musi być wcześniejsza niż godzina zakończenia.";
                return false;
            }

            if (!_isEditMode && !CzyWybranoJakikolwiekDzien())
            {
                message = "Wybierz przynajmniej jeden dzień tygodnia.";
                return false;
            }

            if (LimitMiejsc.HasValue && LimitMiejsc < 1)
            {
                message = "Limit miejsc musi być większy od 0.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private bool CzyWybranoJakikolwiekDzien()
        {
            return Pon || Wt || Sr || Czw || Pt || Sob || Nd;
        }

        private bool CzyWybranyDzien(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => Pon,
                DayOfWeek.Tuesday => Wt,
                DayOfWeek.Wednesday => Sr,
                DayOfWeek.Thursday => Czw,
                DayOfWeek.Friday => Pt,
                DayOfWeek.Saturday => Sob,
                DayOfWeek.Sunday => Nd,
                _ => false
            };
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
                    var startTime = TimeSpan.Parse(GodzinaStart);
                    var endTime = TimeSpan.Parse(GodzinaKoniec);

                    _harmonogram.StartAt = DataOd.Date.Add(startTime);
                    _harmonogram.EndAt = DataOd.Date.Add(endTime);

                    success = await _harmonogramService.UpdateAsync(_harmonogramId, _harmonogram);
                }
                else
                {
                    var startTime = TimeSpan.Parse(GodzinaStart);
                    var endTime = TimeSpan.Parse(GodzinaKoniec);

                    var current = DataOd.Date;
                    var createdCount = 0;

                    while (current <= DataDo.Date)
                    {
                        if (CzyWybranyDzien(current.DayOfWeek))
                        {
                            var createDto = new CreateHarmonogramZajecDto
                            {
                                KlubId = KlubId,
                                RodzajZajecId = RodzajZajecId,
                                SalaId = SalaId,
                                TrenerId = TrenerId,
                                StartAt = current.Add(startTime),
                                EndAt = current.Add(endTime),
                                LimitMiejsc = LimitMiejsc,
                                Status = Status,
                                CzyWidoczneOnline = CzyWidoczneOnline,
                                DataUtworzenia = DateTime.Now,
                                UtworzylUzytkownikId = UtworzylUzytkownikId
                            };

                            var ok = await _harmonogramService.CreateAsync(createDto);

                            if (ok)
                                createdCount++;
                        }

                        current = current.AddDays(1);
                    }

                    if (createdCount == 0)
                    {
                        MessageBox.Show(
                            "Nie utworzono żadnych zajęć. Sprawdź zakres dat i dni tygodnia.",
                            "Brak utworzonych zajęć",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }

                    success = true;
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać harmonogramu zajęć.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                HarmonogramSaved?.Invoke(this, EventArgs.Empty);
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