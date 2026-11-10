using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.DostepnoscTrenera;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieDostepnosciTreneraViewModel : WszyscyViewModel<DostepnoscTreneraDto>
    {
        private readonly DostepnoscTreneraService _service;
        private readonly TrenerzyService _trenerzyService;
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieDostepnosciTreneraViewModel(
            DostepnoscTreneraService service,
            TrenerzyService trenerzyService,
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Dostępność trenerów")
        {
            _service = service;
            _trenerzyService = trenerzyService;
            _klubyService = klubyService;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            var items = await _service.GetAllAsync();
            List = new ObservableCollection<DostepnoscTreneraDto>(items);
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano dostępności.");
                return;
            }

            var success = await _service.DeleteAsync(SelectedItem.DostepnoscId);
            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć dostępności.");
                return;
            }

            Load();
        }

        protected override void Add()
        {
            var vm = new NowaDostepnoscTreneraViewModel(
                _service,
                _trenerzyService,
                _klubyService);

            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano dostępności.");
                return;
            }

            var vm = new NowaDostepnoscTreneraViewModel(
                _service,
                _trenerzyService,
                _klubyService,
                SelectedItem);

            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}