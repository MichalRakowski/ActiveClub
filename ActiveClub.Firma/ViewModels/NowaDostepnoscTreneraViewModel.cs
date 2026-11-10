using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.DostepnoscTrenera;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.Trenerzy;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaDostepnoscTreneraViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly DostepnoscTreneraService _service;
        private readonly TrenerzyService _trenerzyService;
        private readonly KlubyService _klubyService;

        private readonly bool _isEditMode;
        private readonly int _id;

        private int _selectedTrenerId;
        private int? _selectedKlubId;
        private string? _selectedStatus;

        private string _selectedTrenerNazwa = string.Empty;
        private string _selectedKlubNazwa = string.Empty;

        private DateTime _startDate;
        private string _startTime = "08:00";
        private DateTime _endDate;
        private string _endTime = "09:00";

        public ObservableCollection<string> Statusy { get; set; } = new()
        {
            "DOSTEPNY",
    "NIEDOSTEPNY",
    "URLOP"
        };

        public NowaDostepnoscTreneraViewModel(
            DostepnoscTreneraService service,
            TrenerzyService trenerzyService,
            KlubyService klubyService)
        {
            DisplayName = "Nowa dostępność trenera";

            _service = service;
            _trenerzyService = trenerzyService;
            _klubyService = klubyService;

            _isEditMode = false;
            _id = 0;

            _startDate = DateTime.Now.Date;
            _endDate = DateTime.Now.Date;
            _selectedStatus = "DOSTEPNY";
        }

        public NowaDostepnoscTreneraViewModel(
            DostepnoscTreneraService service,
            TrenerzyService trenerzyService,
            KlubyService klubyService,
            DostepnoscTreneraDto dto)
        {
            DisplayName = "Edycja dostępności trenera";

            _service = service;
            _trenerzyService = trenerzyService;
            _klubyService = klubyService;

            _isEditMode = true;
            _id = dto.DostepnoscId;

            _selectedTrenerId = dto.TrenerId;
            _selectedKlubId = dto.KlubId;
            _selectedStatus = dto.Status;

            _startDate = dto.StartAt.Date;
            _endDate = dto.EndAt.Date;
            _startTime = dto.StartAt.ToString("HH:mm");
            _endTime = dto.EndAt.ToString("HH:mm");

            _ = LoadSelectedNamesAsync();
        }

        public int SelectedTrenerId
        {
            get => _selectedTrenerId;
            set
            {
                _selectedTrenerId = value;
                OnPropertyChanged(nameof(SelectedTrenerId));
            }
        }

        public int? SelectedKlubId
        {
            get => _selectedKlubId;
            set
            {
                _selectedKlubId = value;
                OnPropertyChanged(nameof(SelectedKlubId));
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

        public string SelectedKlubNazwa
        {
            get => _selectedKlubNazwa;
            set
            {
                _selectedKlubNazwa = value;
                OnPropertyChanged(nameof(SelectedKlubNazwa));
            }
        }

        public string? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public string StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public string EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzTreneraCommand;
        public ICommand WybierzTreneraCommand =>
            _wybierzTreneraCommand ??= new BaseCommand(() => _ = WybierzTreneraAsync());

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(() => _ = WybierzKlubAsync());

        private async Task WybierzTreneraAsync()
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
                    ApplyTrener(vm.SelectedItem);
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

        private async Task WybierzKlubAsync()
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
                    ApplyKlub(vm.SelectedItem);
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

        private void ApplyTrener(TrenerDto trener)
        {
            SelectedTrenerId = trener.TrenerId;
            SelectedTrenerNazwa = trener.ImieNazwisko;
        }

        private void ApplyKlub(KlubDto klub)
        {
            SelectedKlubId = klub.KlubId;
            SelectedKlubNazwa = klub.Nazwa;
        }

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

                if (SelectedKlubId.HasValue && SelectedKlubId.Value > 0)
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

        private bool TryBuildDates(out DateTime startAt, out DateTime endAt, out string message)
        {
            startAt = default;
            endAt = default;

            if (!TimeSpan.TryParseExact(StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out var startTime))
            {
                message = "Niepoprawna godzina rozpoczęcia. Użyj formatu HH:mm.";
                return false;
            }

            if (!TimeSpan.TryParseExact(EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out var endTime))
            {
                message = "Niepoprawna godzina zakończenia. Użyj formatu HH:mm.";
                return false;
            }

            startAt = StartDate.Date.Add(startTime);
            endAt = EndDate.Date.Add(endTime);

            if (endAt <= startAt)
            {
                message = "Data i godzina zakończenia muszą być późniejsze niż rozpoczęcie.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (SelectedTrenerId <= 0)
            {
                message = "Wybierz trenera.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedStatus))
            {
                message = "Wybierz status.";
                return false;
            }

            if (!TryBuildDates(out _, out _, out message))
                return false;

            return true;
        }

        private CreateDostepnoscTreneraDto BuildCreateDto()
        {
            TryBuildDates(out var startAt, out var endAt, out _);

            return new CreateDostepnoscTreneraDto
            {
                TrenerId = SelectedTrenerId,
                KlubId = SelectedKlubId,
                StartAt = startAt,
                EndAt = endAt,
                Status = SelectedStatus
            };
        }

        private UpdateDostepnoscTreneraDto BuildUpdateDto()
        {
            TryBuildDates(out var startAt, out var endAt, out _);

            return new UpdateDostepnoscTreneraDto
            {
                TrenerId = SelectedTrenerId,
                KlubId = SelectedKlubId,
                StartAt = startAt,
                EndAt = endAt,
                Status = SelectedStatus
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
                    ? await _service.UpdateAsync(_id, BuildUpdateDto())
                    : await _service.CreateAsync(BuildCreateDto());

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać dostępności trenera.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

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