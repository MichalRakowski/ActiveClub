using ActiveClub.Firma.Helper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel
    {
        #region Commands

        private BaseCommand _loadCommand;
        private BaseCommand _addCommand;
        private BaseCommand _editCommand;
        private BaseCommand _deleteCommand;
        private BaseCommand _clearFiltersCommand;

        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                    _loadCommand = new BaseCommand(() => Load());
                return _loadCommand;
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new BaseCommand(() => Add());
                return _addCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new BaseCommand(() => Edit());
                return _editCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new BaseCommand(() => Delete());
                return _deleteCommand;
            }
        }

        public ICommand ClearFiltersCommand
        {
            get
            {
                if (_clearFiltersCommand == null)
                    _clearFiltersCommand = new BaseCommand(() => ClearFilters());
                return _clearFiltersCommand;
            }
        }

        #endregion

        #region SelectedItem

        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion

        #region List

        private ObservableCollection<T> _list;
        public ObservableCollection<T> List
        {
            get
            {
                if (_list == null)
                    Load();

                return _list;
            }
            set
            {
                _list = value ?? new ObservableCollection<T>();
                OnPropertyChanged(nameof(List));
                RefreshView();
            }
        }

        #endregion

        #region Filter / Sort

        private ICollectionView _itemsView;
        public ICollectionView ItemsView
        {
            get => _itemsView;
            private set
            {
                _itemsView = value;
                OnPropertyChanged(nameof(ItemsView));
            }
        }

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value ?? string.Empty;
                OnPropertyChanged(nameof(FilterText));

                try
                {
                    ItemsView?.Refresh();
                }
                catch (InvalidOperationException)
                {
                    // DataGrid jest aktualnie w trakcie edycji
                }
            }
        }

        public ObservableCollection<string> SortProperties { get; } = new ObservableCollection<string>();

        private string _selectedSortProperty;
        public string SelectedSortProperty
        {
            get => _selectedSortProperty;
            set
            {
                _selectedSortProperty = value;
                OnPropertyChanged(nameof(SelectedSortProperty));
                ApplySorting();
            }
        }

        private int _sortDirectionIndex;
        public int SortDirectionIndex
        {
            get => _sortDirectionIndex;
            set
            {
                _sortDirectionIndex = value;
                OnPropertyChanged(nameof(SortDirectionIndex));
                ApplySorting();
            }
        }

        #endregion

        #region Constructor

        protected WszystkieViewModel(string displayName)
        {
            DisplayName = displayName;
            SortDirectionIndex = 0;
            InitializeSortProperties();
        }

        #endregion

        #region Helpers

        public abstract void Load();

        protected virtual void Add() { }
        protected virtual void Edit() { }
        protected virtual void Delete() { }

        protected virtual void RefreshView()
        {
            try
            {
                ItemsView = CollectionViewSource.GetDefaultView(List);
                if (ItemsView == null)
                    return;

                ItemsView.Filter = FilterObject;
                ApplySorting();
                ItemsView.Refresh();
            }
            catch (InvalidOperationException)
            {
                // DataGrid jest aktualnie w trakcie AddNew/EditItem
            }
        }

        private bool FilterObject(object obj)
        {
            if (obj is T item)
                return FilterItem(item, FilterText);

            return false;
        }

        protected virtual bool FilterItem(T item, string filterText)
        {
            if (string.IsNullOrWhiteSpace(filterText))
                return true;

            var text = filterText.Trim().ToLower();

            var stringProperties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string));

            foreach (var property in stringProperties)
            {
                var value = property.GetValue(item) as string;

                if (!string.IsNullOrWhiteSpace(value) &&
                    value.ToLower().Contains(text))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void InitializeSortProperties()
        {
            SortProperties.Clear();

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.PropertyType == typeof(string) ||
                    p.PropertyType == typeof(int) ||
                    p.PropertyType == typeof(decimal) ||
                    p.PropertyType == typeof(double) ||
                    p.PropertyType == typeof(DateTime) ||
                    p.PropertyType == typeof(bool) ||
                    p.PropertyType == typeof(int?) ||
                    p.PropertyType == typeof(decimal?) ||
                    p.PropertyType == typeof(double?) ||
                    p.PropertyType == typeof(DateTime?) ||
                    p.PropertyType == typeof(bool?))
                .Select(p => p.Name);

            foreach (var property in properties)
                SortProperties.Add(property);

            if (SortProperties.Any() && string.IsNullOrWhiteSpace(SelectedSortProperty))
                SelectedSortProperty = SortProperties.First();
        }

        protected virtual void ApplySorting()
        {
            if (ItemsView == null)
                return;

            var editableView = ItemsView as IEditableCollectionView;

            if (editableView != null &&
                (editableView.IsAddingNew || editableView.IsEditingItem))
            {
                return;
            }

            try
            {
                using (ItemsView.DeferRefresh())
                {
                    ItemsView.SortDescriptions.Clear();

                    if (string.IsNullOrWhiteSpace(SelectedSortProperty))
                        return;

                    var direction = SortDirectionIndex == 0
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending;

                    ItemsView.SortDescriptions.Add(
                        new SortDescription(SelectedSortProperty, direction));
                }
            }
            catch (InvalidOperationException)
            {
                // DataGrid jest aktualnie w trakcie AddNew/EditItem
            }
        }

        protected virtual void ClearFilters()
        {
            FilterText = string.Empty;
            SortDirectionIndex = 0;

            if (SortProperties.Any())
                SelectedSortProperty = SortProperties.First();

            try
            {
                ItemsView?.Refresh();
            }
            catch (InvalidOperationException)
            {
                // DataGrid jest aktualnie w trakcie edycji
            }
        }

        #endregion
    }
}