using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq; // For LINQ operations
using System.Reflection; // For getting properties

namespace HospitalManagementSystem.WPF.ViewModels.Admin.GeneralUserControls
{
    public partial class UC_SearchableDataGridViewModel : ObservableObject
    {
        private ObservableCollection<object> _allItems; // Holds all original items
        [ObservableProperty]
        private ObservableCollection<object> _displayItems; // Items currently displayed in the DataGrid

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        private string _searchQuery;

        [ObservableProperty]
        private string _searchToolTip = "Enter text to search...";

        public UC_SearchableDataGridViewModel()
        {
            SearchCommand = new RelayCommand(ExecuteSearch);
        }

        // This property will be set by the parent ViewModel
        // We use an object type to make it generic for any data type
        public ObservableCollection<object> AllItems
        {
            get => _allItems;
            set
            {
                if (SetProperty(ref _allItems, value))
                {
                    // When AllItems changes, reset DisplayItems to the full list
                    DisplayItems = new ObservableCollection<object>(_allItems);
                }
            }
        }

        public IRelayCommand SearchCommand { get; }

        private void ExecuteSearch()
        {
            if (AllItems == null || string.IsNullOrWhiteSpace(SearchQuery))
            {
                // If search query is empty or AllItems is null, show all items
                DisplayItems = new ObservableCollection<object>(AllItems ?? new ObservableCollection<object>());
            }
            else
            {
                var lowerCaseSearchQuery = SearchQuery.ToLower();
                var filteredItems = AllItems.Where(item =>
                {
                    if (item == null) return false;

                    // Get all public properties of the item
                    PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // Check if any property's string representation contains the search query
                    return properties.Any(p =>
                    {
                        object value = p.GetValue(item);
                        return value != null && value.ToString().ToLower().Contains(lowerCaseSearchQuery);
                    });
                }).ToList();

                DisplayItems = new ObservableCollection<object>(filteredItems);
            }
        }
    }
}