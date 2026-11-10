using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class NowyRodzajCzlonkostwaViewModel : WorkspaceViewModel
    {
        private readonly RodzajeCzlonkostwaService _service;
        private readonly UpdateRodzajCzlonkostwaDto _dto;
        private readonly bool _isEditMode;

        public event EventHandler? RodzajCzlonkostwaSaved;

        private BaseCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new BaseCommand(async () => await SaveAndClose());

        private BaseCommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new BaseCommand(() => OnRequestClose());

        public NowyRodzajCzlonkostwaViewModel(RodzajeCzlonkostwaService service)
        {
            _service = service;
            _isEditMode = false;

            DisplayName = "Rodzaj członkostwa";

            _dto = new UpdateRodzajCzlonkostwaDto
            {
                Kod = "",
                Nazwa = "",
                Opis = "",
                Cena = 0,
                TypRozliczenia = "Okresowe",
                OkresDni = 30,
                LiczbaWejsc = null,
                CzyAktywny = true,
                CzyWidoczneOnline = true
            };
        }

        public NowyRodzajCzlonkostwaViewModel(RodzajeCzlonkostwaService service, RodzajCzlonkostwaDto dto)
        {
            _service = service;
            _isEditMode = true;

            DisplayName = "Edycja rodzaju członkostwa";

            _dto = new UpdateRodzajCzlonkostwaDto
            {
                RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId,
                Kod = dto.Kod,
                Nazwa = dto.Nazwa,
                Opis = dto.Opis,
                Cena = dto.Cena,
                TypRozliczenia = dto.TypRozliczenia,
                OkresDni = dto.OkresDni,
                LiczbaWejsc = dto.LiczbaWejsc,
                CzyAktywny = dto.CzyAktywny,
                CzyWidoczneOnline = dto.CzyWidoczneOnline
            };
        }

        public int RodzajCzlonkostwaId
        {
            get => _dto.RodzajCzlonkostwaId;
            set
            {
                _dto.RodzajCzlonkostwaId = value;
                OnPropertyChanged(nameof(RodzajCzlonkostwaId));
            }
        }

        public string Kod
        {
            get => _dto.Kod;
            set
            {
                _dto.Kod = value;
                OnPropertyChanged(nameof(Kod));
            }
        }

        public string Nazwa
        {
            get => _dto.Nazwa;
            set
            {
                _dto.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string? Opis
        {
            get => _dto.Opis;
            set
            {
                _dto.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public decimal? Cena
        {
            get => _dto.Cena;
            set
            {
                _dto.Cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public string TypRozliczenia
        {
            get => _dto.TypRozliczenia;
            set
            {
                _dto.TypRozliczenia = value;
                OnPropertyChanged(nameof(TypRozliczenia));
            }
        }

        public int? OkresDni
        {
            get => _dto.OkresDni;
            set
            {
                _dto.OkresDni = value;
                OnPropertyChanged(nameof(OkresDni));
            }
        }

        public int? LiczbaWejsc
        {
            get => _dto.LiczbaWejsc;
            set
            {
                _dto.LiczbaWejsc = value;
                OnPropertyChanged(nameof(LiczbaWejsc));
            }
        }

        public bool CzyAktywny
        {
            get => _dto.CzyAktywny;
            set
            {
                _dto.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }

        public bool CzyWidoczneOnline
        {
            get => _dto.CzyWidoczneOnline;
            set
            {
                _dto.CzyWidoczneOnline = value;
                OnPropertyChanged(nameof(CzyWidoczneOnline));
            }
        }

        public string SaveButtonText => _isEditMode ? "Zapisz zmiany" : "Dodaj";

        private async Task SaveAndClose()
        {
            if (string.IsNullOrWhiteSpace(Kod))
            {
                ShowMessageBox("Pole Kod jest wymagane.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                ShowMessageBox("Pole Nazwa jest wymagane.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TypRozliczenia))
            {
                ShowMessageBox("Pole Typ rozliczenia jest wymagane.");
                return;
            }

            (bool Success, string ErrorMessage) result;

            if (_isEditMode)
            {
                result = await _service.UpdateAsync(_dto.RodzajCzlonkostwaId, _dto);
            }
            else
            {
                var createDto = new CreateRodzajCzlonkostwaDto
                {
                    Kod = _dto.Kod,
                    Nazwa = _dto.Nazwa,
                    Opis = _dto.Opis,
                    Cena = _dto.Cena,
                    TypRozliczenia = _dto.TypRozliczenia,
                    OkresDni = _dto.OkresDni,
                    LiczbaWejsc = _dto.LiczbaWejsc,
                    CzyAktywny = _dto.CzyAktywny,
                    CzyWidoczneOnline = _dto.CzyWidoczneOnline
                };

                result = await _service.CreateAsync(createDto);
            }

            if (!result.Success)
            {
                ShowMessageBox("Nie udało się zapisać.\n\n" + result.ErrorMessage);
                return;
            }

            RodzajCzlonkostwaSaved?.Invoke(this, EventArgs.Empty);
            OnRequestClose();
        }
    }
}