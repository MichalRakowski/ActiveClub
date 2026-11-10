using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ActiveClubFirma.ViewModels
{
    public class NowyKlubViewModel : WorkspaceViewModel
    {
        #region Events
        public event EventHandler KlubSaved;
        #endregion

        #region DB
        private ActiveClubEntities1 activeClubEntities1;
        #endregion

        #region Item
        private Kluby klub;
        #endregion

        #region Commands
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
        public NowyKlubViewModel()
        {
            base.DisplayName = "Klub";

            activeClubEntities1 = new ActiveClubEntities1();

            klub = new Kluby
            {
                DataUtworzenia = DateTime.Now,
                CzyAktywny = true
            };
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get { return klub.Nazwa; }
            set { klub.Nazwa = value; OnPropertyChanged("Nazwa"); }
        }

        public string Miasto
        {
            get { return klub.Miasto; }
            set { klub.Miasto = value; OnPropertyChanged("Miasto"); }
        }

        public string Ulica
        {
            get { return klub.Ulica; }
            set { klub.Ulica = value; OnPropertyChanged("Ulica"); }
        }

        public string KodPocztowy
        {
            get { return klub.KodPocztowy; }
            set { klub.KodPocztowy = value; OnPropertyChanged("KodPocztowy"); }
        }

        public string Telefon
        {
            get { return klub.Telefon; }
            set { klub.Telefon = value; OnPropertyChanged("Telefon"); }
        }

        public string EmailKontakt
        {
            get { return klub.EmailKontakt; }
            set { klub.EmailKontakt = value; OnPropertyChanged("EmailKontakt"); }
        }

        public string Opis
        {
            get { return klub.Opis; }
            set { klub.Opis = value; OnPropertyChanged("Opis"); }
        }

        public bool CzyAktywny
        {
            get { return klub.CzyAktywny; }
            set { klub.CzyAktywny = value; OnPropertyChanged("CzyAktywny"); }
        }

        public DateTime DataUtworzenia
        {
            get { return klub.DataUtworzenia; }
            set { klub.DataUtworzenia = value; OnPropertyChanged("DataUtworzenia"); }
        }
        #endregion

        #region Helpers
        private bool ValidateBeforeSave(out string message)
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                message = "Nazwa klubu jest wymagana.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Miasto))
            {
                message = "Miasto jest wymagane.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(EmailKontakt) && !EmailKontakt.Contains("@"))
            {
                message = "Email kontaktowy wygląda na niepoprawny.";
                return false;
            }

            // (opcjonalnie) prosta walidacja kodu pocztowego PL: 00-000
            if (!string.IsNullOrWhiteSpace(KodPocztowy))
            {
                var kp = KodPocztowy.Trim();
                if (kp.Length != 6 || kp[2] != '-')
                {
                    message = "Kod pocztowy powinien mieć format 00-000.";
                    return false;
                }
            }

            message = null;
            return true;
        }

        public bool Save()
        {
            if (!ValidateBeforeSave(out var msg))
            {
                MessageBox.Show(msg, "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            try
            {
                activeClubEntities1.Kluby.Add(klub);
                activeClubEntities1.SaveChanges();

                KlubSaved?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Nie można zapisać klubu. Popraw dane:");
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