using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels
{
    public class StaffManagementViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogService _dialogService;

        // ObservableCollection to bind to DataGrid
        public ObservableCollection<StaffMember> StaffMembers { get; set; } = new ObservableCollection<StaffMember>();

        private StaffMember _selectedStaffMember;
        public StaffMember SelectedStaffMember
        {
            get => _selectedStaffMember;
            set
            {
                _selectedStaffMember = value;
                OnPropertyChanged();
                // When selection changes, load it into the editor or clear it
                if (value != null)
                {
                    CurrentStaffMember = new StaffMember // Create a copy for editing
                    {
                        Id = value.Id,
                        FirstName = value.FirstName,
                        LastName = value.LastName,
                        Username = value.Username,
                        Password = value.Password, // For demo, avoid sensitive data here in real app
                        Role = value.Role,
                        DateOfBirth = value.DateOfBirth,
                        PhoneNumber = value.PhoneNumber,
                        Email = value.Email
                    };
                }
                else
                {
                    CurrentStaffMember = new StaffMember(); // Clear form if no selection
                }
                ResetValidationErrors(); // Clear validation errors when selection changes
            }
        }

        private StaffMember _currentStaffMember; // For Add/Edit form
        public StaffMember CurrentStaffMember
        {
            get => _currentStaffMember;
            set
            {
                _currentStaffMember = value;
                OnPropertyChanged();
                // Notify properties that validation depends on
                OnPropertyChanged(nameof(CurrentStaffMember.FirstName));
                OnPropertyChanged(nameof(CurrentStaffMember.LastName));
                OnPropertyChanged(nameof(CurrentStaffMember.Username));
                OnPropertyChanged(nameof(CurrentStaffMember.Password));
                OnPropertyChanged(nameof(CurrentStaffMember.DateOfBirth));
                OnPropertyChanged(nameof(CurrentStaffMember.PhoneNumber));
                OnPropertyChanged(nameof(CurrentStaffMember.Email));
            }
        }

        public Array StaffRoles => Enum.GetValues(typeof(StaffRole)); // For ComboBox binding

        // Commands
        public ICommand LoadStaffCommand { get; }
        public ICommand AddStaffCommand { get; }
        public ICommand UpdateStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand ClearFormCommand { get; }

        public StaffManagementViewModel(IStaffRepository staffRepository, IUnitOfWork unitOfWork, IDialogService dialogService)
        {
            _staffRepository = staffRepository;
            _unitOfWork = unitOfWork;
            _dialogService = dialogService;

            CurrentStaffMember = new StaffMember(); // Initialize an empty staff member for the form

            LoadStaffCommand = new RelayCommand(async _ => await LoadStaffAsync());
            AddStaffCommand = new RelayCommand(async _ => await AddStaffAsync(), _ => CanAddStaff());
            UpdateStaffCommand = new RelayCommand(async _ => await UpdateStaffAsync(), _ => CanUpdateStaff());
            DeleteStaffCommand = new RelayCommand(async _ => await DeleteStaffAsync(), _ => CanDeleteStaff());
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            // Load staff members on startup of the ViewModel
            Task.Run(LoadStaffAsync);
        }

        private async Task LoadStaffAsync()
        {
            try
            {
                StaffMembers.Clear();
                var staff = await _staffRepository.GetAllAsync();
                foreach (var member in staff)
                {
                    StaffMembers.Add(member);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessage("Error", $"Failed to load staff members: {ex.Message}");
            }
        }

        private async Task AddStaffAsync()
        {
            if (!IsValid())
            {
                await _dialogService.ShowMessage("Validation Error", "Please correct the errors in the form.");
                return;
            }

            try
            {
                await _staffRepository.AddAsync(CurrentStaffMember);
                await _unitOfWork.CompleteAsync();
                await _dialogService.ShowMessage("Success", "Staff member added successfully!");
                await LoadStaffAsync(); // Reload list
                ClearForm();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessage("Error", $"Failed to add staff member: {ex.Message}");
            }
        }

        private bool CanAddStaff()
        {
            // Allow adding if a new staff member is being created and is valid
            return CurrentStaffMember != null && CurrentStaffMember.Id == 0 && IsValid();
        }

        private async Task UpdateStaffAsync()
        {
            if (!IsValid())
            {
                await _dialogService.ShowMessage("Validation Error", "Please correct the errors in the form.");
                return;
            }
            if (SelectedStaffMember == null || SelectedStaffMember.Id == 0)
            {
                await _dialogService.ShowMessage("Error", "No staff member selected for update.");
                return;
            }

            try
            {
                await _staffRepository.UpdateAsync(CurrentStaffMember);
                await _unitOfWork.CompleteAsync();
                await _dialogService.ShowMessage("Success", "Staff member updated successfully!");
                await LoadStaffAsync(); // Reload list
                ClearForm();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessage("Error", $"Failed to update staff member: {ex.Message}");
            }
        }

        private bool CanUpdateStaff()
        {
            // Allow updating if a staff member is selected AND the form data is valid
            return SelectedStaffMember != null && SelectedStaffMember.Id != 0 && IsValid();
        }

        private async Task DeleteStaffAsync()
        {
            if (SelectedStaffMember == null)
            {
                await _dialogService.ShowMessage("Error", "Please select a staff member to delete.");
                return;
            }

            bool confirmed = await _dialogService.ShowConfirmation("Confirm Delete", $"Are you sure you want to delete {SelectedStaffMember.FullName}?");
            if (!confirmed) return;

            try
            {
                await _staffRepository.DeleteAsync(SelectedStaffMember.Id);
                await _unitOfWork.CompleteAsync();
                await _dialogService.ShowMessage("Success", "Staff member deleted successfully!");
                await LoadStaffAsync(); // Reload list
                ClearForm();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessage("Error", $"Failed to delete staff member: {ex.Message}");
            }
        }

        private bool CanDeleteStaff()
        {
            return SelectedStaffMember != null && SelectedStaffMember.Id != 0;
        }

        private void ClearForm()
        {
            SelectedStaffMember = null; // Clear selection
            CurrentStaffMember = new StaffMember(); // Reset the form fields
            ResetValidationErrors();
        }

        // --- IDataErrorInfo Implementation for UI Validation ---
        public string Error => null; // Class-level error (not used here, but required by interface)

        public string this[string columnName]
        {
            get
            {
                if (CurrentStaffMember == null) return string.Empty;

                switch (columnName)
                {
                    case nameof(CurrentStaffMember.FirstName):
                        if (string.IsNullOrWhiteSpace(CurrentStaffMember.FirstName))
                            return "First Name is required.";
                        break;
                    case nameof(CurrentStaffMember.LastName):
                        if (string.IsNullOrWhiteSpace(CurrentStaffMember.LastName))
                            return "Last Name is required.";
                        break;
                    case nameof(CurrentStaffMember.Username):
                        if (string.IsNullOrWhiteSpace(CurrentStaffMember.Username))
                            return "Username is required.";
                        if (CurrentStaffMember.Username.Length < 3 || CurrentStaffMember.Username.Length > 50)
                            return "Username must be between 3 and 50 characters.";
                        // Basic check, unique username check would ideally be done async on blur or before save
                        break;
                    case nameof(CurrentStaffMember.Password):
                        if (string.IsNullOrWhiteSpace(CurrentStaffMember.Password))
                            return "Password is required.";
                        if (CurrentStaffMember.Password.Length < 6)
                            return "Password must be at least 6 characters.";
                        break;
                    case nameof(CurrentStaffMember.DateOfBirth):
                        if (CurrentStaffMember.DateOfBirth == DateTime.MinValue)
                            return "Date of Birth is required.";
                        if (CurrentStaffMember.DateOfBirth > DateTime.Now)
                            return "Date of Birth cannot be in the future.";
                        break;
                    case nameof(CurrentStaffMember.PhoneNumber):
                        if (!string.IsNullOrWhiteSpace(CurrentStaffMember.PhoneNumber) &&
                            !System.Text.RegularExpressions.Regex.IsMatch(CurrentStaffMember.PhoneNumber, @"^\+?\d{10,15}$"))
                            return "Invalid phone number format.";
                        break;
                    case nameof(CurrentStaffMember.Email):
                        if (!string.IsNullOrWhiteSpace(CurrentStaffMember.Email) &&
                            !System.Text.RegularExpressions.Regex.IsMatch(CurrentStaffMember.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            return "Invalid email address format.";
                        break;
                }
                return string.Empty; // No error
            }
        }

        private bool IsValid()
        {
            // Check all properties for validation errors
            if (CurrentStaffMember == null) return false;

            return string.IsNullOrEmpty(this[nameof(CurrentStaffMember.FirstName)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.LastName)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.Username)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.Password)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.DateOfBirth)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.PhoneNumber)]) &&
                   string.IsNullOrEmpty(this[nameof(CurrentStaffMember.Email)]);
        }

        private void ResetValidationErrors()
        {
            // Trigger property changed for all validated properties to clear error display
            OnPropertyChanged(nameof(CurrentStaffMember.FirstName));
            OnPropertyChanged(nameof(CurrentStaffMember.LastName));
            OnPropertyChanged(nameof(CurrentStaffMember.Username));
            OnPropertyChanged(nameof(CurrentStaffMember.Password));
            OnPropertyChanged(nameof(CurrentStaffMember.DateOfBirth));
            OnPropertyChanged(nameof(CurrentStaffMember.PhoneNumber));
            OnPropertyChanged(nameof(CurrentStaffMember.Email));
        }
    }
}