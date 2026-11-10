using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.Sale;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaSalaViewModel : WorkspaceViewModel
    {
        public event EventHandler? SalaSaved;

        private readonly SaleService _saleService;
        private readonly KlubyService _klubyService;
        private readonly UpdateSalaDto _sala;
        private readonly bool _isEditMode;
        private readonly int _salaId;

        private string _selectedKlubNazwa = string.Empty;

        public NowaSalaViewModel(SaleService saleService, KlubyService klubyService)
        {
            DisplayName = "Nowa sala";

            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));

            _isEditMode = false;
            _salaId = 0;

            _sala = new UpdateSalaDto
            {
                KlubId = 0,
                Nazwa = string.Empty,
                Pojemnosc = null,
                Opis = string.Empty,
                CzyAktywna = true
            };
        }

        public NowaSalaViewModel(SaleService saleService, KlubyService klubyService, SalaDto dto)
        {
            DisplayName = "Edycja sali";

            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));

            _isEditMode = true;
            _salaId = dto.SalaId;

            _sala = new UpdateSalaDto
            {
                KlubId = dto.KlubId,
                Nazwa = dto.Nazwa ?? string.Empty,
                Pojemnosc = dto.Pojemnosc,
                Opis = dto.Opis ?? string.Empty,
                CzyAktywna = dto.CzyAktywna
            };

            _ = LoadSelectedKlubNameAsync(dto.KlubId);
        }

        public int SelectedKlubId
        {
            get => _sala.KlubId;
            set
            {
                _sala.KlubId = value;
                OnPropertyChanged(nameof(SelectedKlubId));
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

        public string Nazwa
        {
            get => _sala.Nazwa ?? string.Empty;
            set
            {
                _sala.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string PojemnoscText
        {
            get => _sala.Pojemnosc?.ToString() ?? string.Empty;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _sala.Pojemnosc = null;
                else if (int.TryParse(value, out var parsed))
                    _sala.Pojemnosc = parsed;

                OnPropertyChanged(nameof(PojemnoscText));
            }
        }

        public string Opis
        {
            get => _sala.Opis ?? string.Empty;
            set
            {
                _sala.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public bool CzyAktywna
        {
            get => _sala.CzyAktywna;
            set
            {
                _sala.CzyAktywna = value;
                OnPropertyChanged(nameof(CzyAktywna));
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

        private void ApplyKlub(KlubDto klub)
        {
            SelectedKlubId = klub.KlubId;
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
            if (SelectedKlubId <= 0)
            {
                message = "Wybierz klub.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa sali jest wymagana.";
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
                    success = await _saleService.UpdateAsync(_salaId, _sala);
                }
                else
                {
                    var createDto = new CreateSalaDto
                    {
                        KlubId = _sala.KlubId,
                        Nazwa = _sala.Nazwa,
                        Pojemnosc = _sala.Pojemnosc,
                        Opis = _sala.Opis,
                        CzyAktywna = _sala.CzyAktywna
                    };

                    success = await _saleService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        "Nie udało się zapisać sali.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                SalaSaved?.Invoke(this, EventArgs.Empty);
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