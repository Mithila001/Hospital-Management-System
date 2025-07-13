using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.WPF.ViewModels.Admin;
using HospitalManagementSystem.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalManagementSystem.WPF.Services
{
    
    public class DialogService : IWpfDialogService // IWpfDialogService is inherited from IDialogService
    {
        // -------------------------------------------------------------------
        // SECTION 1: The Dependency Injection Integrator
        // This section focuses on how the service leverages the DI container
        // to get its own dependencies and to resolve views.
        // -------------------------------------------------------------------

        private readonly IServiceProvider _serviceProvider;
        public DialogService(IServiceProvider serviceProvider) // Inject IServiceProvider
        {
            _serviceProvider = serviceProvider;
        }

        // -------------------------------------------------------------------
        // SECTION 2: The UI Interaction Orchestrator (Basic Dialogs)
        // This section handles the display of standard message box dialogs
        // which are a direct interaction with the UI.
        // -------------------------------------------------------------------
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

        // -------------------------------------------------------------------
        // SECTION 3: The ViewModel-View Bridge (for Custom Modal Dialogs)
        // This is the core logic for dynamically resolving and displaying
        // custom WPF Window-based dialogs based on their ViewModel.
        // It also handles the ViewModel-driven closure mechanism.
        // -------------------------------------------------------------------


        public bool? ShowDialog<TViewModel>(TViewModel viewModel)
    where TViewModel : ViewModelBase
        {
            // -------------------------------------------------------------------
            // SUB-SECTION 3.1: View Resolution Strategy
            // This part focuses on identifying and locating the correct WPF View (Window)
            // that corresponds to the given ViewModel type, based on naming conventions
            // and predefined namespaces.
            // -------------------------------------------------------------------

            // 3.1.1: Derive View Name from ViewModel Name
            // e.g. TViewModel = AddNewStaffMemberViewModel -> viewName = "AddNewStaffMemberView"
            var vmType = viewModel.GetType();
            var asm = vmType.Assembly; // Get the assembly where the ViewModel resides (expected to be WPF project)
            var viewName = vmType.Name.Replace("ViewModel", "View"); // Convention: ViewModelName -> ViewName

            // 3.1.2: Define and Search Possible View Namespaces
            // I see your views live under Views.Admin (and Views.Admin.StaffRegister for wizard pages)
            string[] possibleNamespaces = new[]
            {
                "HospitalManagementSystem.WPF.Views.Admin.StaffRegister",
                "HospitalManagementSystem.WPF.Views.Admin"
                // Future Improvement: Consider a more robust mapping or convention system
                // for more dynamic or complex scenarios (e.g., if views are scattered).
            };

            Type? viewType = null;
            foreach (var ns in possibleNamespaces)
            {
                viewType = asm.GetType($"{ns}.{viewName}");
                if (viewType != null) break; // Found the view, stop searching
            }

            // 3.1.3: Validation and Error Handling for View Resolution
            // Crucial: If the corresponding View is not found or is not a WPF Window,
            // this ensures a clear error is thrown early.
            if (viewType == null || !typeof(Window).IsAssignableFrom(viewType))
                throw new InvalidOperationException(
                   $"Could not resolve a Window for ViewModel '{vmType.FullName}'. " +
                   $"Expected a Window type in '{string.Join(", ", possibleNamespaces)}' named '{viewName}'.");


            // -------------------------------------------------------------------
            // SUB-SECTION 3.2: Dialog Window Instantiation and Data Binding
            // This part focuses on creating the actual WPF Window instance using DI
            // and associating it with the provided ViewModel.
            // -------------------------------------------------------------------

            // 3.2.1: Instantiate the Window via Dependency Injection
            // Crucial: Resolving from DI ensures that the Window's own dependencies
            // (if any, e.g., other services injected into its constructor) are fulfilled.
            var dialogWindow = (Window)_serviceProvider.GetRequiredService(viewType);

            // 3.2.2: Assign ViewModel to DataContext
            // This is the core MVVM principle: the View binds to properties on this ViewModel.
            dialogWindow.DataContext = viewModel;


            // -------------------------------------------------------------------
            // SUB-SECTION 3.3: ViewModel-Driven Dialog Closure Mechanism
            // This part sets up the communication channel for the ViewModel to
            // request the closing of its associated dialog, along with a result.
            // -------------------------------------------------------------------

            // 3.3.1: Specific ViewModel Callback Hook (Current Implementation)
            // This current implementation is specific to 'AddNewStaffMemberViewModel'.
            // Future Improvement: For broader applicability, consider an interface
            // like IClosableViewModel in HospitalManagementSystem.Core.Interfaces
            // that ViewModelBase or relevant ViewModels could implement,
            // e.g., 'if (viewModel is IClosableViewModel closableVm) { closableVm.RequestClose = ... }'
            if (viewModel is AddNewStaffMemberViewModel addNewStaffVm)
            {
                // Assign the action that closes the dialog
                addNewStaffVm.RequestClose = (dialogResult) =>
                {
                    // Ensure the dialog window exists and is open before attempting to close
                    if (dialogWindow != null)
                    {
                        dialogWindow.DialogResult = dialogResult; // Set the result (true/false/null)
                        dialogWindow.Close(); // Close the window
                    }
                };
            }

            // -------------------------------------------------------------------
            // SUB-SECTION 3.4: Display Dialog
            // The final step: showing the configured WPF Window as a modal dialog.
            // -------------------------------------------------------------------

            return dialogWindow.ShowDialog();
        }

        // -------------------------------------------------------------------
        // SECTION 4: The Error Presentation Handler
        // This section specifically deals with displaying error messages.
        // -------------------------------------------------------------------

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}