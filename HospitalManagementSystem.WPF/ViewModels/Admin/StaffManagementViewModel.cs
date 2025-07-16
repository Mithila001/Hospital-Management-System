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

        // ─── New: CollectionViews for All, Doctors, Nurses ────────────────────────
        public ICollectionView AllStaffView { get; private set; }
        public ICollectionView DoctorsView { get; private set; }
        public ICollectionView NursesView { get; private set; }



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

            _ = LoadStaffAsync();

        }


        // NEW: Method decorated with [RelayCommand]
        // This will automatically generate a public ICommand property named 'OpenAddNewStaffWindow'.
        [RelayCommand]
        private void OpenAddNewStaffWindow() // Bind with generate name : OpenAddNewStaffWindowCommand
        {
            var addStaffVm = _addNewStaffMemberVmFactory();
            bool? result = _dialogService.ShowDialog(addStaffVm);
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