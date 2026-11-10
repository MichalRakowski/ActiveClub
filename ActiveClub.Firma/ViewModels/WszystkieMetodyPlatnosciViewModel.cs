using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.MetodyPlatnosci;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieMetodyPlatnosciViewModel : WszyscyViewModel<MetodaPlatnosciDto>
    {
        private readonly MetodyPlatnosciService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieMetodyPlatnosciViewModel(
            MetodyPlatnosciService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Metody płatności")
        {
            _service = service;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            var items = await _service.GetAllAsync();
            List = new ObservableCollection<MetodaPlatnosciDto>(items);
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano metody płatności.");
                return;
            }

            var success = await _service.DeleteAsync(SelectedItem.MetodaPlatnosciId);
            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć metody płatności.");
                return;
            }

            Load();
        }

        protected override void Add()
        {
            var vm = new NowaMetodaPlatnosciViewModel(_service);
            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano metody płatności.");
                return;
            }

            var vm = new NowaMetodaPlatnosciViewModel(_service, SelectedItem);
            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}