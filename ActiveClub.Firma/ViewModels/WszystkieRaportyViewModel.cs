using ActiveClub.Firma.Services;
using ActiveClub.Shared.DTOs.Raporty;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace ActiveClub.Firma.ViewModels
{
    public class WszystkieRaportyViewModel : WszystkieViewModel<RaportWygenerowanyDto>
    {
        private readonly RaportyService _raportyService;

        public WszystkieRaportyViewModel(RaportyService raportyService)
            : base("Wygenerowane raporty")
        {
            _raportyService = raportyService ?? throw new ArgumentNullException(nameof(raportyService));
            Load();
        }

        public override async void Load()
        {
            try
            {
                var items = await _raportyService.GetGeneratedAsync();
                List = new ObservableCollection<RaportWygenerowanyDto>(items);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd podczas ładowania raportów: {ex.Message}");
            }
        }

        protected override void Delete()
        {
            ShowMessageBox("Usuwanie raportów możesz dodać później.");
        }

        protected override void Edit()
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.PlikUrl))
            {
                ShowMessageBox("Nie wybrano raportu lub raport nie ma pliku.");
                return;
            }

            if (!File.Exists(SelectedItem.PlikUrl))
            {
                ShowMessageBox("Plik raportu nie istnieje.");
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = SelectedItem.PlikUrl,
                UseShellExecute = true
            });
        }

        protected override void Add()
        {
            ShowMessageBox("Generowanie raportu odbywa się przez formularz „Generuj raport”.");
        }
    }
}