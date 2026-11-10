using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.Raporty;
using ActiveClub.Shared.DTOs.Trenerzy;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyRaportViewModel : WorkspaceViewModel
    {
        public event EventHandler? RaportSaved;

        private readonly RaportyService _raportyService;
        private readonly GenerateRaportDto _raport;

        private string _selectedKlubNazwa = string.Empty;
        private string _selectedTrenerNazwa = string.Empty;

        public NowyRaportViewModel(RaportyService raportyService)
        {
            DisplayName = "Generuj raport";

            _raportyService = raportyService ?? throw new ArgumentNullException(nameof(raportyService));

            _raport = new GenerateRaportDto
            {
                Format = "PDF",
                DataOd = DateTime.Today.AddMonths(-1),
                DataDo = DateTime.Today
            };

            DefinicjeRaportow = new ObservableCollection<LookupItemDto>();
            Kluby = new ObservableCollection<LookupItemDto>();
            Trenerzy = new ObservableCollection<LookupItemDto>();
            Formaty = new ObservableCollection<LookupItemDto>();

            _ = LoadFormDataAsync();
        }

        public ObservableCollection<LookupItemDto> DefinicjeRaportow { get; set; }
        public ObservableCollection<LookupItemDto> Kluby { get; set; }
        public ObservableCollection<LookupItemDto> Trenerzy { get; set; }
        public ObservableCollection<LookupItemDto> Formaty { get; set; }

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

        public int RaportDefinicjaId
        {
            get => _raport.RaportDefinicjaId;
            set
            {
                _raport.RaportDefinicjaId = value;
                OnPropertyChanged(nameof(RaportDefinicjaId));
            }
        }

        public string Format
        {
            get => _raport.Format;
            set
            {
                _raport.Format = value;
                OnPropertyChanged(nameof(Format));
            }
        }

        public DateTime? DataOd
        {
            get => _raport.DataOd;
            set
            {
                _raport.DataOd = value;
                OnPropertyChanged(nameof(DataOd));
            }
        }

        public DateTime? DataDo
        {
            get => _raport.DataDo;
            set
            {
                _raport.DataDo = value;
                OnPropertyChanged(nameof(DataDo));
            }
        }

        public int? KlubId
        {
            get => _raport.KlubId;
            set
            {
                _raport.KlubId = value;
                OnPropertyChanged(nameof(KlubId));

                var selected = Kluby.FirstOrDefault(x => x.Id == value);
                SelectedKlubNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        public int? TrenerId
        {
            get => _raport.TrenerId;
            set
            {
                _raport.TrenerId = value;
                OnPropertyChanged(nameof(TrenerId));

                var selected = Trenerzy.FirstOrDefault(x => x.Id == value);
                SelectedTrenerNazwa = selected?.Nazwa ?? string.Empty;
            }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(WybierzKlub);

        private BaseCommand? _wybierzTreneraCommand;
        public ICommand WybierzTreneraCommand =>
            _wybierzTreneraCommand ??= new BaseCommand(WybierzTrenera);

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

        private void WybierzTrenera()
        {
            var trenerzy = Trenerzy
                .Where(x => x.Id > 0)
                .Select(x => new TrenerDto
                {
                    TrenerId = x.Id,
                    Imie = x.Nazwa,
                    Nazwisko = string.Empty
                })
                .ToList();

            var vm = new WybierzTreneraViewModel(trenerzy);

            var window = new WybierzTreneraWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (window.ShowDialog() == true && vm.SelectedItem != null)
            {
                TrenerId = vm.SelectedItem.TrenerId;
                SelectedTrenerNazwa = vm.SelectedItem.ImieNazwisko;
            }
        }

        private async Task LoadFormDataAsync()
        {
            try
            {
                var data = await _raportyService.GetFormDataAsync();

                if (data == null)
                    return;

                DefinicjeRaportow = new ObservableCollection<LookupItemDto>(
                    data.DefinicjeRaportow ?? Enumerable.Empty<LookupItemDto>());

                Kluby = new ObservableCollection<LookupItemDto>(
                    data.Kluby ?? Enumerable.Empty<LookupItemDto>());

                Trenerzy = new ObservableCollection<LookupItemDto>(
                    data.Trenerzy ?? Enumerable.Empty<LookupItemDto>());

                Formaty = new ObservableCollection<LookupItemDto>(
                    data.Formaty ?? Enumerable.Empty<LookupItemDto>());

                if (Formaty.Count > 0 && string.IsNullOrWhiteSpace(Format))
                    Format = Formaty[0].Nazwa;

                SelectedKlubNazwa =
                    Kluby.FirstOrDefault(x => x.Id == KlubId)?.Nazwa ?? string.Empty;

                SelectedTrenerNazwa =
                    Trenerzy.FirstOrDefault(x => x.Id == TrenerId)?.Nazwa ?? string.Empty;

                OnPropertyChanged(nameof(DefinicjeRaportow));
                OnPropertyChanged(nameof(Kluby));
                OnPropertyChanged(nameof(Trenerzy));
                OnPropertyChanged(nameof(Formaty));
                OnPropertyChanged(nameof(KlubId));
                OnPropertyChanged(nameof(TrenerId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd ładowania danych formularza: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (RaportDefinicjaId <= 0)
            {
                message = "Wybierz raport.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Format))
            {
                message = "Wybierz format.";
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
                var result = await _raportyService.GenerateAsync(_raport);

                if (result == null || !result.Success)
                {
                    MessageBox.Show(
                        result?.Message ?? "Nie udało się wygenerować raportu.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                MessageBox.Show(
                    $"Raport został wygenerowany.\n\nPlik PDF:\n{result.PlikUrl}",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                RaportSaved?.Invoke(this, EventArgs.Empty);
                OnRequestClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd generowania",
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