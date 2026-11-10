using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.PozycjeZamowienia;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkiePozycjeZamowieniaViewModel : WszystkieViewModel<PozycjaZamowieniaDto>
    {
        private readonly PozycjeZamowieniaService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkiePozycjeZamowieniaViewModel(
            PozycjeZamowieniaService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Pozycje zamówienia")
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
                List = new ObservableCollection<PozycjaZamowieniaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania pozycji zamówienia: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano pozycji zamówienia.");
                return;
            }

            try
            {
                var success = await _service.DeleteAsync(SelectedItem.PozycjaId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć pozycji zamówienia.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania pozycji zamówienia: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano pozycji zamówienia.");
                return;
            }

            var vm = new NowaPozycjaZamowieniaViewModel(_service, SelectedItem);
            vm.PozycjaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowaPozycjaZamowieniaViewModel(_service);
            vm.PozycjaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}