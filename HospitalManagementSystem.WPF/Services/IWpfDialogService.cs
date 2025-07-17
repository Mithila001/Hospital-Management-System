using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services
{
    /// <summary>
    /// Interface that provides WPF-specific dialog services for the application.
    /// Extends the core dialog service with WPF-specific functionality.
    /// </summary>
    public interface IWpfDialogService : IDialogService // Inherit from Core's IDialogService
    {
        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}
