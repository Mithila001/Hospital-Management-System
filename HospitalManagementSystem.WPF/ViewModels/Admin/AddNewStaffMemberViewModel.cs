using CommunityToolkit.Mvvm.Input;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.Services.ErrorMappers;
using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq; // Added for .Any() and .All() in CanExecuteNext
using System.Threading.Tasks;
using System.Windows.Input; // Ensure ICommand is recognized

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public partial class AddNewStaffMemberViewModel : ViewModelBase
    {
        readonly IStaffRegistrationRepository _repo;
        readonly IDialogService _dialogService;
        readonly IExceptionMessageMapper _exceptionMessageMapper;
        readonly StaffRegistrationData_VDM _data;

        // Factories for creating the step ViewModels
        private readonly Func<StaffRegistrationData_VDM, GeneralFormViewModel> _generalFormViewModelFactory;
        private readonly Func<StaffRegistrationData_VDM, DoctorFormViewModel> _doctorFormViewModelFactory;
        private readonly Func<StaffRegistrationData_VDM, NurseFormViewModel> _nurseFormViewModelFactory;
        // ... and so on for other form view models

        ViewModelBase _roleVm, _finalVm;
        readonly List<ViewModelBase> _steps = new();
        bool _pipelineBuilt;
        int _currentIndex;

       

        public ViewModelBase CurrentStep => _steps[_currentIndex];
        public bool CanGoBack => _currentIndex > 0 && _currentIndex < (_steps.Count - 1);
        public string NextButtonText => _currentIndex switch
        {
            0 => "Next",
            1 => "Confirm",
            _ when _currentIndex == _steps.Count - 1 => "Finish", // Explicitly define "Finish" for the last step
            _ => "Next" // Default for intermediate steps if more are added
        };

        public AddNewStaffMemberViewModel(
            IStaffRegistrationRepository repo,
            IExceptionMessageMapper exceptionMessageMapper,
            IDialogService dialogService,
            Func<StaffRegistrationData_VDM, GeneralFormViewModel> generalFormViewModelFactory,
            Func<StaffRegistrationData_VDM, DoctorFormViewModel> doctorFormViewModelFactory,
            Func<StaffRegistrationData_VDM, NurseFormViewModel> nurseFormViewModelFactory)
        {
            _repo = repo;
            _dialogService = dialogService;
            _exceptionMessageMapper = exceptionMessageMapper;
            _data = new StaffRegistrationData_VDM(); // StaffRegistrationData_VDM created once per wizard
            _generalFormViewModelFactory = generalFormViewModelFactory;
            _doctorFormViewModelFactory = doctorFormViewModelFactory;
            _nurseFormViewModelFactory = nurseFormViewModelFactory;

            // Step 1: general form
            _steps.Add(_generalFormViewModelFactory(_data));

            

            NotifyAll();
        }


        public Action<bool?> RequestClose { get; set; } //An Action to request dialog closure with a result

        // NEW: [RelayCommand] for OnNext and OnBack methods
        // The attribute will automatically create public ICommand properties: NextCommand and BackCommand.
        // It will also automatically look for CanNext() and CanBack() methods for CanExecute logic.

        [RelayCommand(CanExecute = nameof(CanExecuteNext))] // CommunityToolkit will look for a method named CanExecuteNext()
        private void OnNext()
        {
            // --- Step 1: Handle validation for the current step BEFORE attempting to proceed ---
            if (CurrentStep is GeneralFormViewModel generalFormVm)
            {
                // Call the ViewModel's own validation method.
                // If it returns false, it means there are errors, so stop.
                if (!generalFormVm.ValidateAllProperties()) // Modified: Check return value of ValidateAllProperties()
                {
                    // The UI (red highlights, etc.) will show the errors thanks to INotifyDataErrorInfo
                    // implemented on GeneralFormViewModel (via ViewModelBase).
                    return;
                }
            }
            //// Added: Similar validation checks for other steps as they are developed
            //else if (CurrentStep is DoctorFormViewModel doctorFormVm)
            //{
            //    if (!doctorFormVm.ValidateAllProperties()) return;
            //}
            //else if (CurrentStep is NurseFormViewModel nurseFormVm)
            //{
            //    if (!nurseFormVm.ValidateAllProperties()) return;
            //}
            //// ... Add similar checks for other form ViewModels ...


            // --- Step 2: Proceed with the navigation logic ONLY IF validation passed ---
            if (_currentIndex == 0 && !_pipelineBuilt)
            {
                // Build role-specific step
                _roleVm = _data.SelectedRole switch
                {
                    // Ensure _data.SelectedRole is not null for this switch
                    HospitalManagementSystem.Core.Enums.StaffRole.Doctor => _doctorFormViewModelFactory(_data),
                    HospitalManagementSystem.Core.Enums.StaffRole.Nurse => _nurseFormViewModelFactory(_data),
                    // … add other roles here …
                    _ => throw new InvalidOperationException("Unknown role or role not selected") // Added check
                };
                _steps.Add(_roleVm);

                // Final step
                _finalVm = new FinalPageViewModel(); // Assuming you have this ViewModel
                _steps.Add(_finalVm);

                _pipelineBuilt = true;
                _currentIndex++; // Move to the next step
            }
            else if (_currentIndex == (_steps.Count - 2)) // Last data entry step before final summary/save
            {
                // This 'if' block handles the transition from the last *input* step
                // (e.g., DoctorForm/NurseForm) to the final summary/save step.
                // It ensures all previous steps are valid before attempting to save.

                // Re-validate ALL steps before attempting to save (optional, but good practice)
                foreach (var step in _steps.Take(_currentIndex + 1)) // Validate current and previous data entry steps
                {
                    if (step is GeneralFormViewModel generalVm && !generalVm.ValidateAllProperties()) return;
                    //if (step is DoctorFormViewModel doctorVm && !doctorVm.ValidateAllProperties()) return;
                    //if (step is NurseFormViewModel nurseVm && !nurseVm.ValidateAllProperties()) return;
                    // ... Add checks for other steps
                }

                _ = SaveAndAdvanceAsync();
            }
            // Handle the "Finish" button click to close the dialog
            else if (_currentIndex == (_steps.Count - 1)) // This is the final step
            {
                RequestClose?.Invoke(true); // Signal successful completion and close the dialog
            }
            // Default case: advance to the next step if not the first, not the last-before-save, and not the final step
            else if (_currentIndex < _steps.Count - 1)
            {
                _currentIndex++;
            }

            NotifyAll();
        }

        async Task SaveAndAdvanceAsync()
        {
            IsBusy = true;
            try
            {
                var creds = await _repo.RegisterAsync(_data);
                var final = (FinalPageViewModel)_finalVm;
                final.UserName = creds.UserName;
                final.Password = creds.Password;
                _currentIndex++;
            }
            catch (Exception ex)
            {
                string userFriendlyMessage = _exceptionMessageMapper.GetUserFriendlyMessage(ex);
                _dialogService.ShowError(userFriendlyMessage);
            }
            finally
            {
                IsBusy = false;
                NotifyAll();
            }
        }

        bool CanExecuteNext()
        {
            // First, check if the current step itself has any validation errors.
            // If it does, the 'Next' button should be disabled.
            if (CurrentStep is ViewModelBase currentValidationVm && currentValidationVm.HasErrors)
            {
                return false;
            }

            // Specific logic for the initial step (index 0) where role selection is crucial.
            // Even if no general errors are showing, a role must be selected to build the pipeline.
            if (_currentIndex == 0)
            {
                return _data.SelectedRole != default;
            }

            // For the step that triggers saving (typically the one before the final summary),
            // ensure we are not busy with a save operation.
            if (_currentIndex == (_steps.Count - 2)) // If this is the last data entry step before saving
            {
                return !IsBusy;
            }

            // Default: allow progression if no immediate errors on the current step and not busy.
            return true;
        }

        private bool CanBack()
        {
            return CanGoBack;
        }

        [RelayCommand(CanExecute = nameof(CanBack))]
        void OnBack()
        {
            if (_currentIndex > 0) _currentIndex--;
            NotifyAll();
        }

        void NotifyAll()
        {
            OnPropertyChanged(nameof(CurrentStep));
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(NextButtonText));

            // *** KEY CHANGE 3: Update RaiseCanExecuteChanged calls ***
            // Now that NextCommand and BackCommand are generated by [RelayCommand],
            // they expose a NotifyCanExecuteChanged() method.
            NextCommand.NotifyCanExecuteChanged(); //OnNext -> Next -> NextCommand (how the name is generated)
            BackCommand.NotifyCanExecuteChanged();

            // OPTIONAL IMPROVEMENT:
            // For CanGoBack property, if it directly affects BackCommand's enabled state,
            // and if CanGoBack was backed by a field instead of a calculated property,
            // you could use [ObservableProperty(CanExecute = "BackCommand")] on the private field.
            // However, since CanGoBack is a calculated property, manually calling NotifyCanExecuteChanged
            // is the correct approach here.
        }
    }
}