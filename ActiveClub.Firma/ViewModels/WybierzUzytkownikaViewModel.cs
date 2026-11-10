using ActiveClub.Firma.Helper;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ActiveClub.Firma.ViewModels
{
    public class WybierzUzytkownikaViewModel : WorkspaceViewModel
    {
        private readonly List<UzytkownikDto> _allUsers;

        public event EventHandler? Accepted;

        public ObservableCollection<UzytkownikDto> FilteredUsers { get; } = new();

        private UzytkownikDto? _selectedUser;
        public UzytkownikDto? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
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

        public WybierzUzytkownikaViewModel(List<UzytkownikDto> users)
        {
            DisplayName = "Wybierz użytkownika";
            _allUsers = users;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            FilteredUsers.Clear();

            var query = SearchText.Trim().ToLower();

            var result = string.IsNullOrWhiteSpace(query)
                ? _allUsers
                : _allUsers.Where(u =>
                    u.Imie.ToLower().Contains(query) ||
                    u.Nazwisko.ToLower().Contains(query) ||
                    u.Email.ToLower().Contains(query) ||
                    (u.Telefon ?? string.Empty).ToLower().Contains(query) ||
                    (u.Rola ?? string.Empty).ToLower().Contains(query));

            foreach (var user in result)
                FilteredUsers.Add(user);
        }

        private void Select()
        {
            if (SelectedUser == null)
                return;

            Accepted?.Invoke(this, EventArgs.Empty);
        }
    }
}