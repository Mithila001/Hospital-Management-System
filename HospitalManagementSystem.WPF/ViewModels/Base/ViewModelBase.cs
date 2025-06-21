using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HospitalManagementSystem.WPF.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Implement IDataErrorInfo for basic validation feedback (crucial for portfolio)
        // You can make this abstract if all VMs will use it, or implement it here
        // For a more advanced setup, you'd use a separate validation service/library
        public string Error => null; // No class-level error

        public virtual string this[string columnName]
        {
            get
            {
                // This is a placeholder. Specific validation logic for each property
                // will be implemented in the derived ViewModels (e.g., StaffManagementViewModel).
                return string.Empty;
            }
        }
    }
}