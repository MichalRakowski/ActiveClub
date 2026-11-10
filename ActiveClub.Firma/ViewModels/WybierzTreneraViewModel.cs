using ActiveClub.Firma.Helper;
using ActiveClub.Shared.DTOs.Trenerzy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class WybierzTreneraViewModel : WorkspaceViewModel
    {
        private readonly List<TrenerDto> _allItems;

        public event EventHandler? Accepted;

        public ObservableCollection<TrenerDto> FilteredItems { get; } = new();

        private TrenerDto? _selectedItem;
        public TrenerDto? SelectedItem
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

        public WybierzTreneraViewModel(List<TrenerDto> items)
        {
            DisplayName = "Wybierz trenera";
            _allItems = items;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            FilteredItems.Clear();

            var query = SearchText.Trim().ToLower();

            var result = string.IsNullOrWhiteSpace(query)
                ? _allItems
                : _allItems.Where(t =>
                    t.Imie.ToLower().Contains(query) ||
                    t.Nazwisko.ToLower().Contains(query) ||
                    (t.Email ?? string.Empty).ToLower().Contains(query) ||
                    (t.Telefon ?? string.Empty).ToLower().Contains(query));

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