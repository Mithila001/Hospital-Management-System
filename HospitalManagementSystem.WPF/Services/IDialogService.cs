using HospitalManagementSystem.WPF.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalManagementSystem.WPF.Services
{
    public interface IDialogService
    {
        Task ShowMessage(string title, string message);
        Task<bool> ShowConfirmation(string title, string message);
        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}