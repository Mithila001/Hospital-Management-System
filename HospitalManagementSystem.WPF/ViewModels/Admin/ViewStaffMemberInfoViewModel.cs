using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HospitalManagementSystem.Core.Models.Admin; // Make sure this path is correct for StaffMember
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    /// <summary>
    /// ViewModel for displaying detailed information about a single staff member.
    /// </summary>
    public partial class ViewStaffMemberInfoViewModel : ViewModelBase
    {
        // Property to hold the staff member data
        [ObservableProperty]
        private StaffMember _selectedStaffMember;
        [ObservableProperty]
        private string _title = string.Empty;

        /// <summary>
        /// Action to request the closing of the dialog.
        /// The DialogService will set this.
        /// </summary>
        public Action<bool?>? RequestClose { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStaffMemberInfoViewModel"/> class.
        /// </summary>
        /// <param name="staffMember">The staff member to display.</param>
        public ViewStaffMemberInfoViewModel(StaffMember staffMember)
        {
            _selectedStaffMember = staffMember ?? throw new ArgumentNullException(nameof(staffMember));
            Title = $"Staff Details: {staffMember.UserName}"; // Set window title
        }

        /// <summary>
        /// Command to close the staff details window.
        /// </summary>
        [RelayCommand]
        private void CloseWindow()
        {
            RequestClose?.Invoke(null); // Close without a specific result
        }
    }
}