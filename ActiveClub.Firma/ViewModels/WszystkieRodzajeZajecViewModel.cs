using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRodzajeZajecViewModel : WszystkieViewModel<RodzajZajecDto>
    {
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieRodzajeZajecViewModel(
            RodzajeZajecService rodzajeZajecService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Rodzaje zajęć")
        {
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _rodzajeZajecService.GetAllAsync();
                List = new ObservableCollection<RodzajZajecDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania rodzajów zajęć: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju zajęć.");
                return;
            }

            try
            {
                var success = await _rodzajeZajecService.DeleteAsync(SelectedItem.RodzajZajecId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć rodzaju zajęć.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania rodzaju zajęć: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju zajęć.");
                return;
            }

            var vm = new NowyRodzajZajecViewModel(_rodzajeZajecService, SelectedItem);
            vm.RodzajZajecSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowyRodzajZajecViewModel(_rodzajeZajecService);
            vm.RodzajZajecSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}