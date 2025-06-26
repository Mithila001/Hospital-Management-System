using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.WPF.ViewDataModels.Admin;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class GeneralFormViewModel : ViewModelBase
    {
        readonly StaffRegistration_VDM _staffVDM;
        // Property to hold the StaffMember data
        private StaffMember _staffMember;

        // Properties for Enums used in the StaffMember model
        public ObservableCollection<Gender> Genders { get; }
        public ObservableCollection<MaritalStatus> MaritalStatuses { get; }
        public ObservableCollection<BloodGroup> BloodGroups { get; }
        public ObservableCollection<EmploymentStatus> EmploymentStatuses { get; }

        public GeneralFormViewModel(StaffRegistration_VDM staffData)
        {
            _staffVDM = staffData;

            Roles = new ObservableCollection<StaffRole>(
                Enum.GetValues(typeof(StaffRole)).Cast<StaffRole>());

            // Populate ObservableCollections for other enums
            Genders = new ObservableCollection<Gender>(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            MaritalStatuses = new ObservableCollection<MaritalStatus>(Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>());
            BloodGroups = new ObservableCollection<BloodGroup>(Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>());
            EmploymentStatuses = new ObservableCollection<EmploymentStatus>(Enum.GetValues(typeof(EmploymentStatus)).Cast<EmploymentStatus>());
        }
        public StaffMember StaffMember
        {
            get => _staffMember;
            set
            {
                _staffMember = value;
                OnPropertyChanged(nameof(StaffMember));
            }
        }
        // Property to hold the selected role from the StaffRole enum
        public ObservableCollection<StaffRole> Roles { get; }
        public StaffRole? SelectedRole
        {
            get => _staffVDM.SelectedRole;
            set
            {
                if (_staffVDM.SelectedRole != value)
                {
                    _staffVDM.SelectedRole = value;
                    OnPropertyChanged();                  // notify combo
                    CommandManager.InvalidateRequerySuggested(); // refresh NextCommand
                }
            }
        }


        // --- Personal Details ---
        public string FirstName
        {
            get => _staffVDM.Personal.FirstName;
            set { _staffVDM.Personal.FirstName = value; OnPropertyChanged(); }
        }
        public string MiddleName
        {
            get => _staffVDM.Personal.MiddleName;
            set { _staffVDM.Personal.MiddleName = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get => _staffVDM.Personal.LastName;
            set { _staffVDM.Personal.LastName = value; OnPropertyChanged(); }
        }
        public DateTime? DOB
        {
            get => _staffVDM.Personal.DOB;
            set { _staffVDM.Personal.DOB = value; OnPropertyChanged(); }
        }
        public Gender? Gender
        {
            get => _staffVDM.Personal.Gender;
            set { _staffVDM.Personal.Gender = value; OnPropertyChanged(); }
        }
        public string Nationality
        {
            get => _staffVDM.Personal.Nationality;
            set { _staffVDM.Personal.Nationality = value; OnPropertyChanged(); }
        }
        public string NationalIdNumber
        {
            get => _staffVDM.Personal.NationalIdNumber;
            set { _staffVDM.Personal.NationalIdNumber = value; OnPropertyChanged(); }
        }
        public MaritalStatus? MaritalStatus
        {
            get => _staffVDM.Personal.MaritalStatus;
            set { _staffVDM.Personal.MaritalStatus = value; OnPropertyChanged(); }
        }
        public BloodGroup? BloodGroup
        {
            get => _staffVDM.Personal.BloodGroup;
            set { _staffVDM.Personal.BloodGroup = value; OnPropertyChanged(); }
        }

        // --- Personal Details ---
        public string PrimaryPhone
        {
            get => _staffVDM.Personal.PrimaryPhone;
            set { _staffVDM.Personal.PrimaryPhone = value; OnPropertyChanged(); }
        }
        public string SecondaryPhone
        {
            get => _staffVDM.Personal.SecondaryPhone;
            set { _staffVDM.Personal.SecondaryPhone = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get => _staffVDM.Personal.Email;
            set { _staffVDM.Personal.Email = value; OnPropertyChanged(); }
        }
        public string EmergencyContactName
        {
            get => _staffVDM.Personal.EmergencyContactName;
            set { _staffVDM.Personal.EmergencyContactName = value; OnPropertyChanged(); }
        }
        public string EmergencyContactRelationship
        {
            get => _staffVDM.Personal.EmergencyContactRelationship;
            set { _staffVDM.Personal.EmergencyContactRelationship = value; OnPropertyChanged(); }
        }
        public string EmergencyContactPhone
        {
            get => _staffVDM.Personal.EmergencyContactPhone;
            set { _staffVDM.Personal.EmergencyContactPhone = value; OnPropertyChanged(); }
        }

        // --- Address Details ---
        public string AddressLine1
        {
            get => _staffVDM.Personal.AddressLine1;
            set { _staffVDM.Personal.AddressLine1 = value; OnPropertyChanged(); }
        }
        public string AddressLine2
        {
            get => _staffVDM.Personal.AddressLine2;
            set { _staffVDM.Personal.AddressLine2 = value; OnPropertyChanged(); }
        }
        public string City
        {
            get => _staffVDM.Personal.City;
            set { _staffVDM.Personal.City = value; OnPropertyChanged(); }
        }
        public string State
        {
            get => _staffVDM.Personal.State;
            set { _staffVDM.Personal.State = value; OnPropertyChanged(); }
        }
        public string PostalCode
        {
            get => _staffVDM.Personal.PostalCode;
            set { _staffVDM.Personal.PostalCode = value; OnPropertyChanged(); }
        }
        public string Country
        {
            get => _staffVDM.Personal.Country;
            set { _staffVDM.Personal.Country = value; OnPropertyChanged(); }
        }

        // --- Employment Details ---
        public string EmployeeId
        {
            get => _staffVDM.Personal.EmployeeId;
            set { _staffVDM.Personal.EmployeeId = value; OnPropertyChanged(); }
        }
        public DateTime? DateOfHire
        {
            get => _staffVDM.Personal.DateOfHire;
            set { _staffVDM.Personal.DateOfHire = value; OnPropertyChanged(); }
        }
        public string Department
        {
            get => _staffVDM.Personal.Department;
            set { _staffVDM.Personal.Department = value; OnPropertyChanged(); }
        }
        public string JobTitle
        {
            get => _staffVDM.Personal.JobTitle;
            set { _staffVDM.Personal.JobTitle = value; OnPropertyChanged(); }
        }
        public EmploymentStatus? EmploymentStatus
        {
            get => _staffVDM.Personal.EmploymentStatus;
            set { _staffVDM.Personal.EmploymentStatus = value; OnPropertyChanged(); }
        }
        public string ReportingManager
        {
            get => _staffVDM.Personal.ReportingManager;
            set { _staffVDM.Personal.ReportingManager = value; OnPropertyChanged(); }
        }

        // --- Bank Details ---
        public string BankName
        {
            get => _staffVDM.Personal.BankName;
            set { _staffVDM.Personal.BankName = value; OnPropertyChanged(); }
        }
        public string BankAccountNumber
        {
            get => _staffVDM.Personal.BankAccountNumber;
            set { _staffVDM.Personal.BankAccountNumber = value; OnPropertyChanged(); }
        }
        public string BankSwiftCode
        {
            get => _staffVDM.Personal.BankSwiftCode;
            set { _staffVDM.Personal.BankSwiftCode = value; OnPropertyChanged(); }
        }
        public string BankAccountHolder
        {
            get => _staffVDM.Personal.BankAccountHolder;
            set { _staffVDM.Personal.BankAccountHolder = value; OnPropertyChanged(); }
        }

    }
}
