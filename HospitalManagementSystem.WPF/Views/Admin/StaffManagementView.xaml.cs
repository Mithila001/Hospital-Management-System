using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagementSystem.WPF.ViewModels.Admin; // Important to resolve ViewModel

namespace HospitalManagementSystem.WPF.Views.Admin
{
    public partial class StaffManagementView : UserControl
    {
        public StaffManagementView()
        {
            InitializeComponent();
            // Resolve ViewModel from the DI container
            if (System.Windows.Application.Current is App app)
            {
                // Ensure that 'app.ServiceProvider' is public or accessible.
                // It is public because it's set in App.xaml.cs 'App' class directly.
                this.DataContext = app.ServiceProvider.GetRequiredService<StaffManagementViewModel>();
            }
        }
    }
}