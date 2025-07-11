using FluentValidation;
using FluentValidation.Results;
using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
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
        
        readonly StaffRegistrationData_VDM _data;
        private readonly IValidator<StaffRegistrationData_VDM> _validator; // FluentValidation validator
        public StaffRegistrationData_VDM StaffData => _data;

        public ObservableCollection<StaffRole> Roles { get; }
        public ObservableCollection<Gender> Genders { get; }
        public ObservableCollection<MaritalStatus> MaritalStatuses { get; }
        public ObservableCollection<BloodGroup> BloodGroups { get; }
        public ObservableCollection<EmploymentStatus> EmploymentStatuses { get; }

        public GeneralFormViewModel(
            StaffRegistrationData_VDM data, 
            IValidator<StaffRegistrationData_VDM> validator) : base()
        {
            _data = data;
            _validator = validator; // Assign the injected validator

            // Initial validation when the ViewModel is created/loaded
            ValidateAllProperties();

            Roles = new ObservableCollection<StaffRole>(
                Enum.GetValues(typeof(StaffRole)).Cast<StaffRole>());

            Genders = new ObservableCollection<Gender>(
                Enum.GetValues(typeof(Gender)).Cast<Gender>());
            MaritalStatuses = new ObservableCollection<MaritalStatus>(
                Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>());
            BloodGroups = new ObservableCollection<BloodGroup>(
                Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>());
            EmploymentStatuses = new ObservableCollection<EmploymentStatus>(
                Enum.GetValues(typeof(EmploymentStatus)).Cast<EmploymentStatus>());


            // --- Development Pre-population ---

            #region Auto Fill Form Info
            // Personal Information
            FirstName = "Aisha";
            MiddleName = "Priya";
            LastName = "Perera";
            DOB = new DateTime(1988, 7, 21); // Born in 1988
            Gender = Gender.Female;
            Nationality = "Sri Lankan";
            NationalIdNumber = "887213456V"; // Old format for demo
            MaritalStatus = MaritalStatus.Married;
            BloodGroup = BloodGroup.A_Positive;

            // Contact Information
            PrimaryPhone = "0771234567";
            SecondaryPhone = "0112345678";
            Email = "aisha.perera@hospital.lk";
            EmergencyContactName = "Kamal Perera";
            EmergencyContactRelationship = "Husband";
            EmergencyContactPhone = "0719876543";

            // Address Information (Realistic for Colombo, Sri Lanka)
            AddressLine1 = "No. 123, Galle Road";
            AddressLine2 = "Kollupitiya";
            City = "Colombo";
            State = "Western Province"; // Or "Western"
            PostalCode = "00300";
            Country = "Sri Lanka";

            // Employment Details
            SelectedRole = StaffRole.Doctor; // Crucial for the next step (DoctorFormViewModel)
            EmployeeId = "EMP00123";
            DateOfHire = new DateTime(2015, 3, 10);
            Department = "Cardiology";
            JobTitle = "Consultant Cardiologist";
            EmploymentStatus = EmploymentStatus.FullTime;
            ReportingManager = "Dr. Silva";

            // Bank Details
            BankName = "Bank of Ceylon";
            BankAccountNumber = "123456789012";
            BankSwiftCode = "BCEYLKLX";
            BankAccountHolder = "Aisha Priya Perera"; 
            #endregion

            // --- End Development Pre-population ---
        }


        // helper method to validate a single property
        private void ValidateProperty(string propertyName)
        {
            // Clear existing errors for this property on the VDM
            _data.ClearErrors(propertyName);

            // Create a ValidationContext for the specific property
            var validationContext = ValidationContext<StaffRegistrationData_VDM>.CreateWithOptions(_data, options => options.IncludeProperties(propertyName));

            // Perform validation using FluentValidation
            ValidationResult result = _validator.Validate(validationContext);

            // Add new errors to the VDM
            foreach (var error in result.Errors)
            {
                _data.AddError(error.PropertyName, error.ErrorMessage);
            }
        }

        // method to validate all properties (e.g., when moving to the next step or saving)
        public void ValidateAllProperties()
        {
            // Clear all existing errors on the VDM before re-validating everything
            _data.ClearAllErrors();

            // Perform full validation using FluentValidation
            ValidationResult result = _validator.Validate(_data);

            // Add all errors to the VDM
            foreach (var error in result.Errors)
            {
                _data.AddError(error.PropertyName, error.ErrorMessage);
            }

            
        }

        // Personal Identification
        public string FirstName
        {
            get => _data.FirstName;
            set
            {
                // Use SetProperty to check if value changed and notify PropertyChanged
                if (_data.FirstName != value)
                {
                    _data.FirstName = value;
                    OnPropertyChanged(); // Notify that FirstName property changed on the ViewModel
                    ValidateProperty(nameof(FirstName)); // Trigger validation for FirstName
                }
            }
        }

        public string MiddleName
        {
            get => _data.MiddleName;
            set { _data.MiddleName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _data.LastName;
            set { _data.LastName = value; OnPropertyChanged(); }
        }

        public DateTime? DOB
        {
            get => _data.DOB;
            set { _data.DOB = value; OnPropertyChanged(); }
        }

        public Gender Gender
        {
            get => _data.Gender;
            set { _data.Gender = value; OnPropertyChanged(); }
        }

        public string Nationality
        {
            get => _data.Nationality;
            set { _data.Nationality = value; OnPropertyChanged(); }
        }

        public string NationalIdNumber
        {
            get => _data.NationalIdNumber;
            set { _data.NationalIdNumber = value; OnPropertyChanged(); }
        }

        public MaritalStatus MaritalStatus
        {
            get => _data.MaritalStatus;
            set { _data.MaritalStatus = value; OnPropertyChanged(); }
        }

        public BloodGroup BloodGroup
        {
            get => _data.BloodGroup;
            set { _data.BloodGroup = value; OnPropertyChanged(); }
        }

        // Contact Information
        public string PrimaryPhone
        {
            get => _data.PrimaryPhone;
            set { _data.PrimaryPhone = value; OnPropertyChanged(); }
        }

        public string SecondaryPhone
        {
            get => _data.SecondaryPhone;
            set { _data.SecondaryPhone = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _data.Email;
            set { _data.Email = value; OnPropertyChanged(); }
        }

        public string EmergencyContactName
        {
            get => _data.EmergencyContactName;
            set { _data.EmergencyContactName = value; OnPropertyChanged(); }
        }

        public string EmergencyContactRelationship
        {
            get => _data.EmergencyContactRelationship;
            set { _data.EmergencyContactRelationship = value; OnPropertyChanged(); }
        }

        public string EmergencyContactPhone
        {
            get => _data.EmergencyContactPhone;
            set { _data.EmergencyContactPhone = value; OnPropertyChanged(); }
        }

        // Address Information
        public string AddressLine1
        {
            get => _data.AddressLine1;
            set { _data.AddressLine1 = value; OnPropertyChanged(); }
        }

        public string AddressLine2
        {
            get => _data.AddressLine2;
            set { _data.AddressLine2 = value; OnPropertyChanged(); }
        }

        public string City
        {
            get => _data.City;
            set { _data.City = value; OnPropertyChanged(); }
        }

        public string State
        {
            get => _data.State;
            set { _data.State = value; OnPropertyChanged(); }
        }

        public string PostalCode
        {
            get => _data.PostalCode;
            set { _data.PostalCode = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => _data.Country;
            set { _data.Country = value; OnPropertyChanged(); }
        }

        // Employment Details
        public StaffRole? SelectedRole
        {
            get => _data.SelectedRole;
            set { _data.SelectedRole = value; OnPropertyChanged(); }
        }

        public string EmployeeId
        {
            get => _data.EmployeeId;
            set { _data.EmployeeId = value; OnPropertyChanged(); }
        }

        public DateTime? DateOfHire
        {
            get => _data.DateOfHire;
            set { _data.DateOfHire = value; OnPropertyChanged(); }
        }

        public string Department
        {
            get => _data.Department;
            set { _data.Department = value; OnPropertyChanged(); }
        }

        public string JobTitle
        {
            get => _data.JobTitle;
            set { _data.JobTitle = value; OnPropertyChanged(); }
        }

        public EmploymentStatus EmploymentStatus
        {
            get => _data.EmploymentStatus;
            set { _data.EmploymentStatus = value; OnPropertyChanged(); }
        }

        public string ReportingManager
        {
            get => _data.ReportingManager;
            set { _data.ReportingManager = value; OnPropertyChanged(); }
        }

        // Bank Details
        public string BankName
        {
            get => _data.BankName;
            set { _data.BankName = value; OnPropertyChanged(); }
        }

        public string BankAccountNumber
        {
            get => _data.BankAccountNumber;
            set { _data.BankAccountNumber = value; OnPropertyChanged(); }
        }

        public string BankSwiftCode
        {
            get => _data.BankSwiftCode;
            set { _data.BankSwiftCode = value; OnPropertyChanged(); }
        }

        public string BankAccountHolder
        {
            get => _data.BankAccountHolder;
            set { _data.BankAccountHolder = value; OnPropertyChanged(); }
        }

        // Medical Professional Specific Fields
        public string MedicalRegistrationNumber
        {
            get => _data.MedicalRegistrationNumber;
            set { _data.MedicalRegistrationNumber = value; OnPropertyChanged(); }
        }

        public string MedicalCouncilName
        {
            get => _data.MedicalCouncilName;
            set { _data.MedicalCouncilName = value; OnPropertyChanged(); }
        }

        public string Specializations
        {
            get => _data.Specializations;
            set { _data.Specializations = value; OnPropertyChanged(); }
        }
    }
}
