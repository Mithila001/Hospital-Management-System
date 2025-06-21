using HospitalManagementSystem.WPF.ViewModels.Base;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public class AdminDashboardViewModel : ViewModelBase
    {

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand ShowStaffManagementCommand { get; }
        // injected
        private readonly StaffManagementViewModel _staffVm;

        public AdminDashboardViewModel(StaffManagementViewModel staffVm)
        {
            _staffVm = staffVm;

            ShowStaffManagementCommand = new RelayCommand(_ => CurrentViewModel = _staffVm);

            // you can initialize with a default:
            CurrentViewModel = null; // or maybe a DashboardHomeViewModel
        }
    }
}
