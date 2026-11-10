using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Data.Entity;
using System.Windows.Input;

namespace ActiveClubFirma.ViewModels
{
    public class NowaAktualnoscViewModel : WorkspaceViewModel
    {
        #region DB
        private readonly ActiveClubEntities1 activeClubEntities1;
        #endregion

        #region Item
        private readonly Aktualnosci aktualnosc;
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
        public NowaAktualnoscViewModel()
        {
            base.DisplayName = "Aktualnosc";
            activeClubEntities1 = new ActiveClubEntities1();
            aktualnosc = new Aktualnosci();
        }
        #endregion

        #region Properties
        public string Tytul
        {
            get => aktualnosc.Tytul ?? string.Empty;
            set
            {
                aktualnosc.Tytul = value;
                OnPropertyChanged(nameof(Tytul));
            }
        }

        public string Tresc
        {
            get => aktualnosc.Tresc ?? string.Empty;
            set
            {
                aktualnosc.Tresc = value;
                OnPropertyChanged(nameof(Tresc));
            }
        }

        public DateTime DataDodania
        {
            get => aktualnosc.DataDodania;
            set
            {
                aktualnosc.DataDodania = value;
                OnPropertyChanged(nameof(DataDodania));
            }
        }
        #endregion

        #region Helpers
        public void Save()
        {
            // minimalna walidacja
            if (string.IsNullOrWhiteSpace(aktualnosc.Tytul))
                return;

            // ustaw datę jeśli brak
            if (aktualnosc.DataDodania == default(DateTime))
                aktualnosc.DataDodania = DateTime.Now;

            var entry = activeClubEntities1.Entry(aktualnosc);
            if (entry.State == EntityState.Detached)
                activeClubEntities1.Aktualnosci.Add(aktualnosc);

            activeClubEntities1.SaveChanges();
        }

        public void SaveAndClose()
        {
            Save();
            base.OnRequestClose();
        }
        #endregion
    }
}