using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System.Data.Entity;
using System.Windows.Input;

namespace ActiveClubFirma.ViewModels
{
    public class NowyRodzajCzlonkostwaViewModel : WorkspaceViewModel
    {
        #region DB
        private readonly ActiveClubEntities1 activeClubEntities1;
        #endregion

        #region Item
        private readonly RodzajeCzlonkostwa rodzajCzlonkostwa;
        #endregion

        #region Command
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(SaveAndClose);
                return _SaveCommand;
            }
        }
        #endregion

        #region Konstruktor
        public NowyRodzajCzlonkostwaViewModel()
        {
            DisplayName = "Rodzaj członkostwa";
            activeClubEntities1 = new ActiveClubEntities1();
            rodzajCzlonkostwa = new RodzajeCzlonkostwa();

            // domyślne wartości
            CzyAktywny = true;
            OkresDni = 30;
            Cena = 0m;
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get => rodzajCzlonkostwa.Nazwa ?? string.Empty;
            set
            {
                rodzajCzlonkostwa.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string Opis
        {
            get => rodzajCzlonkostwa.Opis ?? string.Empty;
            set
            {
                rodzajCzlonkostwa.Opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public decimal Cena
        {
            get => rodzajCzlonkostwa.Cena;
            set
            {
                rodzajCzlonkostwa.Cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public int OkresDni
        {
            get => rodzajCzlonkostwa.OkresDni;
            set
            {
                rodzajCzlonkostwa.OkresDni = value;
                OnPropertyChanged(nameof(OkresDni));
            }
        }

        public bool CzyAktywny
        {
            get => rodzajCzlonkostwa.CzyAktywny;
            set
            {
                rodzajCzlonkostwa.CzyAktywny = value;
                OnPropertyChanged(nameof(CzyAktywny));
            }
        }
        #endregion

        #region Helpers
        public void Save()
        {
            // minimalna walidacja
            if (string.IsNullOrWhiteSpace(rodzajCzlonkostwa.Nazwa))
                return;

            if (rodzajCzlonkostwa.OkresDni <= 0)
                return;

            if (rodzajCzlonkostwa.Cena < 0)
                return;

            var entry = activeClubEntities1.Entry(rodzajCzlonkostwa);
            if (entry.State == EntityState.Detached)
                activeClubEntities1.RodzajeCzlonkostwa.Add(rodzajCzlonkostwa);

            activeClubEntities1.SaveChanges();
        }

        public void SaveAndClose()
        {
            Save();
            OnRequestClose();
        }
        #endregion
    }
}