using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.HarmonogramZajec;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieHarmonogramyZajecViewModel : WszystkieViewModel<HarmonogramZajecDto>
    {
        private readonly HarmonogramZajecService _harmonogramService;
        private readonly KlubyService _klubyService;
        private readonly RodzajeZajecService _rodzajeZajecService;
        private readonly SaleService _saleService;
        private readonly TrenerzyService _trenerzyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;
        private readonly UzytkownicyService _uzytkownicyService;
        public WszystkieHarmonogramyZajecViewModel(
    HarmonogramZajecService harmonogramService,
    KlubyService klubyService,
    RodzajeZajecService rodzajeZajecService,
    SaleService saleService,
    TrenerzyService trenerzyService,
    UzytkownicyService uzytkownicyService,
    Action<WorkspaceViewModel> openWorkspace)
    : base("Harmonogram zajęć")
        {
            _harmonogramService = harmonogramService ?? throw new ArgumentNullException(nameof(harmonogramService));
            _klubyService = klubyService ?? throw new ArgumentNullException(nameof(klubyService));
            _rodzajeZajecService = rodzajeZajecService ?? throw new ArgumentNullException(nameof(rodzajeZajecService));
            _saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
            _trenerzyService = trenerzyService ?? throw new ArgumentNullException(nameof(trenerzyService));
            _uzytkownicyService = uzytkownicyService ?? throw new ArgumentNullException(nameof(uzytkownicyService));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _harmonogramService.GetAllAsync();
                List = new ObservableCollection<HarmonogramZajecDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania harmonogramu zajęć: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano harmonogramu zajęć.");
                return;
            }

            try
            {
                var success = await _harmonogramService.DeleteAsync(SelectedItem.HarmonogramId);
                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć harmonogramu zajęć.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania harmonogramu zajęć: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano harmonogramu zajęć.");
                return;
            }

            var vm = new NowyHarmonogramZajecViewModel(
     _harmonogramService,
     _klubyService,
     _rodzajeZajecService,
     _saleService,
     _trenerzyService,
     _uzytkownicyService,
     SelectedItem);

            vm.HarmonogramSaved += (_, _) => Load();
            _openWorkspace(vm);
        }

        protected override void Add()
        {
            var vm = new NowyHarmonogramZajecViewModel(
    _harmonogramService,
    _klubyService,
    _rodzajeZajecService,
    _saleService,
    _trenerzyService,
    _uzytkownicyService);

            vm.HarmonogramSaved += (_, _) => Load();
            _openWorkspace(vm);
        }
    }
}