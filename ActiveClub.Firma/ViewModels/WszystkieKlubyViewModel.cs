using ActiveClub.Shared.DTOs.Kluby;
using ActiveClub.Firma.Services;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieKlubyViewModel : WszystkieViewModel<KlubDto>
    {
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieKlubyViewModel(
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Kluby")
        {
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _klubyService.GetAllAsync();
                List = new ObservableCollection<KlubDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania klubów: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano klubu.");
                return;
            }

            try
            {
                var success = await _klubyService.DeleteAsync(SelectedItem.KlubId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć klubu.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania klubu: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano klubu.");
                return;
            }

            var vm = new NowyKlubViewModel(_klubyService, SelectedItem);
            vm.KlubSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowyKlubViewModel(_klubyService);
            vm.KlubSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}