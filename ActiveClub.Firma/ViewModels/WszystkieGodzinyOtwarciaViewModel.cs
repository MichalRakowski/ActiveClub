using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.GodzinyOtwarcia;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieGodzinyOtwarciaViewModel : WszyscyViewModel<GodzinyOtwarciaDto>
    {
        private readonly GodzinyOtwarciaService _service;
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieGodzinyOtwarciaViewModel(
            GodzinyOtwarciaService service,
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Godziny otwarcia")
        {
            _service = service;
            _klubyService = klubyService;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _service.GetAllAsync();
                List = new ObservableCollection<GodzinyOtwarciaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd ładowania godzin otwarcia: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            try
            {
                if (SelectedItem == null)
                {
                    ShowMessageBox("Nie wybrano godzin otwarcia.");
                    return;
                }

                var success = await _service.DeleteAsync(SelectedItem.GodzinyOtwarciaId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć godzin otwarcia.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd usuwania godzin otwarcia: {ex.Message}");
            }
        }

        protected override void Add()
        {
            var vm = new NoweGodzinyOtwarciaViewModel(_service, _klubyService);
            vm.Saved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            try
            {
                if (SelectedItem == null)
                {
                    ShowMessageBox("Nie wybrano godzin otwarcia.");
                    return;
                }

                var vm = new NoweGodzinyOtwarciaViewModel(
                    _service,
                    _klubyService,
                    SelectedItem);

                vm.Saved += (_, _) => Load();
                _openWorkspace(vm);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd otwierania edycji: {ex.Message}");
            }
        }
    }
}