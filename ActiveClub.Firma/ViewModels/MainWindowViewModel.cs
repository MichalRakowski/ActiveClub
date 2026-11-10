using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using ActiveClub.Shared.DTOs.PracownicyKlubu;
using System.Windows;
namespace ActiveClub.Firma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Services
        private readonly ICurrentUserService _currentUserService;
        private readonly DashboardService _dashboardService;
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly AktualnosciService _aktualnosciService;
        private readonly KlubyService _klubyService;
        private readonly RodzajeCzlonkostwaService _rodzajeCzlonkostwaService;
        private readonly TrenerzyService _trenerzyService;
        private readonly RezerwacjeTreneraService _rezerwacjeTreneraService;
        private readonly DostepnoscTreneraService _dostepnoscTreneraService;
        private readonly GodzinyOtwarciaService _godzinyOtwarciaService;
        private readonly MetodyPlatnosciService _metodyPlatnosciService;
        private readonly SaleService _saleService;
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly RabatyService _rabatyService;
        private readonly OpinieService _opinieService;
        private readonly HarmonogramZajecService _harmonogramZajecService;
        private readonly ZamowieniaService _zamowieniaService;
        private readonly OfertyKlubuCzlonkostwaService _ofertyKlubuCzlonkostwaService;
        private readonly PlatnosciService _platnosciService;
        private readonly PozycjeZamowieniaService _pozycjeZamowieniaService;
        private readonly StanowiskaService _stanowiskaService;
        private readonly RejestracjeNaZajeciaService _rejestracjeNaZajeciaService;
        private readonly CzlonkostwaUzytkownikaService _czlonkostwaUzytkownikaService;
        private readonly PracownicyKlubuService _pracownicyKlubuService;
        private readonly RaportyService _raportyService;
        private readonly UstawieniaKontaktoweService _ustawieniaKontaktoweService;
        #endregion

        #region Fields

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        private ReadOnlyCollection<CommandViewModel> _uzytkownicyCommands;
        private ReadOnlyCollection<CommandViewModel> _aktualnosciCommands;
        private ReadOnlyCollection<CommandViewModel> _klubyCommands;
        private ReadOnlyCollection<CommandViewModel> _rodzajeCzlonkostwaCommands;
        private ReadOnlyCollection<CommandViewModel> _trenerzyCommands;
        private ReadOnlyCollection<CommandViewModel> _rezerwacjeTreneraCommands;
        private ReadOnlyCollection<CommandViewModel> _dostepnoscTreneraCommands;
        private ReadOnlyCollection<CommandViewModel> _godzinyOtwarciaCommands;
        private ReadOnlyCollection<CommandViewModel> _metodyPlatnosciCommands;
        private ReadOnlyCollection<CommandViewModel> _saleCommands;
        private ReadOnlyCollection<CommandViewModel> _rodzajeZajecCommands;
        private ReadOnlyCollection<CommandViewModel> _rabatyCommands;
        private ReadOnlyCollection<CommandViewModel> _opinieCommands;
        private ReadOnlyCollection<CommandViewModel> _harmonogramZajecCommands;
        private ReadOnlyCollection<CommandViewModel> _zamowieniaCommands;
        private ReadOnlyCollection<CommandViewModel> _ofertyKlubuCzlonkostwaCommands;
        private ReadOnlyCollection<CommandViewModel> _platnosciCommands;
        private ReadOnlyCollection<CommandViewModel> _pozycjeZamowieniaCommands;
        private ReadOnlyCollection<CommandViewModel> _stanowiskaCommands;
        private ReadOnlyCollection<CommandViewModel> _rejestracjeNaZajeciaCommands;
        private ReadOnlyCollection<CommandViewModel> _czlonkostwaCommands;
        private ReadOnlyCollection<CommandViewModel> _pracownicyKlubuCommands;
        private ReadOnlyCollection<CommandViewModel> _raportyCommands;
        private ReadOnlyCollection<CommandViewModel> _ustawieniaKontaktoweCommands;
        private int _activeUsersCount;
        private int _activeSoldMembershipsCount;
        private int _activeTrainersCount;
        private int _activeClubsCount;

        private string _lastOpinionAuthor = "Brak danych";
        private string _lastOpinionContent = "Brak opinii";
        private string _lastOpinionDate = string.Empty;

        private string _lastNewsTitle = "Brak aktualności";
        private string _lastNewsContent = "Brak treści.";
        private string _lastNewsDate = string.Empty;

        private ObservableCollection<RodzajZajecDto> _ostatnioDodaneRodzajeZajec = new ObservableCollection<RodzajZajecDto>();
        private ObservableCollection<PracownikKlubuDto> _ostatnioDodaniPracownicyKlubu = new ObservableCollection<PracownikKlubuDto>();
        #endregion

        #region Constructor

        public MainWindowViewModel(
            ICurrentUserService currentUserService,
            DashboardService dashboardService,
            UzytkownicyService uzytkownicyService,
            AktualnosciService aktualnosciService,
            KlubyService klubyService,
            RejestracjeNaZajeciaService rejestracjeNaZajeciaService,
            OfertyKlubuCzlonkostwaService ofertyKlubuCzlonkostwaService,
            MetodyPlatnosciService metodyPlatnosciService,
            GodzinyOtwarciaService godzinyOtwarciaService,
            CzlonkostwaUzytkownikaService czlonkostwaUzytkownikaService,
            DostepnoscTreneraService dostepnoscTreneraService,
            RodzajeCzlonkostwaService rodzajeCzlonkostwaService,
            RezerwacjeTreneraService rezerwacjeTreneraService,
            SaleService saleService,
            RodzajeZajecService rodzajeZajecService,
            RabatyService rabatyService,
            PlatnosciService platnosciService,
            StanowiskaService stanowiskaService,
            PozycjeZamowieniaService pozycjeZamowieniaService,
            OpinieService opinieService,
            ZamowieniaService zamowieniaService,
            RaportyService raportyService,
            PracownicyKlubuService pracownicyKlubuService,

            UstawieniaKontaktoweService ustawieniaKontaktoweService,
            HarmonogramZajecService harmonogramZajecService,
            TrenerzyService trenerzyService)
        {
            _currentUserService = currentUserService;
            _dashboardService = dashboardService;
            _uzytkownicyService = uzytkownicyService;
            _aktualnosciService = aktualnosciService;
            _klubyService = klubyService;
            _rejestracjeNaZajeciaService = rejestracjeNaZajeciaService;
            _ofertyKlubuCzlonkostwaService = ofertyKlubuCzlonkostwaService;
            _metodyPlatnosciService = metodyPlatnosciService;
            _godzinyOtwarciaService = godzinyOtwarciaService;
            _czlonkostwaUzytkownikaService = czlonkostwaUzytkownikaService;
            _dostepnoscTreneraService = dostepnoscTreneraService;
            _rodzajeCzlonkostwaService = rodzajeCzlonkostwaService;
            _rezerwacjeTreneraService = rezerwacjeTreneraService;
            _ustawieniaKontaktoweService = ustawieniaKontaktoweService;
            _saleService = saleService;
            _rodzajeZajecService = rodzajeZajecService;
            _rabatyService = rabatyService;
            _platnosciService = platnosciService;
            _stanowiskaService = stanowiskaService;
            _pozycjeZamowieniaService = pozycjeZamowieniaService;
            _opinieService = opinieService;
            _zamowieniaService = zamowieniaService;
            _raportyService = raportyService;
            _pracownicyKlubuService = pracownicyKlubuService;
            _harmonogramZajecService = harmonogramZajecService;
            _trenerzyService = trenerzyService;

            _ = RefreshDashboardAsync();
        }

        #endregion

        #region Workspace State
        public bool IsAdmin => _currentUserService.IsAdmin;

        public bool IsPracownik => _currentUserService.IsPracownik;

        public bool IsTrener => _currentUserService.IsTrener;

       

        public bool HasOpenWorkspace => Workspaces.Any();

        public bool IsDashboardVisible => !HasOpenWorkspace;
        public bool IsWorkspaceVisible => HasOpenWorkspace;

        public bool CanSeeUsers =>
    _currentUserService.IsAdmin ||
    _currentUserService.IsPracownik;

        public bool CanSeeSales =>
            _currentUserService.IsAdmin ||
            _currentUserService.IsPracownik;

        public bool CanSeeClasses =>
            _currentUserService.IsAdmin ||
            _currentUserService.IsPracownik ||
            _currentUserService.IsTrener;

        public bool CanSeeStaff =>
            _currentUserService.IsAdmin ||
            _currentUserService.IsPracownik;

        public bool CanSeeConfig =>
            _currentUserService.IsAdmin;

        public bool CanSeeContent =>
            _currentUserService.IsAdmin ||
            _currentUserService.IsPracownik;

        public bool CanSeeReports =>
            _currentUserService.IsAdmin;
        public WorkspaceViewModel CurrentWorkspace
        {
            get
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
                return collectionView?.CurrentItem as WorkspaceViewModel;
            }
        }

        #endregion

        #region KPI + Dashboard

        public int ActiveUsersCount
        {
            get => _activeUsersCount;
            set
            {
                _activeUsersCount = value;
                OnPropertyChanged(nameof(ActiveUsersCount));
            }
        }
        public ObservableCollection<PracownikKlubuDto> OstatnioDodaniPracownicyKlubu
        {
            get => _ostatnioDodaniPracownicyKlubu;
            set
            {
                _ostatnioDodaniPracownicyKlubu = value;
                OnPropertyChanged(nameof(OstatnioDodaniPracownicyKlubu));
            }
        }
        public int ActiveSoldMembershipsCount
        {
            get => _activeSoldMembershipsCount;
            set
            {
                _activeSoldMembershipsCount = value;
                OnPropertyChanged(nameof(ActiveSoldMembershipsCount));
            }
        }

        public int ActiveTrainersCount
        {
            get => _activeTrainersCount;
            set
            {
                _activeTrainersCount = value;
                OnPropertyChanged(nameof(ActiveTrainersCount));
            }
        }

        public int ActiveClubsCount
        {
            get => _activeClubsCount;
            set
            {
                _activeClubsCount = value;
                OnPropertyChanged(nameof(ActiveClubsCount));
            }
        }

        public string LastOpinionAuthor
        {
            get => _lastOpinionAuthor;
            set
            {
                _lastOpinionAuthor = value;
                OnPropertyChanged(nameof(LastOpinionAuthor));
            }
        }

        public string LastOpinionContent
        {
            get => _lastOpinionContent;
            set
            {
                _lastOpinionContent = value;
                OnPropertyChanged(nameof(LastOpinionContent));
            }
        }

        public string LastOpinionDate
        {
            get => _lastOpinionDate;
            set
            {
                _lastOpinionDate = value;
                OnPropertyChanged(nameof(LastOpinionDate));
            }
        }

        public string LastNewsTitle
        {
            get => _lastNewsTitle;
            set
            {
                _lastNewsTitle = value;
                OnPropertyChanged(nameof(LastNewsTitle));
            }
        }

        public string LastNewsContent
        {
            get => _lastNewsContent;
            set
            {
                _lastNewsContent = value;
                OnPropertyChanged(nameof(LastNewsContent));
            }
        }

        public string LastNewsDate
        {
            get => _lastNewsDate;
            set
            {
                _lastNewsDate = value;
                OnPropertyChanged(nameof(LastNewsDate));
            }
        }

        public ObservableCollection<RodzajZajecDto> OstatnioDodaneRodzajeZajec
        {
            get => _ostatnioDodaneRodzajeZajec;
            set
            {
                _ostatnioDodaneRodzajeZajec = value;
                OnPropertyChanged(nameof(OstatnioDodaneRodzajeZajec));
            }
        }

        public async Task RefreshDashboardAsync()
        {
            await RefreshKpiAsync();
            await RefreshLastOpinionAsync();
            await RefreshLastNewsAsync();
            await RefreshOstatnioDodaneRodzajeZajecAsync();
            await RefreshOstatnioDodanychPracownikowKlubuAsync();
        }

        public async Task RefreshKpiAsync()
        {
            try
            {
                var users = await _uzytkownicyService.GetAllAsync();
                ActiveUsersCount = users?.Count(IsActive) ?? 0;
            }
            catch
            {
                ActiveUsersCount = 0;
            }

            try
            {
                var memberships = await _czlonkostwaUzytkownikaService.GetAllAsync();
                ActiveSoldMembershipsCount = memberships?.Count(IsActiveMembership) ?? 0;
            }
            catch
            {
                ActiveSoldMembershipsCount = 0;
            }

            try
            {
                var trainers = await _trenerzyService.GetAllAsync();
                ActiveTrainersCount = trainers?.Count(IsActive) ?? 0;
            }
            catch
            {
                ActiveTrainersCount = 0;
            }

            try
            {
                var clubs = await _klubyService.GetAllAsync();
                ActiveClubsCount = clubs?.Count(IsActive) ?? 0;
            }
            catch
            {
                ActiveClubsCount = 0;
            }
        }

        private async Task RefreshLastOpinionAsync()
        {
            try
            {
                var opinions = await _opinieService.GetAllAsync();
                var lastOpinion = opinions?
                    .OrderByDescending(GetOpinionDate)
                    .FirstOrDefault();

                if (lastOpinion == null)
                {
                    LastOpinionAuthor = "Brak danych";
                    LastOpinionContent = "Brak opinii";
                    LastOpinionDate = string.Empty;
                    return;
                }

                LastOpinionAuthor = GetOpinionAuthor(lastOpinion) ?? "Użytkownik";
                LastOpinionContent = GetOpinionContent(lastOpinion) ?? "Brak treści opinii";

                var date = GetOpinionDate(lastOpinion);
                LastOpinionDate = date == DateTime.MinValue
                    ? string.Empty
                    : date.ToString("dd.MM.yyyy HH:mm");
            }
            catch
            {
                LastOpinionAuthor = "Brak danych";
                LastOpinionContent = "Nie udało się pobrać opinii.";
                LastOpinionDate = string.Empty;
            }
        }

        private async Task RefreshLastNewsAsync()
        {
            try
            {
                var news = await _aktualnosciService.GetAllAsync();
                var lastNews = news?
                    .OrderByDescending(GetNewsDate)
                    .FirstOrDefault();

                if (lastNews == null)
                {
                    LastNewsTitle = "Brak aktualności";
                    LastNewsContent = "Brak treści.";
                    LastNewsDate = string.Empty;
                    return;
                }

                LastNewsTitle = GetNewsTitle(lastNews) ?? "Bez tytułu";

                var content = GetNewsContent(lastNews) ?? "Brak treści.";
                LastNewsContent = content.Length > 140
                    ? content.Substring(0, 140) + "..."
                    : content;

                var date = GetNewsDate(lastNews);
                LastNewsDate = date == DateTime.MinValue
                    ? string.Empty
                    : date.ToString("dd.MM.yyyy HH:mm");
            }
            catch
            {
                LastNewsTitle = "Brak aktualności";
                LastNewsContent = "Nie udało się pobrać aktualności.";
                LastNewsDate = string.Empty;
            }
        }
        private async Task RefreshOstatnioDodanychPracownikowKlubuAsync()
        {
            try
            {
                var items = await _pracownicyKlubuService.GetAllAsync();

                OstatnioDodaniPracownicyKlubu = new ObservableCollection<PracownikKlubuDto>(
                    items?
                        .OrderByDescending(x => x.PracownikKlubId)
                        .Take(5)
                        .ToList()
                    ?? new List<PracownikKlubuDto>());
            }
            catch
            {
                OstatnioDodaniPracownicyKlubu = new ObservableCollection<PracownikKlubuDto>();
            }
        }
        private async Task RefreshOstatnioDodaneRodzajeZajecAsync()
        {
            try
            {
                var items = await _rodzajeZajecService.GetAllAsync();

                OstatnioDodaneRodzajeZajec = new ObservableCollection<RodzajZajecDto>(
                    items?
                        .OrderByDescending(x => x.RodzajZajecId)
                        .Take(5)
                        .ToList()
                    ?? new List<RodzajZajecDto>());
            }
            catch
            {
                OstatnioDodaneRodzajeZajec = new ObservableCollection<RodzajZajecDto>();
            }
        }

        private static bool IsActive<T>(T item)
        {
            if (item == null)
            {
                return false;
            }

            var type = item.GetType();

            var propertyNames = new[]
            {
                "IsActive",
                "Aktywny",
                "CzyAktywny",
                "Active",
                "IsEnabled",
                "Enabled"
            };

            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property?.PropertyType == typeof(bool))
                {
                    var value = property.GetValue(item);
                    if (value is bool boolValue)
                    {
                        return boolValue;
                    }
                }

                if (property?.PropertyType == typeof(bool?))
                {
                    var value = property.GetValue(item);
                    if (value is bool boolValue)
                    {
                        return boolValue;
                    }
                }
            }

            return true;
        }

        private static bool IsActiveMembership<T>(T item)
        {
            if (item == null)
            {
                return false;
            }

            var type = item.GetType();

            var boolPropertyNames = new[]
            {
                "IsActive",
                "Aktywny",
                "CzyAktywny",
                "Active",
                "CzyAktywne"
            };

            foreach (var propertyName in boolPropertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property?.PropertyType == typeof(bool))
                {
                    var value = property.GetValue(item);
                    if (value is bool boolValue)
                    {
                        return boolValue;
                    }
                }

                if (property?.PropertyType == typeof(bool?))
                {
                    var value = property.GetValue(item);
                    if (value is bool boolValue)
                    {
                        return boolValue;
                    }
                }
            }

            var endDatePropertyNames = new[]
            {
                "DataKonca",
                "DataZakonczenia",
                "EndDate",
                "WazneDo",
                "AktywneDo",
                "EndAt"
            };

            foreach (var propertyName in endDatePropertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    continue;
                }

                var value = property.GetValue(item);

                if (value == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    return (DateTime)value >= DateTime.Now;
                }

                if (property.PropertyType == typeof(DateTime?))
                {
                    var nullableEndDate = (DateTime?)value;
                    if (nullableEndDate.HasValue)
                    {
                        return nullableEndDate.Value >= DateTime.Now;
                    }
                }
            }

            return true;
        }

        private static string GetOpinionAuthor(object opinion)
        {
            return GetStringProperty(opinion,
                "AuthorName",
                "UzytkownikNazwa",
                "Uzytkownik",
                "Autor",
                "ImieNazwisko",
                "UserName");
        }

        private static string GetOpinionContent(object opinion)
        {
            return GetStringProperty(opinion,
                "Content",
                "Tresc",
                "Opis",
                "Komentarz",
                "OpinionContent");
        }

        private static DateTime GetOpinionDate(object opinion)
        {
            return GetDateProperty(opinion,
                "CreatedAt",
                "DataDodania",
                "DataUtworzenia",
                "CreatedDate",
                "DodanoDnia");
        }

        private static string GetNewsTitle(object news)
        {
            return GetStringProperty(news,
                "Tytul",
                "Title",
                "Nazwa");
        }

        private static string GetNewsContent(object news)
        {
            return GetStringProperty(news,
                "Tresc",
                "Content",
                "Opis");
        }

        private static DateTime GetNewsDate(object news)
        {
            return GetDateProperty(news,
                "DataDodania",
                "DataPublikacji",
                "CreatedAt",
                "CreatedDate");
        }

        private static string GetStringProperty(object obj, params string[] propertyNames)
        {
            if (obj == null)
            {
                return null;
            }

            var type = obj.GetType();

            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    continue;
                }

                var value = property.GetValue(obj)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }

        private static DateTime GetDateProperty(object obj, params string[] propertyNames)
        {
            if (obj == null)
            {
                return DateTime.MinValue;
            }

            var type = obj.GetType();

            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    continue;
                }

                var value = property.GetValue(obj);

                if (value == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    return (DateTime)value;
                }

                if (property.PropertyType == typeof(DateTime?))
                {
                    var nullableDateTime = (DateTime?)value;
                    if (nullableDateTime.HasValue)
                    {
                        return nullableDateTime.Value;
                    }
                }
            }

            return DateTime.MinValue;
        }

        #endregion

        #region Commands
        //do usu
        public IRelayCommand SetAdminRoleCommand => new RelayCommand(() =>
        {
            _currentUserService.SetRole("Admin");
            RefreshRoleVisibility();
        });

        public IRelayCommand SetPracownikRoleCommand => new RelayCommand(() =>
        {
            _currentUserService.SetRole("Pracownik");
            RefreshRoleVisibility();
        });

        public IRelayCommand SetTrenerRoleCommand => new RelayCommand(() =>
        {
            _currentUserService.SetRole("Trener");
            RefreshRoleVisibility();
        });

        
        private void RefreshRoleVisibility()
        {
            
            OnPropertyChanged(nameof(CanSeeUsers));
            OnPropertyChanged(nameof(CanSeeSales));
            OnPropertyChanged(nameof(CanSeeClasses));
            OnPropertyChanged(nameof(CanSeeStaff));
            OnPropertyChanged(nameof(CanSeeConfig));
            OnPropertyChanged(nameof(CanSeeContent));
            OnPropertyChanged(nameof(CanSeeReports));
        }
        //do usu

        public ReadOnlyCollection<CommandViewModel> UzytkownicyCommands =>
     _uzytkownicyCommands ??= CreateCommandPair(
         "➕ Dodaj użytkownika",
         () => CreateView(new NowyUzytkownikViewModel(
    _uzytkownicyService,
    _klubyService,
    _trenerzyService,
    _stanowiskaService,
    _czlonkostwaUzytkownikaService,
    _rodzajeCzlonkostwaService,
    _harmonogramZajecService,
    _zamowieniaService,
    _platnosciService,
    _rodzajeZajecService,
    _saleService,
    CreateView)),
         "📋 Wszyscy użytkownicy",
         ShowAllUzytkownicy);

        public ReadOnlyCollection<CommandViewModel> AktualnosciCommands =>
    _aktualnosciCommands ??= CreateCommandPair(
        "➕ Dodaj aktualność",
        () => CreateView(new NowaAktualnoscViewModel(
            _aktualnosciService,
            _klubyService,
            _uzytkownicyService)),
        "📋 Wszystkie aktualności",
        ShowAllAktualnosci);
        public ReadOnlyCollection<CommandViewModel> UstawieniaKontaktoweCommands =>
    _ustawieniaKontaktoweCommands ??= CreateSingleCommand(
        "⚙️ Dane kontaktowe",
        ShowUstawieniaKontaktowe);
        public ReadOnlyCollection<CommandViewModel> KlubyCommands =>
            _klubyCommands ??= CreateCommandPair(
                "➕ Dodaj klub",
                () => CreateView(new NowyKlubViewModel(_klubyService)),
                "📋 Wszystkie kluby",
                ShowAllKluby);

        public ReadOnlyCollection<CommandViewModel> RodzajeCzlonkostwaCommands =>
            _rodzajeCzlonkostwaCommands ??= CreateCommandPair(
                "➕ Dodaj rodzaj członkostwa",
                () => CreateView(new NowyRodzajCzlonkostwaViewModel(_rodzajeCzlonkostwaService)),
                "📋 Wszystkie rodzaje członkostwa",
                ShowAllRodzajeCzlonkostwa);

        public ReadOnlyCollection<CommandViewModel> CzlonkostwaCommands =>
            _czlonkostwaCommands ??= CreateCommandPair(
                "➕ Dodaj członkostwo",
                () => CreateView(new NoweCzlonkostwoUzytkownikaViewModel(_czlonkostwaUzytkownikaService)),
                "📋 Wszystkie członkostwa",
                ShowAllCzlonkostwa);

        public ReadOnlyCollection<CommandViewModel> OfertyKlubuCzlonkostwaCommands =>
            _ofertyKlubuCzlonkostwaCommands ??= CreateCommandPair(
                "➕ Dodaj ofertę członkostwa",
                () => CreateView(new NowaOfertaKlubuCzlonkostwoViewModel(
    _ofertyKlubuCzlonkostwaService,
    _klubyService)),
                "📋 Wszystkie oferty członkostwa",
                ShowAllOfertyKlubuCzlonkostwa);

        public ReadOnlyCollection<CommandViewModel> PlatnosciCommands =>
            _platnosciCommands ??= CreateCommandPair(
                "➕ Dodaj płatność",
                () => CreateView(new NowaPlatnoscViewModel(_platnosciService)),
                "📋 Wszystkie płatności",
                ShowAllPlatnosci);

        public ReadOnlyCollection<CommandViewModel> MetodyPlatnosciCommands =>
            _metodyPlatnosciCommands ??= CreateCommandPair(
                "➕ Dodaj metodę płatności",
                () => CreateView(new NowaMetodaPlatnosciViewModel(_metodyPlatnosciService)),
                "📋 Wszystkie metody płatności",
                ShowAllMetodyPlatnosci);

        public ReadOnlyCollection<CommandViewModel> RabatyCommands =>
            _rabatyCommands ??= CreateCommandPair(
                "➕ Dodaj rabat",
                () => CreateView(new NowyRabatViewModel(_rabatyService)),
                "📋 Wszystkie rabaty",
                ShowAllRabaty);

        public ReadOnlyCollection<CommandViewModel> ZamowieniaCommands =>
            _zamowieniaCommands ??= CreateCommandPair(
                "➕ Dodaj zamówienie",
                () => CreateView(new NoweZamowienieViewModel(_zamowieniaService)),
                "📋 Wszystkie zamówienia",
                ShowAllZamowienia);

        public ReadOnlyCollection<CommandViewModel> PozycjeZamowieniaCommands =>
            _pozycjeZamowieniaCommands ??= CreateCommandPair(
                "➕ Dodaj pozycję zamówienia",
                () => CreateView(new NowaPozycjaZamowieniaViewModel(_pozycjeZamowieniaService)),
                "📋 Wszystkie pozycje zamówienia",
                ShowAllPozycjeZamowienia);

        public ReadOnlyCollection<CommandViewModel> SaleCommands =>
            _saleCommands ??= CreateCommandPair(
                "➕ Dodaj salę",
                () => CreateView(new NowaSalaViewModel(_saleService, _klubyService)),
                "📋 Wszystkie sale",
                ShowAllSale);

        public ReadOnlyCollection<CommandViewModel> RodzajeZajecCommands =>
            _rodzajeZajecCommands ??= CreateCommandPair(
                "➕ Dodaj rodzaj zajęć",
                () => CreateView(new NowyRodzajZajecViewModel(_rodzajeZajecService)),
                "📋 Wszystkie rodzaje zajęć",
                ShowAllRodzajeZajec);

        public ReadOnlyCollection<CommandViewModel> HarmonogramZajecCommands =>
            _harmonogramZajecCommands ??= CreateCommandPair(
                "➕ Dodaj harmonogram zajęć",
                () => CreateView(new NowyHarmonogramZajecViewModel(
    _harmonogramZajecService,
    _klubyService,
    _rodzajeZajecService,
    _saleService,
    _trenerzyService,
    _uzytkownicyService)),
                "📋 Wszystkie harmonogramy zajęć",
                ShowAllHarmonogramyZajec);

        public ReadOnlyCollection<CommandViewModel> RejestracjeNaZajeciaCommands =>
            _rejestracjeNaZajeciaCommands ??= CreateCommandPair(
                "➕ Dodaj rezerwację",
                () => CreateView(new NowaRejestracjaNaZajeciaViewModel(_rejestracjeNaZajeciaService)),
                "📋 Wszystkie rezerwacje",
                ShowAllRejestracjeNaZajecia);

        public ReadOnlyCollection<CommandViewModel> TrenerzyCommands =>
     _trenerzyCommands ??= CreateCommandPair(
         "➕ Dodaj trenera",
         () => CreateView(new NowyTrenerViewModel(_trenerzyService, _uzytkownicyService)),
         "📋 Wszyscy trenerzy",
         ShowAllTrenerzy);

        public ReadOnlyCollection<CommandViewModel> RezerwacjeTreneraCommands =>
            _rezerwacjeTreneraCommands ??= CreateCommandPair(
                "➕ Dodaj rezerwację trenera",
                () => CreateView(new NowaRezerwacjaTreneraViewModel(
                    _rezerwacjeTreneraService,
                    _trenerzyService,
                    _uzytkownicyService,
                    _klubyService)),
                "📋 Wszystkie rezerwacje trenera",
                ShowAllRezerwacjeTrenera);

        public ReadOnlyCollection<CommandViewModel> DostepnoscTreneraCommands =>
            _dostepnoscTreneraCommands ??= CreateCommandPair(
                "➕ Dodaj dostępność trenera",
                () => CreateView(new NowaDostepnoscTreneraViewModel(
                    _dostepnoscTreneraService,
                    _trenerzyService,
                    _klubyService)),
                "📋 Wszystkie dostępności trenerów",
                ShowAllDostepnosciTrenera);

        public ReadOnlyCollection<CommandViewModel> PracownicyKlubuCommands =>
            _pracownicyKlubuCommands ??= CreateCommandPair(
                "➕ Dodaj pracownika klubu",
                () => CreateView(new NowyPracownikKlubuViewModel(_pracownicyKlubuService)),
                "📋 Wszyscy pracownicy klubu",
                ShowAllPracownicyKlubu);

        public ReadOnlyCollection<CommandViewModel> StanowiskaCommands =>
            _stanowiskaCommands ??= CreateCommandPair(
                "➕ Dodaj stanowisko",
                () => CreateView(new NoweStanowiskoViewModel(_stanowiskaService)),
                "📋 Wszystkie stanowiska",
                ShowAllStanowiska);

        public ReadOnlyCollection<CommandViewModel> GodzinyOtwarciaCommands =>
            _godzinyOtwarciaCommands ??= CreateCommandPair(
                "➕ Dodaj godziny otwarcia",
                () => CreateView(new NoweGodzinyOtwarciaViewModel(_godzinyOtwarciaService, _klubyService)),
                "📋 Wszystkie godziny otwarcia",
                ShowAllGodzinyOtwarcia);

        public ReadOnlyCollection<CommandViewModel> OpinieCommands =>
            _opinieCommands ??= CreateSingleCommand(
                "📋 Wszystkie opinie",
                ShowAllOpinie);

        public ReadOnlyCollection<CommandViewModel> RaportyCommands =>
            _raportyCommands ??= CreateCommandPair(
                "➕ Generuj raport",
                () => CreateView(new NowyRaportViewModel(_raportyService)),
                "📋 Wszystkie raporty",
                ShowAllRaporty);

        #endregion

        #region Workspaces

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }

                return _workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += OnWorkspaceRequestClose;
                }
            }

            if (e.OldItems != null)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= OnWorkspaceRequestClose;
                }
            }

            OnPropertyChanged(nameof(HasOpenWorkspace));
            OnPropertyChanged(nameof(IsDashboardVisible));
            OnPropertyChanged(nameof(IsWorkspaceVisible));
            OnPropertyChanged(nameof(CurrentWorkspace));
        }

        private async void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            if (sender is not WorkspaceViewModel workspace)
            {
                return;
            }

            Workspaces.Remove(workspace);

            OnPropertyChanged(nameof(CurrentWorkspace));
            OnPropertyChanged(nameof(HasOpenWorkspace));
            OnPropertyChanged(nameof(IsDashboardVisible));
            OnPropertyChanged(nameof(IsWorkspaceVisible));

            await RefreshDashboardAsync();
        }

        private void CreateView(WorkspaceViewModel workspace)
        {
            if (workspace is NowyUzytkownikViewModel nowyUzytkownik)
            {
                nowyUzytkownik.UserSaved += async (_, __) => await RefreshDashboardAsync();
            }

            if (workspace is NowyTrenerViewModel nowyTrener)
            {
                nowyTrener.TrainerSaved += async (_, __) => await RefreshDashboardAsync();
            }

            if (workspace is NowaAktualnoscViewModel nowaAktualnosc)
            {
                nowaAktualnosc.SavedSuccessfully += async () => await RefreshDashboardAsync();
            }

            if (workspace is NowyRodzajZajecViewModel nowyRodzajZajec)
            {
                nowyRodzajZajec.RodzajZajecSaved += async (_, __) => await RefreshDashboardAsync();
            }

            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void ShowSingletonWorkspace<TWorkspace>(Func<TWorkspace> factory)
            where TWorkspace : WorkspaceViewModel
        {
            var existingWorkspace = Workspaces.OfType<TWorkspace>().FirstOrDefault();
            if (existingWorkspace != null)
            {
                SetActiveWorkspace(existingWorkspace);
                return;
            }

            var workspace = factory();
            CreateView(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            collectionView?.MoveCurrentTo(workspace);

            OnPropertyChanged(nameof(CurrentWorkspace));
            OnPropertyChanged(nameof(HasOpenWorkspace));
            OnPropertyChanged(nameof(IsDashboardVisible));
            OnPropertyChanged(nameof(IsWorkspaceVisible));
        }

        #endregion

        #region Show All Methods
        private void ShowUstawieniaKontaktowe()
        {
            ShowSingletonWorkspace<NoweUstawieniaKontaktoweViewModel>(
                () => new NoweUstawieniaKontaktoweViewModel(
                    _ustawieniaKontaktoweService));
        }
        private void ShowAllUzytkownicy()
        {
            ShowSingletonWorkspace<WszyscyUzytkownicyViewModel>(
                () => new WszyscyUzytkownicyViewModel(
                    _uzytkownicyService,
                    _klubyService,
                    _trenerzyService,
                    _stanowiskaService,
                    _czlonkostwaUzytkownikaService,
                    _rodzajeCzlonkostwaService,
                    _harmonogramZajecService,
                    _zamowieniaService,
                    _platnosciService,
                    _rodzajeZajecService,
                    _saleService,
                    CreateView));
        }

        private void ShowAllAktualnosci()
        {
            ShowSingletonWorkspace<WszystkieAktualnosciViewModel>(
                () => new WszystkieAktualnosciViewModel(
    _aktualnosciService,
    _klubyService,
    _uzytkownicyService,
    CreateView));
        }

        private void ShowAllKluby()
        {
            ShowSingletonWorkspace<WszystkieKlubyViewModel>(
                () => new WszystkieKlubyViewModel(_klubyService, CreateView));
        }

        private void ShowAllRodzajeCzlonkostwa()
        {
            ShowSingletonWorkspace<WszystkieRodzajeCzlonkostwaViewModel>(
                () => new WszystkieRodzajeCzlonkostwaViewModel(_rodzajeCzlonkostwaService, CreateView));
        }

        private void ShowAllCzlonkostwa()
        {
            ShowSingletonWorkspace<WszystkieCzlonkostwaUzytkownikaViewModel>(
                () => new WszystkieCzlonkostwaUzytkownikaViewModel(
                    _czlonkostwaUzytkownikaService,
                    CreateView
                ));
        }

        private void ShowAllOfertyKlubuCzlonkostwa()
        {
            ShowSingletonWorkspace<WszystkieOfertyKlubuCzlonkostwaViewModel>(
                () => new WszystkieOfertyKlubuCzlonkostwaViewModel(
    _ofertyKlubuCzlonkostwaService,
    _klubyService,
    CreateView));
        }

        private void ShowAllPlatnosci()
        {
            ShowSingletonWorkspace<WszystkiePlatnosciViewModel>(
                () => new WszystkiePlatnosciViewModel(_platnosciService, CreateView));
        }

        private void ShowAllMetodyPlatnosci()
        {
            ShowSingletonWorkspace<WszystkieMetodyPlatnosciViewModel>(
                () => new WszystkieMetodyPlatnosciViewModel(_metodyPlatnosciService, CreateView));
        }

        private void ShowAllRabaty()
        {
            ShowSingletonWorkspace<WszystkieRabatyViewModel>(
                () => new WszystkieRabatyViewModel(_rabatyService, CreateView));
        }

        private void ShowAllZamowienia()
        {
            ShowSingletonWorkspace<WszystkieZamowieniaViewModel>(
                () => new WszystkieZamowieniaViewModel(_zamowieniaService, CreateView));
        }

        private void ShowAllPozycjeZamowienia()
        {
            ShowSingletonWorkspace<WszystkiePozycjeZamowieniaViewModel>(
                () => new WszystkiePozycjeZamowieniaViewModel(_pozycjeZamowieniaService, CreateView));
        }

        private void ShowAllSale()
        {
            ShowSingletonWorkspace<WszystkieSaleViewModel>(
                () => new WszystkieSaleViewModel(_saleService, _klubyService, CreateView));
        }

        private void ShowAllRodzajeZajec()
        {
            ShowSingletonWorkspace<WszystkieRodzajeZajecViewModel>(
                () => new WszystkieRodzajeZajecViewModel(_rodzajeZajecService, CreateView));
        }

        private void ShowAllHarmonogramyZajec()
        {
            ShowSingletonWorkspace<WszystkieHarmonogramyZajecViewModel>(
                () => new WszystkieHarmonogramyZajecViewModel(
    _harmonogramZajecService,
    _klubyService,
    _rodzajeZajecService,
    _saleService,
    _trenerzyService,
    _uzytkownicyService,
    CreateView));
        }

        private void ShowAllRejestracjeNaZajecia()
        {
            ShowSingletonWorkspace<WszystkieRejestracjeNaZajeciaViewModel>(
                () => new WszystkieRejestracjeNaZajeciaViewModel(_rejestracjeNaZajeciaService, CreateView));
        }

        private void ShowAllTrenerzy()
        {
            ShowSingletonWorkspace<WszyscyTrenerzyViewModel>(
                () => new WszyscyTrenerzyViewModel(
                    _trenerzyService,
                    _uzytkownicyService,
                    CreateView));
        }

        private void ShowAllRezerwacjeTrenera()
        {
            ShowSingletonWorkspace<WszystkieRezerwacjeTreneraViewModel>(
                () => new WszystkieRezerwacjeTreneraViewModel(
                    _rezerwacjeTreneraService,
                    _trenerzyService,
                    _uzytkownicyService,
                    _klubyService,
                    CreateView));
        }

        private void ShowAllDostepnosciTrenera()
        {
            ShowSingletonWorkspace<WszystkieDostepnosciTreneraViewModel>(
                () => new WszystkieDostepnosciTreneraViewModel(
                    _dostepnoscTreneraService,
                    _trenerzyService,
                    _klubyService,
                    CreateView));
        }

        private void ShowAllPracownicyKlubu()
        {
            ShowSingletonWorkspace<WszyscyPracownicyKlubuViewModel>(
                () => new WszyscyPracownicyKlubuViewModel(_pracownicyKlubuService, CreateView));
        }

        private void ShowAllStanowiska()
        {
            ShowSingletonWorkspace<WszystkieStanowiskaViewModel>(
                () => new WszystkieStanowiskaViewModel(_stanowiskaService, CreateView));
        }

        private void ShowAllGodzinyOtwarcia()
        {
            ShowSingletonWorkspace<WszystkieGodzinyOtwarciaViewModel>(
                () => new WszystkieGodzinyOtwarciaViewModel(
                    _godzinyOtwarciaService,
                    _klubyService,
                    CreateView));
        }

        private void ShowAllOpinie()
        {
            ShowSingletonWorkspace<WszystkieOpinieViewModel>(
                () => new WszystkieOpinieViewModel(_opinieService));
        }

        private void ShowAllRaporty()
        {
            ShowSingletonWorkspace<WszystkieRaportyViewModel>(
                () => new WszystkieRaportyViewModel(_raportyService));
        }

        #endregion

        #region Command Helpers

        private static ReadOnlyCollection<CommandViewModel> CreateCommandPair(
            string addCommandName,
            Action addAction,
            string showAllCommandName,
            Action showAllAction)
        {
            return new ReadOnlyCollection<CommandViewModel>(
                new List<CommandViewModel>
                {
                    new CommandViewModel(addCommandName, new BaseCommand(addAction)),
                    new CommandViewModel(showAllCommandName, new BaseCommand(showAllAction))
                });
        }

        private static ReadOnlyCollection<CommandViewModel> CreateSingleCommand(
            string commandName,
            Action action)
        {
            return new ReadOnlyCollection<CommandViewModel>(
                new List<CommandViewModel>
                {
                    new CommandViewModel(commandName, new BaseCommand(action))
                });
        }

        #endregion
    }
}