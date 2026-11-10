using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaOfertaKlubuCzlonkostwoViewModel : WorkspaceViewModel
    {
        public event EventHandler? OfertaSaved;

        private readonly OfertyKlubuCzlonkostwaService _service;
        private readonly KlubyService _klubyService;

        private readonly UpdateOfertaKlubuCzlonkostwoDto _oferta;
        private readonly bool _isEditMode;
        private readonly int _ofertaId;

        private string _selectedKlubNazwa = string.Empty;
        private LookupItemDto? _selectedRodzajCzlonkostwa;

        public ObservableCollection<LookupItemDto> RodzajeCzlonkostwa { get; set; } = new();

        public NowaOfertaKlubuCzlonkostwoViewModel(
            OfertyKlubuCzlonkostwaService service,
            KlubyService klubyService)
        {
            DisplayName = "Nowa oferta członkostwa";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));

            _isEditMode = false;
            _ofertaId = 0;

            _oferta = new UpdateOfertaKlubuCzlonkostwoDto
            {
                KlubId = 0,
                RodzajCzlonkostwaId = 0,
                Cena = 0,
                Waluta = "PLN",
                CzyAktywna = true,
                CzyWidoczneOnline = true,
                DataOd = null,
                DataDo = null,
                Kolejnosc = null,
                OpisMarketingowy = string.Empty
            };

            _ = LoadFormDataAsync();
        }

        public NowaOfertaKlubuCzlonkostwoViewModel(
            OfertyKlubuCzlonkostwaService service,
            KlubyService klubyService,
            OfertaKlubuCzlonkostwoDto dto)
        {
            DisplayName = "Edycja oferty członkostwa";

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));

            _isEditMode = true;
            _ofertaId = dto.OfertaKlubuCzlonkostwoId;

            _oferta = new UpdateOfertaKlubuCzlonkostwoDto
            {
                KlubId = dto.KlubId,
                RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId,
                Cena = dto.Cena,
                Waluta = dto.Waluta,
                CzyAktywna = dto.CzyAktywna,
                CzyWidoczneOnline = dto.CzyWidoczneOnline,
                DataOd = dto.DataOd,
                DataDo = dto.DataDo,
                Kolejnosc = dto.Kolejnosc,
                OpisMarketingowy = dto.OpisMarketingowy
            };

            _ = LoadFormDataAsync();
            _ = LoadSelectedKlubNameAsync(dto.KlubId);
        }

        public int KlubId
        {
            get => _oferta.KlubId;
            set
            {
                _oferta.KlubId = value;
                OnPropertyChanged(nameof(KlubId));
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

        public LookupItemDto? SelectedRodzajCzlonkostwa
        {
            get => _selectedRodzajCzlonkostwa;
            set
            {
                _selectedRodzajCzlonkostwa = value;
                _oferta.RodzajCzlonkostwaId = value?.Id ?? 0;
                OnPropertyChanged(nameof(SelectedRodzajCzlonkostwa));
            }
        }

        public decimal Cena
        {
            get => _oferta.Cena;
            set
            {
                _oferta.Cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public string Waluta
        {
            get => _oferta.Waluta ?? string.Empty;
            set
            {
                _oferta.Waluta = value;
                OnPropertyChanged(nameof(Waluta));
            }
        }

        public bool CzyAktywna
        {
            get => _oferta.CzyAktywna;
            set
            {
                _oferta.CzyAktywna = value;
                OnPropertyChanged(nameof(CzyAktywna));
            }
        }

        public bool CzyWidoczneOnline
        {
            get => _oferta.CzyWidoczneOnline;
            set
            {
                _oferta.CzyWidoczneOnline = value;
                OnPropertyChanged(nameof(CzyWidoczneOnline));
            }
        }

        public DateTime? DataOd
        {
            get => _oferta.DataOd;
            set
            {
                _oferta.DataOd = value;
                OnPropertyChanged(nameof(DataOd));
            }
        }

        public DateTime? DataDo
        {
            get => _oferta.DataDo;
            set
            {
                _oferta.DataDo = value;
                OnPropertyChanged(nameof(DataDo));
            }
        }

        public int? Kolejnosc
        {
            get => _oferta.Kolejnosc;
            set
            {
                _oferta.Kolejnosc = value;
                OnPropertyChanged(nameof(Kolejnosc));
            }
        }

        public string? OpisMarketingowy
        {
            get => _oferta.OpisMarketingowy;
            set
            {
                _oferta.OpisMarketingowy = value;
                OnPropertyChanged(nameof(OpisMarketingowy));
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
            _wybierzKlubCommand ??= new BaseCommand(async () => await WybierzKlubAsync());

        private async Task LoadFormDataAsync()
        {
            try
            {
                var data = await _service.GetFormDataAsync();

                if (data == null)
                    return;

                RodzajeCzlonkostwa = new ObservableCollection<LookupItemDto>(
                    data.RodzajeCzlonkostwa ?? Enumerable.Empty<LookupItemDto>());

                OnPropertyChanged(nameof(RodzajeCzlonkostwa));

                SelectedRodzajCzlonkostwa = RodzajeCzlonkostwa
                    .FirstOrDefault(x => x.Id == _oferta.RodzajCzlonkostwaId);
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
                    ApplyKlub(vm.SelectedItem);
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

        private void ApplyKlub(KlubDto klub)
        {
            KlubId = klub.KlubId;
            SelectedKlubNazwa = klub.Nazwa;
        }

        private async Task LoadSelectedKlubNameAsync(int klubId)
        {
            try
            {
                var kluby = await _klubyService.GetAllAsync();
                var klub = kluby.FirstOrDefault(k => k.KlubId == klubId);

                if (klub != null)
                    ApplyKlub(klub);
            }
            catch
            {
                SelectedKlubNazwa = string.Empty;
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (_oferta.KlubId <= 0)
            {
                message = "Klub jest wymagany.";
                return false;
            }

            if (_oferta.RodzajCzlonkostwaId <= 0)
            {
                message = "Rodzaj członkostwa jest wymagany.";
                return false;
            }

            if (Cena <= 0)
            {
                message = "Cena musi być większa od 0.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Waluta))
            {
                message = "Waluta jest wymagana.";
                return false;
            }

            if (DataOd.HasValue && DataDo.HasValue && DataOd > DataDo)
            {
                message = "Data od nie może być większa niż data do.";
                return false;
            }

            if (Kolejnosc.HasValue && Kolejnosc < 0)
            {
                message = "Kolejność nie może być ujemna.";
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
                    success = await _service.UpdateAsync(_ofertaId, _oferta);
                }
                else
                {
                    var createDto = new CreateOfertaKlubuCzlonkostwoDto
                    {
                        KlubId = _oferta.KlubId,
                        RodzajCzlonkostwaId = _oferta.RodzajCzlonkostwaId,
                        Cena = _oferta.Cena,
                        Waluta = _oferta.Waluta,
                        CzyAktywna = _oferta.CzyAktywna,
                        CzyWidoczneOnline = _oferta.CzyWidoczneOnline,
                        DataOd = _oferta.DataOd,
                        DataDo = _oferta.DataDo,
                        Kolejnosc = _oferta.Kolejnosc,
                        OpisMarketingowy = _oferta.OpisMarketingowy
                    };

                    success = await _service.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać oferty członkostwa.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                OfertaSaved?.Invoke(this, EventArgs.Empty);
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