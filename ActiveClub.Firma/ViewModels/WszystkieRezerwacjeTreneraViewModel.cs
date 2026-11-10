using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRezerwacjeTreneraViewModel : WszyscyViewModel<RezerwacjaTreneraDto>
    {
        private readonly RezerwacjeTreneraService _service;
        private readonly TrenerzyService _trenerzyService;
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieRezerwacjeTreneraViewModel(
            RezerwacjeTreneraService service,
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Rezerwacje trenera")
        {
            _service = service;
            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;
            _klubyService = klubyService;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            var items = await _service.GetAllAsync();
            List = new ObservableCollection<RezerwacjaTreneraDto>(items);
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rezerwacji.");
                return;
            }

            var success = await _service.DeleteAsync(SelectedItem.RezerwacjaId);
            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć rezerwacji.");
                return;
            }

            Load();
        }

        protected override void Add()
        {
            var vm = new NowaRezerwacjaTreneraViewModel(
                _service,
                _trenerzyService,
                _uzytkownicyService,
                _klubyService);

            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rezerwacji.");
                return;
            }

            var vm = new NowaRezerwacjaTreneraViewModel(
                _service,
                _trenerzyService,
                _uzytkownicyService,
                _klubyService,
                SelectedItem);

            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}