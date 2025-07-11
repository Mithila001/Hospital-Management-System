using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin
{
    public class AddNewStaffMemberViewModel : ViewModelBase
    {
        readonly IStaffRegistrationRepository _repo;
        readonly IDialogService _dialogService;
        readonly IExceptionMessageMapper _exceptionMessageMapper;
        readonly StaffRegistrationData_VDM _data;

        // Factories for creating the step ViewModels
        // You'll need one for each type of step ViewModel (GeneralForm, DoctorForm, etc.)
        private readonly Func<StaffRegistrationData_VDM, GeneralFormViewModel> _generalFormViewModelFactory;
        private readonly Func<StaffRegistrationData_VDM, DoctorFormViewModel> _doctorFormViewModelFactory;
        private readonly Func<StaffRegistrationData_VDM, NurseFormViewModel> _nurseFormViewModelFactory;
        // ... and so on for other form view models

        ViewModelBase _roleVm, _finalVm;
        readonly List<ViewModelBase> _steps = new();
        bool _pipelineBuilt;
        int _currentIndex;

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }

        public ViewModelBase CurrentStep => _steps[_currentIndex];
        public bool CanGoBack => _currentIndex > 0;
        public string NextButtonText => _currentIndex switch
        {
            0 => "Next",
            1 => "Confirm",
            _ => string.Empty
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
            _data = new StaffRegistrationData_VDM();
            _generalFormViewModelFactory = generalFormViewModelFactory;
            _doctorFormViewModelFactory = doctorFormViewModelFactory;
            _nurseFormViewModelFactory = nurseFormViewModelFactory;

            // Step 1: general form
            _steps.Add(_generalFormViewModelFactory(_data));

            NextCommand = new RelayCommand(_ => OnNext(), _ => CanExecuteNext());
            BackCommand = new RelayCommand(_ => OnBack(), _ => CanGoBack);

            NotifyAll();
            
        }

        void OnNext()
        {
            // --- Step 1: Handle validation for the current step BEFORE attempting to proceed ---
            if (CurrentStep is GeneralFormViewModel generalFormVm)
            {
                generalFormVm.ValidateAllProperties(); // Trigger full validation on the current step
                if (generalFormVm.StaffData.HasErrors)
                {
                    // If there are errors, stop here. Do NOT advance the step.
                    // The UI (red highlights, etc.) will show the errors thanks to INotifyDataErrorInfo.
                    return;
                }
            }
            // Add similar validation checks for other steps as they are developed
            // else if (CurrentStep is DoctorFormViewModel doctorFormVm)
            // {
            //     doctorFormVm.ValidateAllProperties();
            //     if (doctorFormVm.StaffData.HasErrors) return; // Assuming DoctorFormViewModel also has a StaffData property
            // }

            // --- Step 2: Proceed with the navigation logic ONLY IF validation passed ---
            if (_currentIndex == 0 && !_pipelineBuilt)
            {
                // Build role‐specific step
                _roleVm = _data.SelectedRole switch
                {
                    StaffRole.Doctor => _doctorFormViewModelFactory(_data),
                    StaffRole.Nurse => _nurseFormViewModelFactory(_data),
                    // … add other roles here …
                    _ => throw new InvalidOperationException("Unknown role")
                };
                _steps.Add(_roleVm);

                // Final step
                _finalVm = new FinalPageViewModel();
                _steps.Add(_finalVm);

                _pipelineBuilt = true;
                _currentIndex = 1;
            }
            else if (_currentIndex == 1)
            {
                _ = SaveAndAdvanceAsync();
            }
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
            // For the General Form step (index 0), the 'Next' button should always be clickable,
            // as validation will occur OnNext().
            if (_currentIndex == 0)
            {
                // We still check SelectedRole, as that's a prerequisite to building the next steps.
                // If SelectedRole is not selected, the pipeline can't even be built.
                return _data.SelectedRole != default;
            }

            // For the second step (index 1), CanExecuteNext remains tied to IsBusy for saving.
            return _currentIndex switch
            {
                1 => !IsBusy, // Allow saving if not busy
                _ => true // Default to true for other steps (if any) if no specific CanExecute logic
            };
        }

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


        }
    }


}
