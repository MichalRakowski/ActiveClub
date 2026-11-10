using ActiveClub.Shared.DTOs.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ActiveClub.Firma.ViewModels
{
    public class WybierzHarmonogramViewModel : WorkspaceViewModel
    {
        private LookupItemDto? _selectedItem;

        public ObservableCollection<LookupItemDto> Items { get; }

        public LookupItemDto? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public WybierzHarmonogramViewModel(IEnumerable<LookupItemDto> items)
        {
            DisplayName = "Wybierz harmonogram";
            Items = new ObservableCollection<LookupItemDto>(items ?? Enumerable.Empty<LookupItemDto>());
        }
    }
}