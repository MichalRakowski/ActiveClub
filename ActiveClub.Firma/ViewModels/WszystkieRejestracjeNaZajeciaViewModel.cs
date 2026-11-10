using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRejestracjeNaZajeciaViewModel : WszystkieViewModel<RejestracjaNaZajeciaDto>
    {
        private readonly RejestracjeNaZajeciaService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieRejestracjeNaZajeciaViewModel(
            RejestracjeNaZajeciaService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Rezerwacje na zajęcia")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _service.GetAllAsync();
                List = new ObservableCollection<RejestracjaNaZajeciaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania rezerwacji: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rezerwacji.");
                return;
            }

            try
            {
                var success = await _service.DeleteAsync(SelectedItem.RejestracjaId);
                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć rezerwacji.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania rezerwacji: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rezerwacji.");
                return;
            }

            var vm = new NowaRejestracjaNaZajeciaViewModel(_service, SelectedItem);
            vm.RejestracjaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowaRejestracjaNaZajeciaViewModel(_service);
            vm.RejestracjaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}