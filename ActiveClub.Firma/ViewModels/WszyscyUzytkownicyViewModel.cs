using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszyscyUzytkownicyViewModel : WszyscyViewModel<UzytkownikDto>
    {
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly KlubyService _klubyService;
        private readonly TrenerzyService _trenerzyService;
        private readonly StanowiskaService _stanowiskaService;
        private readonly CzlonkostwaUzytkownikaService _czlonkostwaService;
        private readonly HarmonogramZajecService _harmonogramService;
        private readonly ZamowieniaService _zamowieniaService;
        private readonly PlatnosciService _platnosciService;
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly SaleService _saleService;
        private readonly RodzajeCzlonkostwaService _rodzajeCzlonkostwaService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszyscyUzytkownicyViewModel(
     UzytkownicyService uzytkownicyService,
     KlubyService klubyService,
     TrenerzyService trenerzyService,
     StanowiskaService stanowiskaService,
     CzlonkostwaUzytkownikaService czlonkostwaService,
     RodzajeCzlonkostwaService rodzajeCzlonkostwaService,
     HarmonogramZajecService harmonogramService,
     ZamowieniaService zamowieniaService,
     PlatnosciService platnosciService,
     RodzajeZajecService rodzajeZajecService,
     SaleService saleService,
     Action<WorkspaceViewModel> openWorkspace)
             : base("Użytkownicy")
        {
            _uzytkownicyService = uzytkownicyService;
            _klubyService = klubyService;
            _trenerzyService = trenerzyService;
            _rodzajeCzlonkostwaService = rodzajeCzlonkostwaService;
            _stanowiskaService = stanowiskaService;
            _czlonkostwaService = czlonkostwaService;
            _harmonogramService = harmonogramService;
            _zamowieniaService = zamowieniaService;
            _platnosciService = platnosciService;
            _rodzajeZajecService = rodzajeZajecService;
            _saleService = saleService;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _uzytkownicyService.GetAllAsync()
                            ?? new List<UzytkownikDto>();

                List = new ObservableCollection<UzytkownikDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania użytkowników: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano użytkownika.");
                return;
            }

            try
            {
                var success = await _uzytkownicyService.DeleteAsync(SelectedItem.Id);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć użytkownika.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania użytkownika: {ex.Message}");
            }
        }

        protected override void Add()
        {
            var vm = new NowyUzytkownikViewModel(
    _uzytkownicyService,
    _klubyService,
    _trenerzyService,
    _stanowiskaService,
    _czlonkostwaService,
    _rodzajeCzlonkostwaService,
    _harmonogramService,
    _zamowieniaService,
    _platnosciService,
    _rodzajeZajecService,
    _saleService,
    _openWorkspace);

            vm.UserSaved += (_, _) => Load();

            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano użytkownika.");
                return;
            }

            var vm = new NowyUzytkownikViewModel(
     _uzytkownicyService,
     _klubyService,
     _trenerzyService,
     _stanowiskaService,
     _czlonkostwaService,
     _rodzajeCzlonkostwaService,
     _harmonogramService,
     _zamowieniaService,
     _platnosciService,
     _rodzajeZajecService,
     _saleService,
     SelectedItem,
     _openWorkspace);

            vm.UserSaved += (_, _) => Load();

            _openWorkspace(vm);
        }
    }
}