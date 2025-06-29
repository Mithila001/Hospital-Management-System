using HospitalManagementSystem.WPF.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagementSystem.Core.Interfaces;

namespace HospitalManagementSystem.WPF.Services
{
    
    public class DialogService : IWpfDialogService // IWpfDialogService is inherited from IDialogService
    {
        private readonly IServiceProvider _serviceProvider;
        public DialogService(IServiceProvider serviceProvider) // Inject IServiceProvider
        {
            _serviceProvider = serviceProvider;
        }
        public Task ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            return Task.CompletedTask;
        }

        public Task<bool> ShowConfirmation(string title, string message)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return Task.FromResult(result == MessageBoxResult.Yes);
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
                {
                    // e.g. TViewModel = AddNewStaffMemberViewModel
                    var vmType = viewModel.GetType();
                    var asm = vmType.Assembly;                           // your WPF assembly
                    var viewName = vmType.Name.Replace("ViewModel", "View");  // "AddNewStaffMemberView"
                                                                              // I see your views live under Views.Admin (and Views.Admin.StaffRegister for wizard pages)
                    string[] possibleNamespaces = new[]
                    {
                "HospitalManagementSystem.WPF.Views.Admin.StaffRegister",
                "HospitalManagementSystem.WPF.Views.Admin"
            };

            Type? viewType = null;
            foreach (var ns in possibleNamespaces)
            {
                viewType = asm.GetType($"{ns}.{viewName}");
                if (viewType != null) break;
            }

            if (viewType == null || !typeof(Window).IsAssignableFrom(viewType))
                throw new InvalidOperationException(
                    $"Could not resolve a Window for ViewModel '{vmType.FullName}'.");

            // Now resolve the window from DI
            var dialogWindow = (Window)_serviceProvider.GetRequiredService(viewType);
            dialogWindow.DataContext = viewModel;
            return dialogWindow.ShowDialog();
        }


        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}