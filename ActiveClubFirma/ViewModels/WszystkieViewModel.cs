using ActiveClubFirma.Helper;
using ActiveClubFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ActiveClubFirma.ViewModels
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel
    {
        #region DB
        protected readonly ActiveClubEntities1 activeClubEntities1;
        #endregion

        #region Command
        private BaseCommand _LoadCommand;
        private BaseCommand _AddCommand;
        private BaseCommand _EditCommand;
        private BaseCommand _DeleteCommand;

        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }

        
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                    _AddCommand = new BaseCommand(() => Add());
                return _AddCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                    _EditCommand = new BaseCommand(() => Edit());
                return _EditCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                    _DeleteCommand = new BaseCommand(() => Delete());
                return _DeleteCommand;
            }
        }
        #endregion

        #region SelectedItem
        private T _SelectedItem;
        public T SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(nameof(List));
            }
        }
        #endregion

        #region Constructor
        public WszystkieViewModel(string displayName)
        {
            activeClubEntities1 = new ActiveClubEntities1();

            

            base.DisplayName = displayName;
        }
        #endregion

        #region Helpers
        public abstract void Load();

        protected virtual void Add() { }
        protected virtual void Edit() { }
        protected virtual void Delete() { }
        #endregion
    }
}