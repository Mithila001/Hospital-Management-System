// HospitalManagementSystem.WPF\ViewModels\Base\ViewModelBase.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HospitalManagementSystem.WPF.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged // No longer implements INotifyDataErrorInfo
    {
        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Convenience SetProperty
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        // Example IsBusy flag (remains as a general ViewModel state)
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        // Removed all INotifyDataErrorInfo related code
        // private readonly INotifyDataErrorInfo _errorsSource;
        // private class EmptyErrorsSource : INotifyDataErrorInfo { ... }
        // public bool HasErrors => _errorsSource.HasErrors;
        // public IEnumerable GetErrors(string propertyName) => _errorsSource.GetErrors(propertyName);
        // public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        // Removed the constructors that took INotifyDataErrorInfo errorsSource
    }
}