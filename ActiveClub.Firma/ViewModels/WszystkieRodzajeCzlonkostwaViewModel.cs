using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRodzajeCzlonkostwaViewModel : WszyscyViewModel<RodzajCzlonkostwaDto>
    {
        private readonly RodzajeCzlonkostwaService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieRodzajeCzlonkostwaViewModel(
            RodzajeCzlonkostwaService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Rodzaje członkostwa")
        {
            _service = service;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            var items = await _service.GetAllAsync();
            List = new ObservableCollection<RodzajCzlonkostwaDto>(items);
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju członkostwa.");
                return;
            }

            var success = await _service.DeleteAsync(SelectedItem.RodzajCzlonkostwaId);

            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć rodzaju członkostwa.");
                return;
            }

            Load();
        }

        protected override void Add()
        {
            var vm = new NowyRodzajCzlonkostwaViewModel(_service);
            vm.RodzajCzlonkostwaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju członkostwa.");
                return;
            }

            var vm = new NowyRodzajCzlonkostwaViewModel(_service, SelectedItem);
            vm.RodzajCzlonkostwaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}