using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.PracownicyKlubu;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszyscyPracownicyKlubuViewModel : WszystkieViewModel<PracownikKlubuDto>
    {
        private readonly PracownicyKlubuService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszyscyPracownicyKlubuViewModel(
            PracownicyKlubuService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Pracownicy klubu")
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
                List = new ObservableCollection<PracownikKlubuDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania pracowników klubu: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rekordu.");
                return;
            }

            try
            {
                var success = await _service.DeleteAsync(SelectedItem.PracownikKlubId);
                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć rekordu.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania rekordu: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rekordu.");
                return;
            }

            var vm = new NowyPracownikKlubuViewModel(_service, SelectedItem);
            vm.PracownikKlubuSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowyPracownikKlubuViewModel(_service);
            vm.PracownikKlubuSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}