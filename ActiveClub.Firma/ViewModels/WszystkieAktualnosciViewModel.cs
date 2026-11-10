using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Aktualnosci;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieAktualnosciViewModel : WszystkieViewModel<AktualnoscDto>
    {
        private readonly AktualnosciService _aktualnosciService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;
        private readonly KlubyService _klubyService;
        private readonly UzytkownicyService _uzytkownicyService;
        private AktualnoscDto _wybranaAktualnosc;
        public AktualnoscDto WybranaAktualnosc
        {
            get => _wybranaAktualnosc;
            set
            {
                _wybranaAktualnosc = value;
                SelectedItem = value;
                OnPropertyChanged(nameof(WybranaAktualnosc));
            }
        }

        public WszystkieAktualnosciViewModel(
     AktualnosciService aktualnosciService,
     KlubyService klubyService,
     UzytkownicyService uzytkownicyService,
     Action<WorkspaceViewModel> openWorkspace)
     : base("Aktualności")
        {
            _aktualnosciService = aktualnosciService;
            _klubyService = klubyService;
            _uzytkownicyService = uzytkownicyService;
            _openWorkspace = openWorkspace;
        }

        public override async void Load()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var items = await _aktualnosciService.GetAllAsync();
            List = new ObservableCollection<AktualnoscDto>(items);
        }

        protected override void Add()
        {
            var vm = new NowaAktualnoscViewModel(
                _aktualnosciService,
                _klubyService,
                _uzytkownicyService);

            vm.SavedSuccessfully += async () => await LoadAsync();
            _openWorkspace?.Invoke(vm);
        }

        protected override void Edit()
        {
            if (WybranaAktualnosc == null)
            {
                ShowMessageBox("Nie wybrano aktualności.");
                return;
            }

            var vm = new NowaAktualnoscViewModel(
                _aktualnosciService,
                _klubyService,
                _uzytkownicyService,
                WybranaAktualnosc);

            vm.SavedSuccessfully += async () => await LoadAsync();
            _openWorkspace?.Invoke(vm);
        }

        protected override async void Delete()
        {
            if (WybranaAktualnosc == null)
            {
                ShowMessageBox("Nie wybrano aktualności.");
                return;
            }

            var success = await _aktualnosciService.DeleteAsync(WybranaAktualnosc.AktualnoscId);

            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć aktualności.");
                return;
            }

            await LoadAsync();
        }
    }
}