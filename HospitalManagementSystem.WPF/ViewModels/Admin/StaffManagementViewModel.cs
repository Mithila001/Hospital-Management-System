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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly Func<StaffMember, ViewStaffMemberInfoViewModel> _viewStaffMemberInfoVmFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the observable collection of staff members displayed in the DataGrid.
        /// </summary>
        public ObservableCollection<StaffMember> StaffMembers { get; } = new ObservableCollection<StaffMember>();

        public ICollectionView AllStaffView { get; private set; }

        public ICollectionView DoctorsView { get; private set; }

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
        public string SearchToolTip => "Enter search term (Employee ID, Username, Name, Phone)";

        /// <summary>
        /// Gets or sets the collection of DataGridColumn objects defining the table's structure.
        /// <para>Automatically generates `AllStaffTableColumns` property and raises PropertyChanged.</para>
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DataGridColumn> _allStaffTableColumns;

        [ObservableProperty]
        private ObservableCollection<DataGridColumn> _doctorsTableColumns;

        [ObservableProperty]
        private ObservableCollection<DataGridColumn> _nursesTableColumns;

        [ObservableProperty]
        private ObservableCollection<string> _filterOptionsItemsSource;

        [ObservableProperty]
        private string? _selectedFilterOption; // Using nullable enum to represent "All" or no selection

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
            Func<AddNewStaffMemberViewModel> addNewStaffMemberVmFactory,
            Func<StaffMember, ViewStaffMemberInfoViewModel> viewStaffMemberInfoVmFactory)

        {
            _staffRepository = staffRepository;
            _unitOfWork = unitOfWork;
            _dialogService = dialogService;
            _addNewStaffMemberVmFactory = addNewStaffMemberVmFactory;
            _viewStaffMemberInfoVmFactory = viewStaffMemberInfoVmFactory;

            InitializeCollectionAndViews();
            SetupStaffTableColumns();
            InitializeFilterOptions();

            // Load staff data asynchronously on ViewModel creation.
            _ = LoadStaffAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the filter options for the ComboBox.
        /// </summary>
        private void InitializeFilterOptions()
        {
            var comboBoxItems = new List<string> { "All", "Staff" }; 
            FilterOptionsItemsSource = new ObservableCollection<string>(comboBoxItems); // populate the ComboBox
                                                                                
            if (FilterOptionsItemsSource.Any())
            {
                SelectedFilterOption = FilterOptionsItemsSource.First(); // Assign First Item as default selection
            }
            else
            {
                SelectedFilterOption = null; 
            }
        }
        

        /// <summary>
        /// Configures collection views for filtering and display.
        /// </summary>
        private void InitializeCollectionAndViews()
        {
            AllStaffView = CollectionViewSource.GetDefaultView(StaffMembers);
            AllStaffView.Filter = FilterStaff; // Apply general search filter

            DoctorsView = new ListCollectionView(StaffMembers)
            {
                Filter = o => o is StaffMember sm && sm.StaffRole == StaffRole.Doctor && FilterStaff(o)
            };
            NursesView = new ListCollectionView(StaffMembers)

            {
                Filter = o => o is StaffMember sm && sm.StaffRole == StaffRole.Nurse && FilterStaff(o)
            };
        }

        /// <summary>
        /// Defines the columns for the staff data tables.
        /// </summary>
        private void SetupStaffTableColumns()
        {
            // Common columns for all staff
            var commonColumns = new List<DataGridColumn>
            {
                new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Employee ID", Binding = new Binding("EmployeeId"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Username", Binding = new Binding("UserName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Middle Name", Binding = new Binding("MiddleName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Email", Binding = new Binding("Email"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Primary Phone", Binding = new Binding("PrimaryPhone"), Width = DataGridLength.Auto }
            };


            // Add an "Actions" column with a button to AllStaffTableColumns
            var allStaffColumns = new ObservableCollection<DataGridColumn>(commonColumns);
            allStaffColumns.Add(new DataGridTextColumn { Header = "Role", Binding = new Binding("StaffRole"), Width = DataGridLength.Auto });
            allStaffColumns.Add(new DataGridTextColumn { Header = "Department", Binding = new Binding("Department"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            allStaffColumns.Add(CreateActionButtonColumn("View Details", ViewStaffDetailsCommand)); // Add the button column

            AllStaffTableColumns = allStaffColumns;



            DoctorsTableColumns = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Employee ID", Binding = new Binding("EmployeeId"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Username", Binding = new Binding("UserName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn
                {
                    Header = "Name",
                    Width = new DataGridLength(2, DataGridLengthUnitType.Star), // Corresponds to Width="2*"
                    Binding = new MultiBinding
                    {
                        StringFormat = "{}{0} {1} {2}",
                        Bindings =
                        {
                            new Binding("FirstName"),
                            new Binding("MiddleName"),
                            new Binding("LastName")
                        }
                    }
                },
                new DataGridTextColumn { Header = "Specializations", Binding = new Binding("Specializations"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Med. Reg. #", Binding = new Binding("MedicalRegistrationNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Email", Binding = new Binding("Email"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Phone", Binding = new Binding("PrimaryPhone"), Width = DataGridLength.Auto }

            };

            Debug.WriteLine($"\nDoctor Table Column ======================" + DoctorsTableColumns);

            NursesTableColumns = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Employee ID", Binding = new Binding("EmployeeId"), Width = DataGridLength.Auto },
                new DataGridTextColumn { Header = "Username", Binding = new Binding("UserName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Shift", Binding = new Binding("OnCallPreferences"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Email", Binding = new Binding("Email"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) },
                new DataGridTextColumn { Header = "Phone", Binding = new Binding("PrimaryPhone"), Width = DataGridLength.Auto }
            };


        }

        

        
        private void ApplyFilter()
        {
            AllStaffView?.Refresh();
            DoctorsView?.Refresh();
            NursesView?.Refresh();
        }

        /// <summary>
        /// Command to trigger the search operation.
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            // The search filter will now only be applied when this command is executed.
            ApplyFilter();
        }

        /// <summary>
        /// The filtering predicate for staff members based on the <see cref="SearchQuery"/>.
        /// </summary>
        /// <param name="item">The staff member object to evaluate.</param>
        /// <returns>True if the staff member matches the search query, otherwise false.</returns>
        private bool FilterStaff(object item)
        {
            //Debug.WriteLine($"\nFilter Getting Called ======================");
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                return true; // No search query, show all items
            }

            if (item is not StaffMember staffMember) return false;

            string lowerCaseSearchQuery = SearchQuery.ToLower();

            // Perform case-insensitive search across relevant staff member properties.
            return staffMember.UserName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.EmployeeId?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.FirstName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.MiddleName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
                   staffMember.LastName?.ToLower().Contains(lowerCaseSearchQuery) == true ||
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



        /// <summary>
        /// Helper method to create a DataGridTemplateColumn with a button.
        /// </summary>
        /// <param name="buttonContent">The text content of the button.</param>
        /// <param name="command">The command to bind to the button.</param>
        /// <returns>A DataGridTemplateColumn containing a button.</returns>
        private DataGridTemplateColumn CreateActionButtonColumn(string buttonContent, IRelayCommand<StaffMember> command)
        {
            var column = new DataGridTemplateColumn
            {
                Header = "Actions",
                Width = DataGridLength.Auto,
                CellTemplate = new DataTemplate()
            };

            var factory = new FrameworkElementFactory(typeof(Button));
            factory.SetBinding(Button.ContentProperty, new Binding { Source = buttonContent }); // Set content directly
            factory.SetBinding(Button.CommandProperty, new Binding { Source = command }); // Bind to the command in this ViewModel
            factory.SetBinding(Button.CommandParameterProperty, new Binding(".")); // Binds the entire row's DataContext (StaffMember object)

            column.CellTemplate.VisualTree = factory;
            return column;
        }


        /// <summary>
        /// Command executed when the "View Details" button is clicked for a staff member.
        /// </summary>
        /// <param name="staffMember">The staff member object associated with the clicked button.</param>
        [RelayCommand]
        private void ViewStaffDetails(StaffMember staffMember)
        {
            if (staffMember != null)
            {
                // Implement your logic to view or edit the staff member's details.
                // This could involve opening another dialog, navigating to a new view, etc.
                var viewStaffVm = _viewStaffMemberInfoVmFactory(staffMember);
                _dialogService.ShowDialog(viewStaffVm);

                // Example: _dialogService.ShowDialog(new StaffDetailViewModel(staffMember));
            }
        }

        #endregion




    }
}