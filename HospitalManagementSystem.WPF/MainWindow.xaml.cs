using HospitalManagementSystem.WPF.ViewModels;
using HospitalManagementSystem.WPF.ViewModels.Admin;
using HospitalManagementSystem.WPF.Views;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HospitalManagementSystem.WPF
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider) // Inject IServiceProvider
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            // Set initial content to StaffManagementView
            //ShowStaffManagementView();
            ShowAdminDashboardView();
        }

        private void ShowStaffManagementView()
        {
            var staffManagementView = _serviceProvider.GetRequiredService<StaffManagementView>();
            MainContent.Content = staffManagementView;
        }

        private void ShowAdminDashboardView()
        {
            //var adminDashboardView = _serviceProvider.GetRequiredService<AdminDashboardView>();
            //MainContent.Content = adminDashboardView;


            // You could add buttons in MainWindow.xaml later to switch views
            // e.g., <Button Content="Manage Staff" Click="ShowStaffManagementView_Click"/>
            // private void ShowStaffManagementView_Click(object sender, RoutedEventArgs e)
            // {
            //     ShowStaffManagementView();
            // }
        }
    }
}