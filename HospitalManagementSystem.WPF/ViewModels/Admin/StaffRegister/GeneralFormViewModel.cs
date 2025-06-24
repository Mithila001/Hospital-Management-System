using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class GeneralFormViewModel : ViewModelBase
    {
        public ObservableCollection<StaffRole> Roles { get; }
        StaffRole? _selectedRole;
        public StaffRole? SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        public GeneralFormViewModel()
        {
            Roles = new ObservableCollection<StaffRole>(
                Enum.GetValues(typeof(StaffRole)).Cast<StaffRole>());
        }
    }
}
