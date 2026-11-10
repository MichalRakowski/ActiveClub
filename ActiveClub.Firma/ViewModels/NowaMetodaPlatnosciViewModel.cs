using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.MetodyPlatnosci;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowaMetodaPlatnosciViewModel : WorkspaceViewModel
    {
        public event EventHandler? Saved;

        private readonly MetodyPlatnosciService _service;
        private readonly UpdateMetodaPlatnosciDto _item;
        private readonly bool _isEditMode;
        private readonly int _id;

        private BaseCommand? _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(() => _ = SaveAndClose());

        private BaseCommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(Cancel);

        public NowaMetodaPlatnosciViewModel(MetodyPlatnosciService service)
        {
            DisplayName = "Nowa metoda płatności";
            _service = service;
            _isEditMode = false;
            _id = 0;

            _item = new UpdateMetodaPlatnosciDto
            {
                CzyAktywna = true
            };
        }

        public NowaMetodaPlatnosciViewModel(MetodyPlatnosciService service, MetodaPlatnosciDto dto)
        {
            DisplayName = "Edycja metody płatności";
            _service = service;
            _isEditMode = true;
            _id = dto.MetodaPlatnosciId;

            _item = new UpdateMetodaPlatnosciDto
            {
                Nazwa = dto.Nazwa,
                CzyAktywna = dto.CzyAktywna
            };
        }

        public string Nazwa
        {
            get => _item.Nazwa;
            set
            {
                _item.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public bool CzyAktywna
        {
            get => _item.CzyAktywna;
            set
            {
                _item.CzyAktywna = value;
                OnPropertyChanged(nameof(CzyAktywna));
            }
        }

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa metody płatności jest wymagana.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private CreateMetodaPlatnosciDto BuildCreateDto()
        {
            return new CreateMetodaPlatnosciDto
            {
                Nazwa = Nazwa,
                CzyAktywna = CzyAktywna
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
                    ? await _service.UpdateAsync(_id, _item)
                    : await _service.CreateAsync(BuildCreateDto());

                if (!success)
                {
                    MessageBox.Show("Nie udało się zapisać metody płatności.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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