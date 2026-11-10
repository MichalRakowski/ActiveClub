using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Sale;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieSaleViewModel : WszystkieViewModel<SalaDto>
    {
        private readonly SaleService _saleService;
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieSaleViewModel(
            SaleService saleService,
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Sale")
        {
            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _saleService.GetAllAsync();
                List = new ObservableCollection<SalaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania sal: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano sali.");
                return;
            }

            try
            {
                var success = await _saleService.DeleteAsync(SelectedItem.SalaId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć sali.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania sali: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano sali.");
                return;
            }

            var vm = new NowaSalaViewModel(_saleService, _klubyService, SelectedItem);
            vm.SalaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowaSalaViewModel(_saleService, _klubyService);
            vm.SalaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}