using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Aktualnosci;
using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaAktualnoscViewModel : WorkspaceViewModel
    {
        private readonly AktualnosciService _aktualnosciService;
        private readonly KlubyService _klubyService;
        private readonly UzytkownicyService _uzytkownicyService;

        private readonly UpdateAktualnoscDto _aktualnosc;
        private readonly bool _isEditMode;

        private string _selectedKlubNazwa = string.Empty;
        private string _selectedUzytkownikNazwa = string.Empty;

        public event Action? SavedSuccessfully;

        public NowaAktualnoscViewModel(
            AktualnosciService aktualnosciService,
            KlubyService klubyService,
            UzytkownicyService uzytkownicyService)
        {
            DisplayName = "Nowa aktualność";

            _aktualnosciService = aktualnosciService;
            _klubyService = klubyService;
            _uzytkownicyService = uzytkownicyService;

            _isEditMode = false;

            _aktualnosc = new UpdateAktualnoscDto
            {
                DataDodania = DateTime.Now,
                DataPublikacji = DateTime.Now,
                CzyWidoczna = true
            };
        }

        public NowaAktualnoscViewModel(
            AktualnosciService aktualnosciService,
            KlubyService klubyService,
            UzytkownicyService uzytkownicyService,
            AktualnoscDto dto)
        {
            DisplayName = "Edycja aktualności";

            _aktualnosciService = aktualnosciService;
            _klubyService = klubyService;
            _uzytkownicyService = uzytkownicyService;

            _isEditMode = true;

            _aktualnosc = new UpdateAktualnoscDto
            {
                AktualnoscId = dto.AktualnoscId,
                Tytul = dto.Tytul,
                Tresc = dto.Tresc,
                DataDodania = dto.DataDodania ?? DateTime.Now,
                DataPublikacji = dto.DataPublikacji,
                KlubId = dto.KlubId,
                DodanePrzezUzytkownikId = dto.DodanePrzezUzytkownikId,
                CzyWidoczna = dto.CzyWidoczna
            };

            if (dto.KlubId.HasValue)
                _ = LoadSelectedKlubNameAsync(dto.KlubId.Value);

            if (dto.DodanePrzezUzytkownikId.HasValue)
                _ = LoadSelectedUzytkownikNameAsync(dto.DodanePrzezUzytkownikId.Value);
        }

        public string Tytul
        {
            get => _aktualnosc.Tytul;
            set
            {
                _aktualnosc.Tytul = value;
                OnPropertyChanged(nameof(Tytul));
            }
        }

        public string Tresc
        {
            get => _aktualnosc.Tresc ?? string.Empty;
            set
            {
                _aktualnosc.Tresc = value;
                OnPropertyChanged(nameof(Tresc));
            }
        }

        public DateTime DataDodania
        {
            get => _aktualnosc.DataDodania;
            set
            {
                _aktualnosc.DataDodania = value;
                OnPropertyChanged(nameof(DataDodania));
            }
        }

        public DateTime? DataPublikacji
        {
            get => _aktualnosc.DataPublikacji;
            set
            {
                _aktualnosc.DataPublikacji = value;
                OnPropertyChanged(nameof(DataPublikacji));
            }
        }

        public int? KlubId
        {
            get => _aktualnosc.KlubId;
            set
            {
                _aktualnosc.KlubId = value;
                OnPropertyChanged(nameof(KlubId));
            }
        }

        public int? DodanePrzezUzytkownikId
        {
            get => _aktualnosc.DodanePrzezUzytkownikId;
            set
            {
                _aktualnosc.DodanePrzezUzytkownikId = value;
                OnPropertyChanged(nameof(DodanePrzezUzytkownikId));
            }
        }

        public bool CzyWidoczna
        {
            get => _aktualnosc.CzyWidoczna.GetValueOrDefault();
            set
            {
                _aktualnosc.CzyWidoczna = value;
                OnPropertyChanged(nameof(CzyWidoczna));
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

        public string SelectedUzytkownikNazwa
        {
            get => _selectedUzytkownikNazwa;
            set
            {
                _selectedUzytkownikNazwa = value;
                OnPropertyChanged(nameof(SelectedUzytkownikNazwa));
            }
        }

        public string SaveButtonText => _isEditMode ? "Zapisz zmiany" : "Dodaj";

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(async () => await SaveAndCloseAsync());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(async () => await WybierzKlubAsync());

        private BaseCommand? _wybierzUzytkownikaCommand;
        public ICommand WybierzUzytkownikaCommand =>
            _wybierzUzytkownikaCommand ??= new BaseCommand(async () => await WybierzUzytkownikaAsync());

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

        private async Task WybierzUzytkownikaAsync()
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

        private void ApplyUzytkownik(UzytkownikDto user)
        {
            DodanePrzezUzytkownikId = user.Id;
            SelectedUzytkownikNazwa = user.DisplayName;
        }

        private async Task LoadSelectedKlubNameAsync(int klubId)
        {
            var kluby = await _klubyService.GetAllAsync();
            var klub = kluby.FirstOrDefault(x => x.KlubId == klubId);

            if (klub != null)
                ApplyKlub(klub);
        }

        private async Task LoadSelectedUzytkownikNameAsync(int userId)
        {
            var user = await _uzytkownicyService.GetByIdAsync(userId);

            if (user != null)
                ApplyUzytkownik(user);
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Tytul))
            {
                message = "Tytuł jest wymagany.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Tresc))
            {
                message = "Treść jest wymagana.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public async Task SaveAndCloseAsync()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(msg, "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_aktualnosc.DataDodania == default)
                    _aktualnosc.DataDodania = DateTime.Now;

                bool success;

                if (_isEditMode)
                {
                    success = await _aktualnosciService.UpdateAsync(
                        _aktualnosc.AktualnoscId,
                        _aktualnosc);
                }
                else
                {
                    var createDto = new CreateAktualnoscDto
                    {
                        Tytul = _aktualnosc.Tytul,
                        Tresc = _aktualnosc.Tresc,
                        DataDodania = _aktualnosc.DataDodania,
                        DataPublikacji = _aktualnosc.DataPublikacji,
                        KlubId = _aktualnosc.KlubId,
                        DodanePrzezUzytkownikId = _aktualnosc.DodanePrzezUzytkownikId,
                        CzyWidoczna = _aktualnosc.CzyWidoczna
                    };

                    success = await _aktualnosciService.CreateAsync(createDto);
                }

                if (!success)
                {
                    MessageBox.Show(
                        _isEditMode ? "Nie udało się zapisać zmian." : "Nie udało się zapisać aktualności.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                SavedSuccessfully?.Invoke();
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