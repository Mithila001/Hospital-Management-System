using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels.Base;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public class StaffManagementViewModel : ViewModelBase
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDialogService _dialogService;

        private readonly Func<AddNewStaffMemberViewModel> _addNewStaffMemberVmFactory;

        // ObservableCollection to bind to DataGrid
        public ObservableCollection<StaffMember> StaffMembers { get; set; } = new ObservableCollection<StaffMember>();

        
        public Array StaffRoles => Enum.GetValues(typeof(StaffRole)); // For ComboBox binding

        // Commands
        public ICommand OpenAddNewStaffWindow { get; }

        // Constructor
        public StaffManagementViewModel(
            IStaffRepository staffRepository, 
            IUnitOfWork unitOfWork, 
            IDialogService dialogService,
            Func<AddNewStaffMemberViewModel> addNewStaffMemberVmFactory)
        {
            _staffRepository = staffRepository;
            _unitOfWork = unitOfWork;
            _dialogService = dialogService;
            _addNewStaffMemberVmFactory = addNewStaffMemberVmFactory;

            OpenAddNewStaffWindow = new RelayCommand(_ => OpenAddStaffWindow());

        }

        private void OpenAddStaffWindow()
        {
            // Get the window from the service provider
            var addStaffVm = _addNewStaffMemberVmFactory();
            bool? result = _dialogService.ShowDialog(addStaffVm);
        }
    }
}