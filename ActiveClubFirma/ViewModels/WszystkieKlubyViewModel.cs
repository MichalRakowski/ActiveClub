using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ActiveClubFirma.ViewModels
{
    public class WszystkieKlubyViewModel : WszystkieViewModel<Kluby>
    {
        #region Properties

        private Kluby _wybranyKlub;
        public Kluby WybranyKlub
        {
            get => _wybranyKlub;
            set
            {
                _wybranyKlub = value;
                SelectedItem = value;
                OnPropertyChanged(nameof(WybranyKlub));
            }
        }

        #endregion

        #region Constructor

        public WszystkieKlubyViewModel()
            : base("Kluby")
        {
        }

        #endregion

        #region Load

        public override void Load()
        {
            List = new ObservableCollection<Kluby>(
                activeClubEntities1.Kluby.ToList()
            );
        }

        #endregion

        #region CRUD

        protected override void Delete()
        {
            if (WybranyKlub == null)
            {
                ShowMessageBox("Nie wybrano klubu.");
                return;
            }

            activeClubEntities1.Kluby.Remove(WybranyKlub);
            activeClubEntities1.SaveChanges();
            Load();
        }

        protected override void Edit()
        {
            if (WybranyKlub == null)
            {
                ShowMessageBox("Nie wybrano klubu.");
                return;
            }

            activeClubEntities1.SaveChanges();
            Load();
        }

        protected override void Add()
        {
            var nowy = new Kluby
            {
                Nazwa = "",
                Miasto = "",
                Ulica = "",
                KodPocztowy = "",
                Telefon = "",
                EmailKontakt = "",
                Opis = "",
                CzyAktywny = true,
                DataUtworzenia = DateTime.Now
            };

            activeClubEntities1.Kluby.Add(nowy);
            activeClubEntities1.SaveChanges();

            Load();

            // Ustaw zaznaczenie na nowo dodany
            WybranyKlub = List.FirstOrDefault(k => k.KlubId == nowy.KlubId);
        }

        #endregion
    }
}