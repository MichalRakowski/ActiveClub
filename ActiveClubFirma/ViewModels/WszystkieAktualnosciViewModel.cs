using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ActiveClubFirma.Helper;

namespace ActiveClubFirma.ViewModels
{
    public class WszystkieAktualnosciViewModel : WszystkieViewModel<Aktualnosci>
    {



        #region Properties
        private Aktualnosci _wybranaAktualnosc;
        public Aktualnosci WybranaAktualnosc
        {
            get => _wybranaAktualnosc;
            set
            {
                _wybranaAktualnosc = value;
                SelectedItem = value;
                OnPropertyChanged(nameof(WybranaAktualnosc));
            }
        }

        #endregion
        #region Constructor
        public WszystkieAktualnosciViewModel()
            : base("Aktualnosci")
        {


        }
        #endregion
        #region Helpers

        #endregion
        public override void Load()

        {
            List = new ObservableCollection<Aktualnosci>
             (
                 activeClubEntities1.Aktualnosci.ToList()
             );
        }
        protected override void Delete()
        {
            if (WybranaAktualnosc == null)
            {
                ShowMessageBox("Nie wybrano aktualności.");
                return;
            }

            activeClubEntities1.Aktualnosci.Remove(WybranaAktualnosc);
            activeClubEntities1.SaveChanges();
            Load();
        }
        protected override void Edit()
        {
            if (WybranaAktualnosc == null)
            {
                ShowMessageBox("Nie wybrano aktualności.");
                return;
            }

            activeClubEntities1.SaveChanges(); // zapis zmian z textboxów
            Load();
        }
        protected override void Add()
        {
            var nowa = new Aktualnosci
            {
                Tytul = "",
                Tresc = "",
                DataDodania = DateTime.Now
            };

            activeClubEntities1.Aktualnosci.Add(nowa);
            activeClubEntities1.SaveChanges();

            Load();

            // ustaw zaznaczenie na dodaną (po ID)
            WybranaAktualnosc = List.FirstOrDefault(a => a.AktualnoscId == nowa.AktualnoscId);
        }
    }
}

