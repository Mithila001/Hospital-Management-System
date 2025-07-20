using CommunityToolkit.Mvvm.Input;
using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels.Base;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public partial class StaffManagementViewModel : ViewModelBase
    {
        private readonly IStaffRegistrationRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWpfDialogService  _dialogService;
        private readonly Func<AddNewStaffMemberViewModel> _addNewStaffMemberVmFactory;

        // ObservableCollection to bind to DataGrid
        public ObservableCollection<StaffMember> StaffMembers { get; set; } = new ObservableCollection<StaffMember>();


        // ─── CollectionViews for All, Doctors, Nurses ────────────────────────
        public ICollectionView AllStaffView { get; private set; }
        public ICollectionView DoctorsView { get; private set; }
        public ICollectionView NursesView { get; private set; }


        // Property to hold the search query from the UI
        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);
                // Optionally, apply filter immediately on text change
                ApplyFilter();
            }
        }

        // Command for the Search button
        public ICommand SearchCommand { get; }

        // ToolTip for the search box (optional, can be a simple string too)
        public string SearchToolTip => "Enter search term (ID, Employee ID, Username, Name, Role, Department, Email, Phone)";

        // NEW: Collection to hold the dynamically defined DataGridColumns
        private ObservableCollection<DataGridColumn> _staffTableColumns;
        public ObservableCollection<DataGridColumn> StaffTableColumns
        {
            get => _staffTableColumns;
            set => SetProperty(ref _staffTableColumns, value);
        }

        public Array StaffRoles => Enum.GetValues(typeof(StaffRole)); // For ComboBox binding


        // Constructor
        public StaffManagementViewModel(
            IStaffRegistrationRepository staffRepository, 
            IUnitOfWork unitOfWork,
            IWpfDialogService dialogService,
            Func<AddNewStaffMemberViewModel> addNewStaffMemberVmFactory)
        {
            _staffRepository = staffRepository;
            _unitOfWork = unitOfWork;
            _dialogService = dialogService;
            _addNewStaffMemberVmFactory = addNewStaffMemberVmFactory;

            



            // ─── Initialize filtered views ──────────────────────────────────

            AllStaffView = CollectionViewSource.GetDefaultView(StaffMembers);
            DoctorsView = new ListCollectionView(StaffMembers)
            {
                Filter = o => ((StaffMember)o).StaffRole == StaffRole.Doctor
            };
            NursesView = new ListCollectionView(StaffMembers)
            {
                Filter = o => ((StaffMember)o).StaffRole == StaffRole.Doctor
            };

            // Initialize commands
            SearchCommand = new RelayCommand(ApplyFilter);

            // Setup the dynamic columns
            SetupStaffTableColumns();

            _ = LoadStaffAsync();

        }

        // Helper method to define your columns
        private void SetupStaffTableColumns()
        {
            StaffTableColumns = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Employee ID", Binding = new Binding("EmployeeId"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Username", Binding = new Binding("UserName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Middle Name", Binding = new Binding("MiddleName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Role", Binding = new Binding("StaffRole"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Department", Binding = new Binding("Department"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Email", Binding = new Binding("Email"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Primary Phone", Binding = new Binding("PrimaryPhone"), Width = DataGridLength.Auto }
            };

            
        }

        // The filtering predicate for AllStaffView
        private bool FilterStaff(object item)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                return true; // No search query, show all items
            }

            var staffMember = item as StaffMember;
            if (staffMember == null) return false;

            string lowerCaseSearchQuery = SearchQuery.ToLower();

            // Perform case-insensitive search across multiple properties
            return staffMember.Id.ToString().Contains(lowerCaseSearchQuery) ||
                   staffMember.EmployeeId?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.UserName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.FirstName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.MiddleName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.LastName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.StaffRole.ToString().ToLower().Contains(lowerCaseSearchQuery) || // Enum to string
                   staffMember.Department?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.Email?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.PrimaryPhone?.ToLower().Contains(lowerCaseSearchQuery) == true;
        }

        // Method to apply the filter (called by SearchCommand or SearchQuery's setter)
        private void ApplyFilter()
        {
            AllStaffView?.Refresh();
        }


        // NEW: Method decorated with [RelayCommand]
        // This will automatically generate a public ICommand property named 'OpenAddNewStaffWindow'.
        [RelayCommand]
        private async void OpenAddNewStaffWindow() // Bind with generate name : OpenAddNewStaffWindowCommand
        {
            var addStaffVm = _addNewStaffMemberVmFactory();
            bool? result = _dialogService.ShowDialog(addStaffVm);

            if(result == true)
            {
                await LoadStaffAsync();

                AllStaffView.Refresh();
                DoctorsView.Refresh();
                NursesView.Refresh();
            }
        }

        public async Task LoadStaffAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var list = await _staffRepository.GetAllAsync();
                StaffMembers.Clear();
                foreach (var staff in list)
                    StaffMembers.Add(staff);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}