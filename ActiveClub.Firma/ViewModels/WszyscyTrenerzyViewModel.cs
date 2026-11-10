using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Trenerzy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszyscyTrenerzyViewModel : WszyscyViewModel<TrenerDto>
    {
        private readonly TrenerzyService _trenerzyService;
        private readonly UzytkownicyService _uzytkownicyService;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        public WszyscyTrenerzyViewModel(
            TrenerzyService trenerzyService,
            UzytkownicyService uzytkownicyService,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Trenerzy")
        {
            _trenerzyService = trenerzyService;
            _uzytkownicyService = uzytkownicyService;
            _openWorkspace = openWorkspace;

            Load();
        }

        public override async void Load()
        {
            var items = await _trenerzyService.GetAllAsync() ?? new List<TrenerDto>();
            List = new ObservableCollection<TrenerDto>(items);
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano trenera.");
                return;
            }

            var success = await _trenerzyService.DeleteAsync(SelectedItem.TrenerId);

            if (!success)
            {
                ShowMessageBox("Nie udało się usunąć trenera.");
                return;
            }

            SelectedItem = null;
            Load();
        }

        protected override void Add()
        {
            var vm = new NowyTrenerViewModel(_trenerzyService, _uzytkownicyService);
            vm.TrainerSaved += (_, _) => Load();

            _openWorkspace(vm);
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano trenera.");
                return;
            }

            var vm = new NowyTrenerViewModel(_trenerzyService, _uzytkownicyService, SelectedItem);
            vm.TrainerSaved += (_, _) => Load();

            _openWorkspace(vm);
        }
    }
}