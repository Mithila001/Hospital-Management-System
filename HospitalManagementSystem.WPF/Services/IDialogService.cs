using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services
{
    public interface IDialogService
    {
        Task ShowMessage(string title, string message);
        Task<bool> ShowConfirmation(string title, string message);
    }
}