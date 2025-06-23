using HospitalManagementSystem.WPF.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem.WPF.Services
{
    public class DialogService : IDialogService
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

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
        {
            string viewTypeName = viewModel.GetType().Name.Replace("ViewModel", "View");
            string fullViewTypeName = $"HospitalManagementSystem.WPF.Views.Admin.{viewTypeName}";
            Type? viewType = Type.GetType(fullViewTypeName);

            if (viewType == null)
            {
                throw new InvalidOperationException($"Could not find view type '{fullViewTypeName}' for ViewModel '{viewModel.GetType().Name}'.");
            }

            if (!typeof(Window).IsAssignableFrom(viewType))
            {
                throw new InvalidOperationException($"View type '{viewType.Name}' must be a Window to be shown as a dialog.");
            }

            Window dialogWindow = (Window)_serviceProvider.GetRequiredService(viewType);
            dialogWindow.DataContext = viewModel;

            return dialogWindow.ShowDialog();
        }
    }
}