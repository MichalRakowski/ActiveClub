using ActiveClub.Firma.Helper;
using ActiveClub.Shared.DTOs.Kluby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class WybierzKlubViewModel : WorkspaceViewModel
    {
        private readonly List<KlubDto> _allItems;

        public event EventHandler? Accepted;

        public ObservableCollection<KlubDto> FilteredItems { get; } = new();

        private KlubDto? _selectedItem;
        public KlubDto? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value ?? string.Empty;
                OnPropertyChanged(nameof(SearchText));
                RefreshFilter();
            }
        }

        private BaseCommand? _selectCommand;
        public ICommand SelectCommand => _selectCommand ??= new BaseCommand(Select);

        public WybierzKlubViewModel(List<KlubDto> items)
        {
            DisplayName = "Wybierz klub";
            _allItems = items;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            FilteredItems.Clear();

            var query = SearchText.Trim().ToLower();

            var result = string.IsNullOrWhiteSpace(query)
                ? _allItems
                : _allItems.Where(k =>
                    k.Nazwa.ToLower().Contains(query));

            foreach (var item in result)
                FilteredItems.Add(item);
        }

        private void Select()
        {
            if (SelectedItem == null)
                return;

            Accepted?.Invoke(this, EventArgs.Empty);
        }
    }
}