using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Opinie;
using System;
using System.Collections.ObjectModel;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieOpinieViewModel : WszystkieViewModel<OpiniaDto>
    {
        private readonly OpinieService _opinieService;
        private OpiniaDto? _wybranaOpinia;

        public OpiniaDto? WybranaOpinia
        {
            get => _wybranaOpinia;
            set
            {
                _wybranaOpinia = value;
                SelectedItem = value;
                OnPropertyChanged(nameof(WybranaOpinia));
            }
        }

        public WszystkieOpinieViewModel(OpinieService opinieService) : base("Opinie")
        {
            _opinieService = opinieService ?? throw new ArgumentNullException(nameof(opinieService));
        }

        public override async void Load()
        {
            try
            {
                var items = await _opinieService.GetAllAsync();
                List = new ObservableCollection<OpiniaDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania opinii: {ex.Message}");
            }
        }

        protected override async void Delete()
        {
            if (WybranaOpinia == null)
            {
                ShowMessageBox("Nie wybrano opinii.");
                return;
            }

            try
            {
                var success = await _opinieService.DeleteAsync(WybranaOpinia.OpiniaId);
                if (!success)
                {
                    ShowMessageBox("Nie udało się usunąć opinii.");
                    return;
                }

                WybranaOpinia = null;
                Load();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas usuwania opinii: {ex.Message}");
            }
        }

        protected override void Edit()
        {
            if (WybranaOpinia == null)
            {
                ShowMessageBox("Nie wybrano opinii.");
                return;
            }

            ShowMessageBox("Edycję opinii dodamy po przygotowaniu formularza edycji.");
        }

        protected override void Add()
        {
            ShowMessageBox("Dodawanie opinii jest możliwe tylko przez portal WWW.");
        }
    }
}