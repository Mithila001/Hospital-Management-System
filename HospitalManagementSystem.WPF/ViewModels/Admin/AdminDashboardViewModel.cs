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

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowStaffManagementCommand { get; }

        // injected
        private readonly HomeViewModel _homeVm;
        private readonly StaffManagementViewModel _staffVm;

        public AdminDashboardViewModel(HomeViewModel homeVm,StaffManagementViewModel staffVm)
        {
            _homeVm = homeVm;
            _staffVm = staffVm;

            ShowHomeCommand = new RelayCommand(_ => CurrentViewModel = _homeVm);
            ShowStaffManagementCommand = new RelayCommand(_ => CurrentViewModel = _staffVm);

            // Start on Home by default:
            CurrentViewModel = _homeVm;
        }
    }
}
