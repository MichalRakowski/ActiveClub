using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieOfertyKlubuCzlonkostwaViewModel : WszystkieViewModel<OfertaKlubuCzlonkostwoDto>
    {
        private readonly OfertyKlubuCzlonkostwaService _service;
        private readonly KlubyService _klubyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszystkieOfertyKlubuCzlonkostwaViewModel(
            OfertyKlubuCzlonkostwaService service,
            KlubyService klubyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Oferty członkostwa")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _service.GetAllAsync();
                List = new ObservableCollection<OfertaKlubuCzlonkostwoDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania ofert członkostwa: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano oferty członkostwa.");
                return;
            }

            try
            {
                var success = await _service.DeleteAsync(SelectedItem.OfertaKlubuCzlonkostwoId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć oferty członkostwa.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania oferty członkostwa: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano oferty członkostwa.");
                return;
            }

            var vm = new NowaOfertaKlubuCzlonkostwoViewModel(
                _service,
                _klubyService,
                SelectedItem);

            vm.OfertaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowaOfertaKlubuCzlonkostwoViewModel(
                _service,
                _klubyService);

            vm.OfertaSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}