using CommunityToolkit.Mvvm.Input;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public partial class AdminDashboardViewModel : ViewModelBase
    {

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

      
        // injected
        private readonly HomeViewModel _homeVm;
        private readonly StaffManagementViewModel _staffVm;

        public AdminDashboardViewModel(HomeViewModel homeVm,StaffManagementViewModel staffVm)
        {
            _homeVm = homeVm;
            _staffVm = staffVm;
            // Start on Home by default:
            CurrentViewModel = _homeVm;
        }

        // The [RelayCommand] attribute will automatically generate
        // a public ICommand property named 'ShowHomeCommand' (from 'ShowHome' method name)
        [RelayCommand]
        private void ShowHome()
        {
            CurrentViewModel = _homeVm;
        }

        // The [RelayCommand] attribute will automatically generate
        // a public ICommand property named 'ShowStaffManagementCommand'
        [RelayCommand]
        private void ShowStaffManagement()
        {
            CurrentViewModel = _staffVm;
        }
    }
}
