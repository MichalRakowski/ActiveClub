using ActiveClub.Firma.Helper;
using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieCzlonkostwaUzytkownikaViewModel
        : WszystkieViewModel<CzlonkostwoUzytkownikaDto>
    {
        private readonly CzlonkostwaUzytkownikaService _service;
        private readonly Action<WorkspaceViewModel> _openWorkspace;

        private BaseCommand? _dodajWejscieCommand;
        public ICommand DodajWejscieCommand =>
            _dodajWejscieCommand ??= new BaseCommand(DodajWejscie);

        private BaseCommand? _cofnijWejscieCommand;
        public ICommand CofnijWejscieCommand =>
            _cofnijWejscieCommand ??= new BaseCommand(CofnijWejscie);
        private BaseCommand? _anulujPrzeterminowaneCommand;

        public ICommand AnulujPrzeterminowaneCommand =>
            _anulujPrzeterminowaneCommand ??= new BaseCommand(AnulujPrzeterminowane);
        public WszystkieCzlonkostwaUzytkownikaViewModel(
            CzlonkostwaUzytkownikaService service,
            Action<WorkspaceViewModel> openWorkspace)
            : base("Członkostwa użytkowników")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _openWorkspace = openWorkspace ?? throw new ArgumentNullException(nameof(openWorkspace));

            Load();
        }
        private async void AnulujPrzeterminowane()
        {
            try
            {
                var count = await _service.AnulujPrzeterminowaneAsync();

                ShowMessageBox($"Anulowano przeterminowane karnety: {count}");

                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas anulowania karnetów: {ex.Message}");
            }
        }
        public override async void Load()
        {
            try
            {
                var items = await _service.GetAllAsync();
                List = new ObservableCollection<CzlonkostwoUzytkownikaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania członkostw: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano członkostwa.");
                return;
            }

            try
            {
                var success = await _service.DeleteAsync(SelectedItem.CzlonkostwoId);

                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć członkostwa.");
                    return;
                }

                SelectedItem = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania członkostwa: {ex.Message}");
            }
        }

        protected override async void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano członkostwa.");
                return;
            }

            try
            {
                var dto = await _service.GetByIdAsync(SelectedItem.CzlonkostwoId);

                if (dto == null)
                {
                    ShowMessageBox("Nie znaleziono członkostwa.");
                    return;
                }

                var vm = new NoweCzlonkostwoUzytkownikaViewModel(_service, dto);
                vm.Saved += (_, _) => Load();

                _openWorkspace(vm);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas otwierania edycji: {ex.Message}");
            }
        }

        protected override void Add()
        {
            var vm = new NoweCzlonkostwoUzytkownikaViewModel(_service);
            vm.Saved += (_, _) => Load();

            _openWorkspace(vm);
        }

        private async void DodajWejscie()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Wybierz członkostwo.");
                return;
            }

            try
            {
                var success = await _service.DodajWejscieAsync(
                    SelectedItem.CzlonkostwoId);

                if (!success)
                {
                    ShowMessageBox(
                        "Nie można zarejestrować wejścia. Limit mógł zostać wykorzystany.");
                    return;
                }

                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas rejestrowania wejścia: {ex.Message}");
            }
        }

        private async void CofnijWejscie()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Wybierz członkostwo.");
                return;
            }

            try
            {
                var success = await _service.CofnijWejscieAsync(
                    SelectedItem.CzlonkostwoId);

                if (!success)
                {
                    ShowMessageBox("Nie można cofnąć wejścia.");
                    return;
                }

                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas cofania wejścia: {ex.Message}");
            }
        }
    }
}