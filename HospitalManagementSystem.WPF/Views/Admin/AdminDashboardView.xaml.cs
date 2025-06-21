using System.Windows;
using HospitalManagementSystem.WPF.ViewModels.Admin;

namespace HospitalManagementSystem.WPF.Views.Admin
{
    public partial class AdminDashboardView : Window
    {
        public AdminDashboardView(AdminDashboardViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
