using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.WPF.ViewDataModels.Admin;
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
        readonly StaffRegistration_VDM _staffData;
        readonly GeneralFormViewModel _generalVm;
        ViewModelBase _roleVm, _finalVm;
        readonly List<ViewModelBase> _steps = new();
        bool _pipelineBuilt;
        int _currentIndex;

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }

        public ViewModelBase CurrentStep => _steps[_currentIndex];
        public bool CanGoBack => _currentIndex > 0;
        public string NextButtonText
        {
            get
            {
                return _currentIndex switch
                {
                    0 => "Next",      // General → Role
                    1 => "Confirm",   // Role → Final
                    _ => string.Empty // Final page: no button
                };
            }
        }

        public AddNewStaffMemberViewModel()
        {
            _staffData = new StaffRegistration_VDM();
            _generalVm = new GeneralFormViewModel(_staffData);

            // start with only the general page
            _steps.Add(_generalVm);

            NextCommand = new RelayCommand(_ => OnNext(), _ => CanExecuteNext());
            BackCommand = new RelayCommand(_ => OnBack(), _ => CanGoBack);

            // prime the UI
            NotifyAll();
        }

        void OnNext()
        {
            if (_currentIndex == 0 && !_pipelineBuilt)
            {
                BuildPipeline();      // once, after role chosen
                _pipelineBuilt = true;
                _currentIndex = 1;    // advance to role form
            }
            else if (_currentIndex < _steps.Count - 1)
            {
                _currentIndex++;
            }

            NotifyAll();
        }

        void OnBack()
        {
            if (_currentIndex > 0)
                _currentIndex--;

            NotifyAll();
        }

        bool CanExecuteNext()
            => _currentIndex switch
            {
                0 => _staffData.SelectedRole.HasValue,
                1 => true,
                _ => false
            };

        void BuildPipeline()
        {
            // 1) role‐specific VM
            _roleVm = _staffData.SelectedRole.Value switch
            {
                StaffRole.Doctor => new DoctorFormViewModel(_staffData),
                //StaffRole.Nurse => new NurseFormViewModel(_staffData),
                //StaffRole.Admin => new AdminFormViewModel(_staffData),
                //StaffRole.Receptionist => new ReceptionistFormViewModel(_staffData),
                _ => new GeneralFormViewModel(_staffData)
            };
            _steps.Add(_roleVm);

            // 2) final summary page
            _finalVm = new FinalPageViewModel();
            _steps.Add(_finalVm);
        }

        void NotifyAll()
        {
            OnPropertyChanged(nameof(CurrentStep));
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(NextButtonText));
        }
    }


}
