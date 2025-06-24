using HospitalManagementSystem.Core.Enums;
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
        ViewModelBase _currentStep;
        public ViewModelBase CurrentStep
        {
            get => _currentStep;
            private set { _currentStep = value; OnPropertyChanged(); }
        }

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }

        public bool CanGoBack => !(CurrentStep is GeneralFormViewModel);
        public string NextButtonText =>
            CurrentStep is GeneralFormViewModel ? "Next" : "Register";

        public AddNewStaffMemberViewModel()
        {
            CurrentStep = new GeneralFormViewModel();
            NextCommand = new RelayCommand(
            _ => OnNext(),           // Action<object>
            _ => CanExecuteNext()    // Predicate<object>
            );
            BackCommand = new RelayCommand(
            _ => OnBack(),
            _ => CanGoBack
            );
        }

        void OnNext()
        {
            if (CurrentStep is GeneralFormViewModel step1)
            {
                // Map the selected role to the appropriate second-step VM
                switch (step1.SelectedRole)
                {
                    case StaffRole.Admin:
                        CurrentStep = new AdminFormViewModel();
                        break;
                    case StaffRole.Receptionist:
                        CurrentStep = new ReceptionistFormViewModel();
                        break;
                    case StaffRole.Doctor:
                        CurrentStep = new DoctorFormViewModel();
                        break;
                    case StaffRole.Nurse:
                        CurrentStep = new NurseFormViewModel();
                        break;
                    default:
                        CurrentStep = new OtherStaffFormViewModel();
                        break;
                }
                // raise CanGoBack + NextButtonText changes
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(NextButtonText));
            }
            else
            {
                // final “Register” logic:
                // collect data from the CurrentStep VM,
                // call your IStaffRepository/UnitOfWork, etc.
            }
        }

        bool CanExecuteNext()
        {
            if (CurrentStep is GeneralFormViewModel s1)
                return s1.SelectedRole.HasValue;
            // for step2, always enabled (or add your own validation)
            return true;
        }

        void OnBack()
        {
            // simply go back to Step 1
            CurrentStep = new GeneralFormViewModel();
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(NextButtonText));
        }

    }
}
