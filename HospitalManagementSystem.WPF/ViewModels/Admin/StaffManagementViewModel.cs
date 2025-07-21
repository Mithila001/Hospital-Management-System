using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    /// <summary>
    /// ViewModel for managing staff, providing data and commands for display, search, and additions.
    /// Utilizes CommunityToolkit.Mvvm for efficient property and command management.
    /// </summary>
    /// <remarks>
    /// Assumes <see cref="ViewModelBase"/> is configured to support <see cref="ObservableObject"/> or equivalent
    /// for CommunityToolkit.Mvvm source generation.
    /// </remarks>
    public partial class StaffManagementViewModel : ViewModelBase
    {
        #region Private Fields


        private readonly IStaffRegistrationRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWpfDialogService _dialogService;
        private readonly Func<AddNewStaffMemberViewModel> _addNewStaffMemberVmFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the observable collection of staff members displayed in the DataGrid.
        /// </summary>
        public ObservableCollection<StaffMember> StaffMembers { get; } = new ObservableCollection<StaffMember>();

        /// <summary>
        /// Gets the default view for all staff members, enabling filtering.
        /// </summary>
        public ICollectionView AllStaffView { get; private set; }

        /// <summary>
        /// Gets a filtered view specifically for staff members with the Doctor role.
        /// </summary>
        public ICollectionView DoctorsView { get; private set; }

        /// <summary>
        /// Gets a filtered view specifically for staff members with the Nurse role.
        /// </summary>
        public ICollectionView NursesView { get; private set; }

        /// <summary>
        /// Gets or sets the current search query.
        /// <para>Automatically generates `SearchQuery` property and raises PropertyChanged.</para>
        /// </summary>
        [ObservableProperty]
        private string _searchQuery = string.Empty;

        /// <summary>
        /// Gets the tooltip text for the search input field.
        /// </summary>
        public string SearchToolTip => "Enter search term (ID, Employee ID, Username, Name, Role, Department, Email, Phone)";

        /// <summary>
        /// Gets or sets the collection of DataGridColumn objects defining the table's structure.
        /// <para>Automatically generates `StaffTableColumns` property and raises PropertyChanged.</para>
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DataGridColumn> _staffTableColumns;

        /// <summary>
        /// Gets an array of all available <see cref="StaffRole"/> enum values for UI binding.
        /// </summary>
        public Array StaffRoles => Enum.GetValues(typeof(StaffRole));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffManagementViewModel"/> class.
        /// </summary>
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

            InitializeCollectionAndViews();
            SetupStaffTableColumns();

            // Load staff data asynchronously on ViewModel creation.
            _ = LoadStaffAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configures collection views for filtering and display.
        /// </summary>
        private void InitializeCollectionAndViews()
        {
            AllStaffView = CollectionViewSource.GetDefaultView(StaffMembers);
            AllStaffView.Filter = FilterStaff; // Apply general search filter

            DoctorsView = new ListCollectionView(StaffMembers)
            {
                Filter = o => o is StaffMember sm && sm.StaffRole == StaffRole.Doctor
            };
            NursesView = new ListCollectionView(StaffMembers)

            {
                Filter = o => o is StaffMember sm && sm.StaffRole == StaffRole.Nurse
            };
        }

        /// <summary>
        /// Defines the columns for the staff data table.
        /// </summary>
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

        /// <summary>
        /// Called automatically by the CommunityToolkit.Mvvm source generator when <see cref="SearchQuery"/> changes.
        /// Triggers the filtering of staff data.
        /// </summary>
        /// <param name="value">The new search query value.</param>
        partial void OnSearchQueryChanged(string value)
        {
            ApplyFilter();
        }

        /// <summary>
        /// Refreshes all collection views based on the current search query and filters.
        /// <para>This method generates the `ApplyFilterCommand` for UI binding.</para>
        /// </summary>
        [RelayCommand]
        private void ApplyFilter()
        {
            AllStaffView?.Refresh();
            DoctorsView?.Refresh();
            NursesView?.Refresh();
        }

        /// <summary>
        /// The filtering predicate for staff members based on the <see cref="SearchQuery"/>.
        /// </summary>
        /// <param name="item">The staff member object to evaluate.</param>
        /// <returns>True if the staff member matches the search query, otherwise false.</returns>
        private bool FilterStaff(object item)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                return true; // No search query, show all items
            }

            if (item is not StaffMember staffMember) return false;

            string lowerCaseSearchQuery = SearchQuery.ToLower();

            // Perform case-insensitive search across relevant staff member properties.
            return staffMember.Id.ToString().Contains(lowerCaseSearchQuery) ||
                   staffMember.EmployeeId?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.UserName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.FirstName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.MiddleName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.LastName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.StaffRole.ToString().ToLower().Contains(lowerCaseSearchQuery) ||
                   staffMember.Department?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.Email?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.PrimaryPhone?.ToLower().Contains(lowerCaseSearchQuery) == true;
        }

        /// <summary>
        /// Opens a dialog to add a new staff member. If successful, reloads staff data.
        /// <para>This method generates the `OpenAddNewStaffWindowCommand` for UI binding.</para>
        /// </summary>
        [RelayCommand]
        private async Task OpenAddNewStaffWindowAsync()
        {
            var addStaffVm = _addNewStaffMemberVmFactory();
            bool? result = _dialogService.ShowDialog(addStaffVm);

            if (result == true)
            {
                await LoadStaffAsync();
                AllStaffView.Refresh();
                DoctorsView.Refresh();
                NursesView.Refresh();
            }
        }

        /// <summary>
        /// Asynchronously loads all staff members from the repository and populates the <see cref="StaffMembers"/> collection.
        /// Manages the <see cref="ViewModelBase.IsBusy"/> state during the operation.
        /// </summary>
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

        #endregion
    }
}