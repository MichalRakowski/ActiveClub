using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Stanowiska;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieStanowiskaViewModel : WszystkieViewModel<StanowiskoDto>
    {
        private readonly StanowiskaService _stanowiskaService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieStanowiskaViewModel(
            StanowiskaService stanowiskaService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Stanowiska")
        {
            _stanowiskaService = stanowiskaService ?? throw new ArgumentNullException(nameof(stanowiskaService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _stanowiskaService.GetAllAsync();
                List = new ObservableCollection<StanowiskoDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania stanowisk: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano stanowiska.");
                return;
            }

            try
            {
                var success = await _stanowiskaService.DeleteAsync(SelectedItem.StanowiskoId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć stanowiska.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania stanowiska: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano stanowiska.");
                return;
            }

            var vm = new NoweStanowiskoViewModel(_stanowiskaService, SelectedItem);
            vm.StanowiskoSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NoweStanowiskoViewModel(_stanowiskaService);
            vm.StanowiskoSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}