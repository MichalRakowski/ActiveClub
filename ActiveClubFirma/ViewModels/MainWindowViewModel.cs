using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;

namespace ActiveClubFirma.ViewModels  //glowny viewmodel zarzadza menu zakladkami
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<WorkspaceViewModel> _workspaces;

        private ReadOnlyCollection<CommandViewModel> _uzytkownicyCommands;
        private ReadOnlyCollection<CommandViewModel> _aktualnosciCommands;
        private ReadOnlyCollection<CommandViewModel> _klubyCommands;
        private ReadOnlyCollection<CommandViewModel> _rodzajeCzlonkostwaCommands;

        private int _usersCount;
        #endregion
        // wlasciwosc bindowania w dashbordzie
        #region KPI  
        public int UsersCount
        {
            get { return _usersCount; }
            set
            {
                _usersCount = value;
                OnPropertyChanged("UsersCount");
            }
        }
        #endregion

        #region Commands (menu)
        public ReadOnlyCollection<CommandViewModel> UzytkownicyCommands
        {
            get
            {
                if (_uzytkownicyCommands == null)
                {
                    _uzytkownicyCommands = new ReadOnlyCollection<CommandViewModel>(
                        new List<CommandViewModel>
                        {
                            new CommandViewModel("Dodaj użytkownika",
                                new BaseCommand(() => this.CreateView(new NowyUzytkownikViewModel()))),

                            new CommandViewModel("Wszyscy użytkownicy",
                                new BaseCommand(() => this.ShowAllUzytkownicy())),
                        });
                }
                return _uzytkownicyCommands;
            }
        }

        public ReadOnlyCollection<CommandViewModel> AktualnosciCommands
        {
            get
            {
                if (_aktualnosciCommands == null)
                {
                    _aktualnosciCommands = new ReadOnlyCollection<CommandViewModel>(
                        new List<CommandViewModel>
                        {
                            new CommandViewModel("Dodaj aktualność",
                                new BaseCommand(() => this.CreateView(new NowaAktualnoscViewModel()))),

                            new CommandViewModel("Wszystkie aktualności",
                                new BaseCommand(() => this.ShowAllAktualnosci())),
                        });
                }
                return _aktualnosciCommands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> KlubyCommands
        {
            get
            {
                if (_klubyCommands == null)
                {
                    _klubyCommands = new ReadOnlyCollection<CommandViewModel>(
                        new List<CommandViewModel>
                        {
                    new CommandViewModel("➕ Dodaj klub",
                        new BaseCommand(() => this.CreateView(new NowyKlubViewModel()))),

                    new CommandViewModel("📋 Wszystkie kluby",
                        new BaseCommand(() => this.ShowAllKluby())),
                        });
                }
                return _klubyCommands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> RodzajeCzlonkostwaCommands
        {
            get
            {
                if (_rodzajeCzlonkostwaCommands == null)
                {
                    _rodzajeCzlonkostwaCommands = new ReadOnlyCollection<CommandViewModel>(
                        new List<CommandViewModel>
                        {
                    new CommandViewModel("➕ Dodaj rodzaj członkostwa",
                        new BaseCommand(() => this.CreateView(new NowyRodzajCzlonkostwaViewModel()))),

                    new CommandViewModel("📋 Wszystkie rodzaje członkostwa",
                        new BaseCommand(() => this.ShowAllRodzajeCzlonkostwa())),
                        });
                }
                return _rodzajeCzlonkostwaCommands;
            }
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;
            }

            if (e.OldItems != null)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            var workspace = sender as WorkspaceViewModel;
            if (workspace == null)
                return;

            this.Workspaces.Remove(workspace);

            //  odśwież licznik użytkowników po zamknięciu zakładki
            RefreshKpi();
        }
        #endregion

        #region Konstruktor
        public MainWindowViewModel()
        {
            RefreshKpi();
        }
        #endregion

        #region KPI Helpers
        public void RefreshKpi()
        {
            try
            {
                using (var db = new ActiveClubEntities1())
                {
                    UsersCount = db.Uzytkownicy.Count();
                }
            }
            catch
            {
                UsersCount = 0;
            }
        }
        #endregion

        #region Private Helpers (workspaces)
        private void CreateView(WorkspaceViewModel workspace)
        {
            //  jeśli to jest "NowyUzytkownikViewModel", to po zapisie odśwież KPI
            var nowy = workspace as NowyUzytkownikViewModel;
            if (nowy != null)
            {
                nowy.UserSaved += (s, e) => this.RefreshKpi();
            }

            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        private void ShowAllUzytkownicy()
        {
            this.ShowSingletonWorkspace<WszyscyUzytkownicyViewModel>(
                () => new WszyscyUzytkownicyViewModel());
        }

        private void ShowAllAktualnosci()
        {
            this.ShowSingletonWorkspace<WszystkieAktualnosciViewModel>(
                () => new WszystkieAktualnosciViewModel());
        }
        private void ShowAllKluby()
        {
            this.ShowSingletonWorkspace<WszystkieKlubyViewModel>(
                () => new WszystkieKlubyViewModel());
        }
        private void ShowAllRodzajeCzlonkostwa()
        {
            this.ShowSingletonWorkspace<WszystkieRodzajeCzlonkostwaViewModel>(
                () => new WszystkieRodzajeCzlonkostwaViewModel());
        }
        private void ShowSingletonWorkspace<TWorkspace>(Func<TWorkspace> factory)
            where TWorkspace : WorkspaceViewModel
        {
            var existing = this.Workspaces.OfType<TWorkspace>().FirstOrDefault();
            if (existing != null)
            {
                this.SetActiveWorkspace(existing);
                return;
            }

            var workspace = factory();
            this.CreateView(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}