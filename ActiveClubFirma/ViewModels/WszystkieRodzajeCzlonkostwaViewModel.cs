using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace ActiveClubFirma.ViewModels
{
    public class WszystkieRodzajeCzlonkostwaViewModel : WszyscyViewModel<RodzajeCzlonkostwa>
    {
        #region Constructor
        public WszystkieRodzajeCzlonkostwaViewModel()
            : base("Rodzaje członkostwa")
        {
        }
        #endregion

        #region Load
        public override void Load()
        {
            List = new ObservableCollection<RodzajeCzlonkostwa>(
                activeClubEntities1.RodzajeCzlonkostwa.ToList()
            );
        }
        #endregion

        #region Delete
        protected override void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju członkostwa.");
                return;
            }

            activeClubEntities1.RodzajeCzlonkostwa.Remove(SelectedItem);
            activeClubEntities1.SaveChanges();
            Load();
        }
        #endregion

        #region Add
        protected override void Add()
        {
            var nowy = new RodzajeCzlonkostwa
            {
                CzyAktywny = true,
                OkresDni = 30,
                Cena = 0m,
                Opis = ""
            };

            // ważne: tracking
            activeClubEntities1.RodzajeCzlonkostwa.Add(nowy);
            List.Add(nowy);
            SelectedItem = nowy;
        }
        #endregion

        #region Edit
        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano rodzaju członkostwa.");
                return;
            }

            try
            {
                // wymuś zatwierdzenie edycji w DataGrid
                Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);

                var entry = activeClubEntities1.Entry(SelectedItem);

                if (entry.State == EntityState.Detached)
                {
                    // jeśli nowy rekord -> Add, jeśli istnieje -> Attach
                    if (SelectedItem.RodzajCzlonkostwaId == 0)   // <- jeśli masz inną nazwę klucza, podmień
                        activeClubEntities1.RodzajeCzlonkostwa.Add(SelectedItem);
                    else
                        activeClubEntities1.RodzajeCzlonkostwa.Attach(SelectedItem);

                    entry = activeClubEntities1.Entry(SelectedItem);
                }

                if (SelectedItem.RodzajCzlonkostwaId == 0)       // <- jeśli masz inną nazwę klucza, podmień
                    entry.State = EntityState.Added;
                else
                    entry.State = EntityState.Modified;

                activeClubEntities1.SaveChanges();
                Load();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var msg = string.Join("\n",
                    ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors)
                      .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                );
                ShowMessageBox("Nie można zapisać:\n" + msg);
            }
        }
        #endregion
    }
}