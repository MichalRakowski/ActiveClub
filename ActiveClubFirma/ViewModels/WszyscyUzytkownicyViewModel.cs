using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;

namespace ActiveClubFirma.ViewModels
{
    public class WszyscyUzytkownicyViewModel : WszyscyViewModel<Uzytkownicy>
    {



        #region Properties

        #endregion
        #region Constructor
        public WszyscyUzytkownicyViewModel()
            : base("Uzytkownicy")
        {


        }
        #endregion
        #region Helpers

        #endregion
        public override void Load()

        {
            List = new ObservableCollection<Uzytkownicy>
             (
                 activeClubEntities1.Uzytkownicy.ToList()
             );
        }
        protected override void Delete()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano użytkownika.");
                return;
            }

            activeClubEntities1.Uzytkownicy.Remove(SelectedItem);
            activeClubEntities1.SaveChanges();
            Load();
        }
        protected override void Add()
        {
            var nowy = new Uzytkownicy
            {
                DataUtworzenia = DateTime.Now
            };

            activeClubEntities1.Uzytkownicy.Add(nowy); // <--- ważne (tracking)
            List.Add(nowy);
            SelectedItem = nowy;
        }

        protected override void Edit()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("Nie wybrano użytkownika.");
                return;
            }

            try
            {
                // 1) Wymuś zatwierdzenie edycji w DataGrid (ważne w WPF!)
                Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);

                // 2) Dopnij encję do kontekstu i ustaw stan
                var entry = activeClubEntities1.Entry(SelectedItem);

                if (entry.State == EntityState.Detached)
                {
                    // jeśli rekord już istnieje w DB – Attach
                    // jeśli nowy – Add
                    if (SelectedItem.UzytkownikId == 0)
                        activeClubEntities1.Uzytkownicy.Add(SelectedItem);
                    else
                        activeClubEntities1.Uzytkownicy.Attach(SelectedItem);

                    entry = activeClubEntities1.Entry(SelectedItem);
                }

                if (SelectedItem.UzytkownikId == 0)
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
    }
}
