using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Platnosci;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkiePlatnosciViewModel : WszystkieViewModel<PlatnoscDto>
    {
        private readonly PlatnosciService _platnosciService;
        private readonly ZamowieniaService? _zamowieniaService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        private int? _filterUzytkownikId;

        public WszystkiePlatnosciViewModel(
            PlatnosciService platnosciService,
            Action<WorkspaceViewModel> openWorkspace,
            ZamowieniaService? zamowieniaService = null)
            : base("Płatności")
        {
            _platnosciService = platnosciService ?? throw new ArgumentNullException(nameof(platnosciService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));
            _zamowieniaService = zamowieniaService;

            Load();
        }

        public void FilterByUzytkownikId(int uzytkownikId)
        {
            _filterUzytkownikId = uzytkownikId;
            DisplayName = $"Płatności użytkownika #{uzytkownikId}";
            Load();
        }

        public void ClearFilter()
        {
            _filterUzytkownikId = null;
            DisplayName = "Płatności";
            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _platnosciService.GetAllAsync();

                if (_filterUzytkownikId.HasValue && _zamowieniaService != null)
                {
                    var zamowienia = await _zamowieniaService.GetAllAsync();

                    var zamowieniaIds = zamowienia
                        .Where(z => z.UzytkownikId == _filterUzytkownikId.Value)
                        .Select(z => z.ZamowienieId)
                        .ToHashSet();

                    items = items
                        .Where(p => zamowieniaIds.Contains(p.ZamowienieId))
                        .ToList();
                }

                List = new ObservableCollection<PlatnoscDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania płatności: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano płatności.");
                return;
            }

            try
            {
                var success = await _platnosciService.DeleteAsync(SelectedItem.PlatnoscId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć płatności.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania płatności: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano płatności.");
                return;
            }

            var vm = new NowaPlatnoscViewModel(_platnosciService, SelectedItem);

            vm.PlatnoscSaved += (_, _) => Load();

            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowaPlatnoscViewModel(_platnosciService);

            vm.PlatnoscSaved += (_, _) => Load();

            _openWorkspace(vm);
        }
    }
}