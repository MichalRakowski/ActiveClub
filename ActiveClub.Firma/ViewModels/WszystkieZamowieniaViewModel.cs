using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Zamowienia;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieZamowieniaViewModel : WszystkieViewModel<ZamowienieDto>
    {
        private readonly ZamowieniaService _zamowieniaService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        private int? _filterUzytkownikId;

        public WszystkieZamowieniaViewModel(
            ZamowieniaService zamowieniaService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Zamówienia")
        {
            _zamowieniaService = zamowieniaService ?? throw new ArgumentNullException(nameof(zamowieniaService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public void FilterByUzytkownikId(int uzytkownikId)
        {
            _filterUzytkownikId = uzytkownikId;
            DisplayName = $"Zamówienia użytkownika #{uzytkownikId}";
            Load();
        }

        public void ClearFilter()
        {
            _filterUzytkownikId = null;
            DisplayName = "Zamówienia";
            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _zamowieniaService.GetAllAsync();

                if (_filterUzytkownikId.HasValue)
                {
                    items = items
                        .Where(x => x.UzytkownikId == _filterUzytkownikId.Value)
                        .ToList();
                }

                List = new ObservableCollection<ZamowienieDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania zamówień: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano zamówienia.");
                return;
            }

            try
            {
                var success = await _zamowieniaService.DeleteAsync(SelectedItem.ZamowienieId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć zamówienia.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania zamówienia: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano zamówienia.");
                return;
            }

            var vm = new NoweZamowienieViewModel(_zamowieniaService, SelectedItem);

            vm.ZamowienieSaved += (_, _) => Load();

            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NoweZamowienieViewModel(_zamowieniaService);

            vm.ZamowienieSaved += (_, _) => Load();

            _openWorkspace(vm);
        }
    }
}