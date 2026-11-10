using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.Trenerzy;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyTrenerViewModel : WorkspaceViewModel
    {
        public event EventHandler? TrainerSaved;

        private readonly TrenerzyService _trenerzyService;
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly UpdateTrenerDto _trener;

        private readonly bool _isEditMode;
        private readonly int _id;

        private string _selectedUzytkownikNazwa = string.Empty;
        private string _email = string.Empty;
        private string _telefon = string.Empty;

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzUzytkownikaCommand;
        public ICommand WybierzUzytkownikaCommand =>
            _wybierzUzytkownikaCommand ??= new BaseCommand(() => _ = WybierzUzytkownika());

        public NowyTrenerViewModel(
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService)
        {
            DisplayName = "Nowy trener";

            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;

            _isEditMode = false;
            _id = 0;

            _trener = new UpdateTrenerDto
            {
                CzyAktywny = true
            };
        }

        public NowyTrenerViewModel(
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            TrenerDto dto)
        {
            DisplayName = "Edycja trenera";

            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;

            _isEditMode = true;
            _id = dto.TrenerId;

            _trener = new UpdateTrenerDto
            {
                UzytkownikId = dto.UzytkownikId,
                Imie = dto.Imie,
                Nazwisko = dto.Nazwisko,
                Bio = dto.Bio,
                SpecjalizacjaOpis = dto.SpecjalizacjaOpis,
                StawkaZaGodzine = dto.StawkaZaGodzine,
                CzyAktywny = dto.CzyAktywny
            };

            SelectedUzytkownikNazwa = dto.ImieNazwisko;
            Email = dto.Email ?? string.Empty;
            Telefon = dto.Telefon ?? string.Empty;
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

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Telefon
        {
            get => _telefon;
            set
            {
                _telefon = value;
                OnPropertyChanged(nameof(Telefon));
            }
        }

        public string Imie
        {
            get => _trener.Imie;
            set
            {
                _trener.Imie = value;
                OnPropertyChanged(nameof(Imie));
            }
        }

        public string Nazwisko
        {
            get => _trener.Nazwisko;
            set
            {
                _trener.Nazwisko = value;
                OnPropertyChanged(nameof(Nazwisko));
            }
        }

        public int? UzytkownikId
        {
            get => _trener.UzytkownikId;
            set
            {
                _trener.UzytkownikId = value;
                OnPropertyChanged(nameof(UzytkownikId));
            }
        }

        public string? Bio
        {
            get => _trener.Bio;
            set
            {
                _trener.Bio = value;
                OnPropertyChanged(nameof(Bio));
            }
        }

        public string? SpecjalizacjaOpis
        {
            get => _trener.SpecjalizacjaOpis;
            set
            {
                _trener.SpecjalizacjaOpis = value;
                OnPropertyChanged(nameof(SpecjalizacjaOpis));
            }
        }

        public decimal? StawkaZaGodzine
        {
            get => _trener.StawkaZaGodzine;
            set
            {
                _trener.StawkaZaGodzine = value;
                OnPropertyChanged(nameof(StawkaZaGodzine));
            }
        }

        public bool CzyAktywny
        {
            get => _trener.CzyAktywny;
            set
            {
                _trener.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }

        public string SaveButtonText => _isEditMode ? "Zapisz zmiany" : "Dodaj";

        public bool IsCreateMode => !_isEditMode;

        private async Task WybierzUzytkownika()
        {
            try
            {
                var users = await _uzytkownicyService.GetAllAsync();

                var viewModel = new WybierzUzytkownikaViewModel(users);

                var window = new WybierzUzytkownikaWindow(viewModel)
                {
                    Owner = Application.Current.MainWindow
                };

                var result = window.ShowDialog();

                if (result == true && viewModel.SelectedUser != null)
                {
                    ApplySelectedUser(viewModel.SelectedUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Błąd pobierania użytkowników",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplySelectedUser(UzytkownikDto user)
        {
            UzytkownikId = user.Id;

            Imie = user.Imie;
            Nazwisko = user.Nazwisko;

            Email = user.Email;
            Telefon = user.Telefon ?? string.Empty;

            SelectedUzytkownikNazwa = user.DisplayName;
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (UzytkownikId == null)
            {
                message = "Wybierz użytkownika.";
                return false;
            }

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

            message = string.Empty;
            return true;
        }

        private CreateTrenerDto BuildCreateDto()
        {
            return new CreateTrenerDto
            {
                UzytkownikId = UzytkownikId,
                Imie = Imie,
                Nazwisko = Nazwisko,
                Bio = Bio,
                SpecjalizacjaOpis = SpecjalizacjaOpis,
                StawkaZaGodzine = StawkaZaGodzine,
                CzyAktywny = CzyAktywny
            };
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
                bool success = _isEditMode
                    ? await _trenerzyService.UpdateAsync(_id, _trener)
                    : await _trenerzyService.CreateAsync(BuildCreateDto());

                if (!success)
                {
                    MessageBox.Show(
                        _isEditMode
                            ? "Nie udało się zapisać zmian trenera."
                            : "Nie udało się zapisać trenera.",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                TrainerSaved?.Invoke(this, EventArgs.Empty);
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