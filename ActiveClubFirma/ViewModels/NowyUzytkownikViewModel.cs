using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ActiveClubFirma.ViewModels
{
    public class NowyUzytkownikViewModel : WorkspaceViewModel
    {
        #region Events
        // MainWindowViewModel może się pod to podpiąć i odświeżyć KPI
        public event EventHandler UserSaved;
        #endregion

        #region DB
        private ActiveClubEntities1 activeClubEntities1;
        #endregion

        #region Item
        private Uzytkownicy uzytkownik;
        #endregion

        #region Command
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                return _SaveCommand;
            }
        }

        private BaseCommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                    _CancelCommand = new BaseCommand(() => Cancel());
                return _CancelCommand;
            }
        }
        #endregion

        #region Konstruktor
        public NowyUzytkownikViewModel()
        {
            base.DisplayName = "Użytkownik";

            activeClubEntities1 = new ActiveClubEntities1();

            uzytkownik = new Uzytkownicy();
            uzytkownik.DataUtworzenia = DateTime.Now;
        }
        #endregion

        #region Properties
        public string Imie
        {
            get { return uzytkownik.Imie; }
            set
            {
                uzytkownik.Imie = value;
                OnPropertyChanged("Imie");
            }
        }

        public string Nazwisko
        {
            get { return uzytkownik.Nazwisko; }
            set
            {
                uzytkownik.Nazwisko = value;
                OnPropertyChanged("Nazwisko");
            }
        }

        public string Email
        {
            get { return uzytkownik.Email; }
            set
            {
                uzytkownik.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Telefon
        {
            get { return uzytkownik.Telefon; }
            set
            {
                uzytkownik.Telefon = value;
                OnPropertyChanged("Telefon");
            }
        }

        public DateTime DataUtworzenia
        {
            get { return uzytkownik.DataUtworzenia; }
            set
            {
                uzytkownik.DataUtworzenia = value;
                OnPropertyChanged("DataUtworzenia");
            }
        }
        #endregion

        #region Helpers

        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Imie))
            {
                message = "Imię jest wymagane.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Nazwisko))
            {
                message = "Nazwisko jest wymagane.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                message = "Email jest wymagany.";
                return false;
            }

            // prosta kontrola formatu email (opcjonalnie)
            if (!Email.Contains("@"))
            {
                message = "Email wygląda na niepoprawny.";
                return false;
            }

            message = null;
            return true;
        }

        // Zwraca true jeśli zapis się udał
        public bool Save()
        {
            string msg;
            if (!ValidateBeforeSave(out msg))
            {
                MessageBox.Show(msg, "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            try
            {
                activeClubEntities1.Uzytkownicy.Add(uzytkownik);
                activeClubEntities1.SaveChanges();

                // Sygnał do MainWindowViewModel: „zapisano użytkownika”
                if (UserSaved != null)
                    UserSaved(this, EventArgs.Empty);

                return true;
            }
            catch (DbEntityValidationException ex)
            {
                // Konkretne info: które pola i dlaczego
                var sb = new StringBuilder();
                sb.AppendLine("Nie można zapisać użytkownika. Popraw dane:");
                sb.AppendLine();

                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine("- " + ve.PropertyName + ": " + ve.ErrorMessage);
                    }
                }

                MessageBox.Show(sb.ToString(), "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void SaveAndClose()
        {
            if (Save())
                base.OnRequestClose();
        }

        public void Cancel()
        {
            base.OnRequestClose();
        }

        #endregion
    }
}