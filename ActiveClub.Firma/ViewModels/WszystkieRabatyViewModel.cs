using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Rabaty;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRabatyViewModel : WszystkieViewModel<RabatDto>
    {
        private readonly RabatyService _rabatyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieRabatyViewModel(
            RabatyService rabatyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Rabaty")
        {
            _rabatyService = rabatyService ?? throw new ArgumentNullException(nameof(rabatyService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _rabatyService.GetAllAsync();
                List = new ObservableCollection<RabatDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania rabatów: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rabatu.");
                return;
            }

            try
            {
                var success = await _rabatyService.DeleteAsync(SelectedItem.RabatId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć rabatu.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania rabatu: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rabatu.");
                return;
            }

            var vm = new NowyRabatViewModel(_rabatyService, SelectedItem);
            vm.RabatSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowyRabatViewModel(_rabatyService);
            vm.RabatSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}