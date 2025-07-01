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
            IDialogService dialogService)
        {
            _repo = repo;
            _dialogService = dialogService;
            _exceptionMessageMapper = exceptionMessageMapper;
            _data = new StaffRegistrationData_VDM();

            // Step 1: general form
            _steps.Add(new GeneralFormViewModel(_data));

            NextCommand = new RelayCommand(_ => OnNext(), _ => CanExecuteNext());
            BackCommand = new RelayCommand(_ => OnBack(), _ => CanGoBack);

            NotifyAll();
        }

        void OnNext()
        {
            if (_currentIndex == 0 && !_pipelineBuilt)
            {
                // Build role‐specific step
                _roleVm = _data.SelectedRole switch
                {
                    StaffRole.Doctor => new DoctorFormViewModel(_data),
                    StaffRole.Nurse => new NurseFormViewModel(_data),
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

        bool CanExecuteNext() => _currentIndex switch
        {
            0 => _data.SelectedRole != default,
            1 => !IsBusy,
            _ => false
        };

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
