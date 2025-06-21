using System.Windows.Input;
using HospitalManagementSystem.WPF.ViewModels.Base;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowStaffManagementCommand { get; }
        public ICommand ShowPatientsCommand { get; }
        public ICommand ShowAppointmentsCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public AdminDashboardViewModel()
        {
            ShowDashboardCommand = new RelayCommand(_ => OnNav("Dashboard"));
            ShowStaffManagementCommand = new RelayCommand(_ => OnNav("StaffManagement"));
            ShowPatientsCommand = new RelayCommand(_ => OnNav("Patients"));
            ShowAppointmentsCommand = new RelayCommand(_ => OnNav("Appointments"));
            ShowSettingsCommand = new RelayCommand(_ => OnNav("Settings"));
        }

        private void OnNav(string viewName)
        {
            // TODO: fire navigation event / update CurrentViewModel
        }
    }
}
