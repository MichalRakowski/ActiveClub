using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Firma.Views;
using ActiveClub.Shared.DTOs.GodzinyOtwarcia;
using ActiveClub.Shared.DTOs.Kluby;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class DzienTygodniaCheckItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class NoweGodzinyOtwarciaViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly GodzinyOtwarciaService _godzinyOtwarciaService;
        private readonly KlubyService _klubyService;

        private readonly UpdateGodzinyOtwarciaDto _item;
        private readonly bool _isEditMode;
        private readonly int _id;

        private string _selectedKlubNazwa = string.Empty;

        public ObservableCollection<DzienTygodniaCheckItem> DniTygodnia { get; set; } = new()
        {
            new DzienTygodniaCheckItem { Id = 1, Nazwa = "Poniedziałek" },
            new DzienTygodniaCheckItem { Id = 2, Nazwa = "Wtorek" },
            new DzienTygodniaCheckItem { Id = 3, Nazwa = "Środa" },
            new DzienTygodniaCheckItem { Id = 4, Nazwa = "Czwartek" },
            new DzienTygodniaCheckItem { Id = 5, Nazwa = "Piątek" },
            new DzienTygodniaCheckItem { Id = 6, Nazwa = "Sobota" },
            new DzienTygodniaCheckItem { Id = 7, Nazwa = "Niedziela" }
        };

        public NoweGodzinyOtwarciaViewModel(
            GodzinyOtwarciaService godzinyOtwarciaService,
            KlubyService klubyService)
        {
            DisplayName = "Nowe godziny otwarcia";

            _godzinyOtwarciaService = godzinyOtwarciaService;
            _klubyService = klubyService;

            _isEditMode = false;
            _id = 0;

            _item = new UpdateGodzinyOtwarciaDto
            {
                DzienTygodnia = 1,
                GodzinaOd = "08:00",
                GodzinaDo = "20:00",
                CzyZamkniete = false
            };
        }

        public NoweGodzinyOtwarciaViewModel(
            GodzinyOtwarciaService godzinyOtwarciaService,
            KlubyService klubyService,
            GodzinyOtwarciaDto dto)
        {
            DisplayName = "Edycja godzin otwarcia";

            _godzinyOtwarciaService = godzinyOtwarciaService;
            _klubyService = klubyService;

            _isEditMode = true;
            _id = dto.GodzinyOtwarciaId;

            _item = new UpdateGodzinyOtwarciaDto
            {
                KlubId = dto.KlubId,
                DzienTygodnia = dto.DzienTygodnia,
                GodzinaOd = dto.GodzinaOd,
                GodzinaDo = dto.GodzinaDo,
                CzyZamkniete = dto.CzyZamkniete
            };

            var selectedDay = DniTygodnia.FirstOrDefault(x => x.Id == dto.DzienTygodnia);
            if (selectedDay != null)
                selectedDay.IsSelected = true;

            _ = LoadSelectedKlubNameAsync(dto.KlubId);
        }

        public int SelectedKlubId
        {
            get => _item.KlubId;
            set
            {
                _item.KlubId = value;
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

        public string? GodzinaOdText
        {
            get => _item.GodzinaOd;
            set
            {
                _item.GodzinaOd = value;
                OnPropertyChanged(nameof(GodzinaOdText));
            }
        }

        public string? GodzinaDoText
        {
            get => _item.GodzinaDo;
            set
            {
                _item.GodzinaDo = value;
                OnPropertyChanged(nameof(GodzinaDoText));
            }
        }

        public bool CzyZamkniete
        {
            get => _item.CzyZamkniete;
            set
            {
                _item.CzyZamkniete = value;
                OnPropertyChanged(nameof(CzyZamkniete));
            }
        }

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand =>
            _cancelCommand ??= new BaseCommand(Cancel);

        private BaseCommand? _wybierzKlubCommand;
        public ICommand WybierzKlubCommand =>
            _wybierzKlubCommand ??= new BaseCommand(() => _ = WybierzKlubAsync());

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
                MessageBox.Show(ex.Message, "Błąd wyboru klubu", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (!DniTygodnia.Any(x => x.IsSelected))
            {
                message = "Wybierz przynajmniej jeden dzień tygodnia.";
                return false;
            }

            if (_isEditMode && DniTygodnia.Count(x => x.IsSelected) > 1)
            {
                message = "Podczas edycji możesz wybrać tylko jeden dzień.";
                return false;
            }

            if (!CzyZamkniete)
            {
                if (string.IsNullOrWhiteSpace(_item.GodzinaOd))
                {
                    message = "Podaj godzinę otwarcia w formacie HH:mm.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(_item.GodzinaDo))
                {
                    message = "Podaj godzinę zamknięcia w formacie HH:mm.";
                    return false;
                }

                if (!TimeOnly.TryParse(_item.GodzinaOd, out var godzinaOd))
                {
                    message = "Podaj poprawną godzinę otwarcia w formacie HH:mm.";
                    return false;
                }

                if (!TimeOnly.TryParse(_item.GodzinaDo, out var godzinaDo))
                {
                    message = "Podaj poprawną godzinę zamknięcia w formacie HH:mm.";
                    return false;
                }

                if (godzinaDo <= godzinaOd)
                {
                    message = "Godzina zamknięcia musi być późniejsza niż godzina otwarcia.";
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private CreateGodzinyOtwarciaDto BuildCreateDto(int dzienTygodnia)
        {
            return new CreateGodzinyOtwarciaDto
            {
                KlubId = SelectedKlubId,
                DzienTygodnia = (byte)dzienTygodnia,
                GodzinaOd = CzyZamkniete ? null : _item.GodzinaOd,
                GodzinaDo = CzyZamkniete ? null : _item.GodzinaDo,
                CzyZamkniete = CzyZamkniete
            };
        }

        private UpdateGodzinyOtwarciaDto BuildUpdateDto(int dzienTygodnia)
        {
            return new UpdateGodzinyOtwarciaDto
            {
                KlubId = SelectedKlubId,
                DzienTygodnia = (byte)dzienTygodnia,
                GodzinaOd = CzyZamkniete ? null : _item.GodzinaOd,
                GodzinaDo = CzyZamkniete ? null : _item.GodzinaDo,
                CzyZamkniete = CzyZamkniete
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
                var selectedDays = DniTygodnia
                    .Where(x => x.IsSelected)
                    .Select(x => x.Id)
                    .ToList();

                if (_isEditMode)
                {
                    var day = selectedDays.First();

                    var success = await _godzinyOtwarciaService.UpdateAsync(_id, BuildUpdateDto(day));

                    if (!success)
                    {
                        MessageBox.Show("Nie udało się zapisać godzin otwarcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    foreach (var day in selectedDays)
                    {
                        var success = await _godzinyOtwarciaService.CreateAsync(BuildCreateDto(day));

                        if (!success)
                        {
                            MessageBox.Show($"Nie udało się zapisać godzin dla dnia: {day}.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
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