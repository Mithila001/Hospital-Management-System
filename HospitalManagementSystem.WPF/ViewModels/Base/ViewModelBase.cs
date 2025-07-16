using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HospitalManagementSystem.WPF.ViewModels.Base
{
    // Added INotifyDataErrorInfo back to ViewModelBase
    // Updated ViewModelBase to inherit from ObservableObject to fix MVVMTK0019
    public abstract partial class ViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        // INotifyDataErrorInfo implementation (re-added/modified)
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Any(kvp => kvp.Value != null && kvp.Value.Any());

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Return all errors if propertyName is null or empty
                return _errors.SelectMany(kvp => kvp.Value).Distinct();
            }
            return _errors.TryGetValue(propertyName, out var errorsForProperty)
                ? errorsForProperty
                : Enumerable.Empty<string>();
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }
            if (!_errors[propertyName].Contains(errorMessage))
            {
                _errors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearAllErrors()
        {
            var propertiesWithErrors = _errors.Keys.ToList();
            _errors.Clear();
            foreach (var prop in propertiesWithErrors)
            {
                OnErrorsChanged(prop); // Notify for each cleared property
            }
            // Optional: OnErrorsChanged(null) could be used to signal a global error change,
            // but individual property notifications are usually sufficient for WPF's data binding.
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        // Example IsBusy flag (existing)
        // Made _isBusy readonly to fix IDE0044
        [ObservableProperty]
        private bool _isBusy;
    }
}