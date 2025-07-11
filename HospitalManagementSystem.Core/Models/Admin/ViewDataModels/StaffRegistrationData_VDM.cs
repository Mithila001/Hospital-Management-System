using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Models.Admin.ViewDataModels
{
    public class StaffRegistrationData_VDM : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        // MVVM‑friendly: we implement INotifyPropertyChanged here,
        // but Core only depends on System.ComponentModel, not WPF.
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // INotifyDataErrorInfo implementation
        private readonly Dictionary<string, List<string>> _errors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errors.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }
            return _errors[propertyName];
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Add an Error message for a specific property.
        /// </summary>
        public void AddError(string propertyName, string errorMessage)
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
        /// <summary>
        /// Clears all error messages for a specific property.
        /// </summary>
        public void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }
        /// <summary>
        /// Clears all errors for all properties.
        /// </summary>
        public void ClearAllErrors()
        {
            var propertiesWithErrors = _errors.Keys.ToList();
            _errors.Clear();
            foreach (var prop in propertiesWithErrors)
            {
                OnErrorsChanged(prop); // Notify for each cleared property
            }
            OnErrorsChanged(null); // Notify that overall errors might have changed
        }

        // ───────────────────────────────────────────────
        // 1) Common fields (from StaffMember)

        #region Staff Member 
        private StaffRole? _selectedRole;
        public StaffRole? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _passwordHash;
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                OnPropertyChanged(nameof(PasswordHash));
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                // Only update and validate if the value is actually different
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName)); // Notify that the property's value has changed

                    // --- VALIDATION LOGIC FOR FirstName ---
                    ClearErrors(nameof(FirstName)); // Clear previous errors for this property first

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        AddError(nameof(FirstName), "First Name is required.");
                    }
                    else if (value.Length < 2)
                    {
                       AddError(nameof(FirstName), "First Name must be at least 2 characters long.");
                    }
                    // --- END VALIDATION LOGIC ---
                }
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private DateTime? _dob;
        public DateTime? DOB
        {
            get => _dob;
            set
            {
                _dob = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string _nationality;
        public string Nationality
        {
            get => _nationality;
            set
            {
                _nationality = value;
                OnPropertyChanged(nameof(Nationality));
            }
        }

        private string _nationalIdNumber;
        public string NationalIdNumber
        {
            get => _nationalIdNumber;
            set
            {
                _nationalIdNumber = value;
                OnPropertyChanged(nameof(NationalIdNumber));
            }
        }

        private MaritalStatus _maritalStatus;
        public MaritalStatus MaritalStatus
        {
            get => _maritalStatus;
            set
            {
                _maritalStatus = value;
                OnPropertyChanged(nameof(MaritalStatus));
            }
        }

        private BloodGroup _bloodGroup;
        public BloodGroup BloodGroup
        {
            get => _bloodGroup;
            set
            {
                _bloodGroup = value;
                OnPropertyChanged(nameof(BloodGroup));
            }
        }

        private string _primaryPhone;
        public string PrimaryPhone
        {
            get => _primaryPhone;
            set
            {
                _primaryPhone = value;
                OnPropertyChanged(nameof(PrimaryPhone));
            }
        }

        private string _secondaryPhone;
        public string SecondaryPhone
        {
            get => _secondaryPhone;
            set
            {
                _secondaryPhone = value;
                OnPropertyChanged(nameof(SecondaryPhone));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _emergencyContactName;
        public string EmergencyContactName
        {
            get => _emergencyContactName;
            set
            {
                _emergencyContactName = value;
                OnPropertyChanged(nameof(EmergencyContactName));
            }
        }

        private string _emergencyContactRelationship;
        public string EmergencyContactRelationship
        {
            get => _emergencyContactRelationship;
            set
            {
                _emergencyContactRelationship = value;
                OnPropertyChanged(nameof(EmergencyContactRelationship));
            }
        }

        private string _emergencyContactPhone;
        public string EmergencyContactPhone
        {
            get => _emergencyContactPhone;
            set
            {
                _emergencyContactPhone = value;
                OnPropertyChanged(nameof(EmergencyContactPhone));
            }
        }

        private string _addressLine1;
        public string AddressLine1
        {
            get => _addressLine1;
            set
            {
                _addressLine1 = value;
                OnPropertyChanged(nameof(AddressLine1));
            }
        }

        private string _addressLine2;
        public string AddressLine2
        {
            get => _addressLine2;
            set
            {
                _addressLine2 = value;
                OnPropertyChanged(nameof(AddressLine2));
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _state;
        public string State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

        private string _employeeId;
        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }

        private DateTime? _dateOfHire;
        public DateTime? DateOfHire
        {
            get => _dateOfHire;
            set
            {
                _dateOfHire = value;
                OnPropertyChanged(nameof(DateOfHire));
            }
        }

        private string _department;
        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        private string _jobTitle;
        public string JobTitle
        {
            get => _jobTitle;
            set
            {
                _jobTitle = value;
                OnPropertyChanged(nameof(JobTitle));
            }
        }

        private EmploymentStatus _employmentStatus;
        public EmploymentStatus EmploymentStatus
        {
            get => _employmentStatus;
            set
            {
                _employmentStatus = value;
                OnPropertyChanged(nameof(EmploymentStatus));
            }
        }

        private string _reportingManager;
        public string ReportingManager
        {
            get => _reportingManager;
            set
            {
                _reportingManager = value;
                OnPropertyChanged(nameof(ReportingManager));
            }
        }

        private string _bankName;
        public string BankName
        {
            get => _bankName;
            set
            {
                _bankName = value;
                OnPropertyChanged(nameof(BankName));
            }
        }

        private string _bankAccountNumber;
        public string BankAccountNumber
        {
            get => _bankAccountNumber;
            set
            {
                _bankAccountNumber = value;
                OnPropertyChanged(nameof(BankAccountNumber));
            }
        }

        private string _bankSwiftCode;
        public string BankSwiftCode
        {
            get => _bankSwiftCode;
            set
            {
                _bankSwiftCode = value;
                OnPropertyChanged(nameof(BankSwiftCode));
            }
        }

        private string _bankAccountHolder;
        public string BankAccountHolder
        {
            get => _bankAccountHolder;
            set
            {
                _bankAccountHolder = value;
                OnPropertyChanged(nameof(BankAccountHolder));
            }
        }
        #endregion

        // ───────────────────────────────────────────────
        // 2) Doctor‑specific

        #region Doctor Specific Properties
        private string _medicalRegistrationNumber;
        public string MedicalRegistrationNumber
        {
            get => _medicalRegistrationNumber;
            set
            {
                _medicalRegistrationNumber = value;
                OnPropertyChanged(nameof(MedicalRegistrationNumber));
            }
        }

        private string _medicalCouncilName;
        public string MedicalCouncilName
        {
            get => _medicalCouncilName;
            set
            {
                _medicalCouncilName = value;
                OnPropertyChanged(nameof(MedicalCouncilName));
            }
        }

        private string _specializations;
        public string Specializations
        {
            get => _specializations;
            set
            {
                _specializations = value;
                OnPropertyChanged(nameof(Specializations));
            }
        }

        private string _subSpecializations;
        public string SubSpecializations
        {
            get => _subSpecializations;
            set
            {
                _subSpecializations = value;
                OnPropertyChanged(nameof(SubSpecializations));
            }
        }

        private int _yearsOfExperience_doc;
        public int YearsOfExperience_Doc
        {
            get => _yearsOfExperience_doc;
            set
            {
                _yearsOfExperience_doc = value;
                OnPropertyChanged(nameof(YearsOfExperience_Doc));
            }
        }

        private string _professionalMemberships;
        public string ProfessionalMemberships
        {
            get => _professionalMemberships;
            set
            {
                _professionalMemberships = value;
                OnPropertyChanged(nameof(ProfessionalMemberships));
            }
        }

        private DateTime? _licenseExpiryDate_Doc;
        public DateTime? LicenseExpiryDate_Doc
        {
            get => _licenseExpiryDate_Doc;
            set
            {
                _licenseExpiryDate_Doc = value;
                OnPropertyChanged(nameof(LicenseExpiryDate_Doc));
            }
        }

        private string _indemnityInsuranceDetails;
        public string IndemnityInsuranceDetails
        {
            get => _indemnityInsuranceDetails;
            set
            {
                _indemnityInsuranceDetails = value;
                OnPropertyChanged(nameof(IndemnityInsuranceDetails));
            }
        }

        private string _qualifications;
        public string Qualifications
        {
            get => _qualifications;
            set
            {
                _qualifications = value;
                OnPropertyChanged(nameof(Qualifications));
            }
        }

        private int? _yearOfGraduation;
        public int? YearOfGraduation
        {
            get => _yearOfGraduation;
            set
            {
                _yearOfGraduation = value;
                OnPropertyChanged(nameof(YearOfGraduation));
            }
        }

        private string _certifications_Doc;
        public string Certifications_Doc
        {
            get => _certifications_Doc;
            set
            {
                _certifications_Doc = value;
                OnPropertyChanged(nameof(Certifications_Doc));
            }
        }

        private string _consultationHours;
        public string ConsultationHours
        {
            get => _consultationHours;
            set
            {
                _consultationHours = value;
                OnPropertyChanged(nameof(ConsultationHours));
            }
        }

        private string _onCallPreferences;
        public string OnCallPreferences
        {
            get => _onCallPreferences;
            set
            {
                _onCallPreferences = value;
                OnPropertyChanged(nameof(OnCallPreferences));
            }
        }

        private int _totalLeaveEntitlement;
        public int TotalLeaveEntitlement
        {
            get => _totalLeaveEntitlement;
            set
            {
                _totalLeaveEntitlement = value;
                OnPropertyChanged(nameof(TotalLeaveEntitlement));
            }
        }

        private int _leaveTaken;
        public int LeaveTaken
        {
            get => _leaveTaken;
            set
            {
                _leaveTaken = value;
                OnPropertyChanged(nameof(LeaveTaken));
            }
        }

        private string _publicationsJson;
        public string PublicationsJson
        {
            get => _publicationsJson;
            set
            {
                _publicationsJson = value;
                OnPropertyChanged(nameof(PublicationsJson));
            }
        }
        #endregion

        // ───────────────────────────────────────────────
        // 3) Nurse‑specific
        #region Nurse Specific Properties
        private string _nursingRegistrationNumber;
        public string NursingRegistrationNumber
        {
            get => _nursingRegistrationNumber;
            set
            {
                _nursingRegistrationNumber = value;
                OnPropertyChanged(nameof(NursingRegistrationNumber));
            }
        }

        private string _nursingCouncilName;
        public string NursingCouncilName
        {
            get => _nursingCouncilName;
            set
            {
                _nursingCouncilName = value;
                OnPropertyChanged(nameof(NursingCouncilName));
            }
        }

        private DateTime? _licenseExpiryDate_Nurce;
        public DateTime? LicenseExpiryDate_Nurce
        {
            get => _licenseExpiryDate_Nurce;
            set
            {
                _licenseExpiryDate_Nurce = value;
                OnPropertyChanged(nameof(LicenseExpiryDate_Nurce));
            }
        }

        private string _specialization;
        public string Specialization
        {
            get => _specialization;
            set
            {
                _specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }

        private string _certifications_Nurce;
        public string Certifications_Nurce
        {
            get => _certifications_Nurce;
            set
            {
                _certifications_Nurce = value;
                OnPropertyChanged(nameof(Certifications_Nurce));
            }
        }

        private int _yearsOfExperience_nurce;
        public int YearsOfExperience_Nurce
        {
            get => _yearsOfExperience_nurce;
            set
            {
                _yearsOfExperience_nurce = value;
                OnPropertyChanged(nameof(YearsOfExperience_Nurce));
            }
        }

        private string _educationalQualifications;
        public string EducationalQualifications
        {
            get => _educationalQualifications;
            set
            {
                _educationalQualifications = value;
                OnPropertyChanged(nameof(EducationalQualifications));
            }
        }

        private string _clinicalSkills;
        public string ClinicalSkills
        {
            get => _clinicalSkills;
            set
            {
                _clinicalSkills = value;
                OnPropertyChanged(nameof(ClinicalSkills));
            }
        }

        private string _shiftPreferences;
        public string ShiftPreferences
        {
            get => _shiftPreferences;
            set
            {
                _shiftPreferences = value;
                OnPropertyChanged(nameof(ShiftPreferences));
            }
        }
        #endregion

        // ───────────────────────────────────────────────
        // 4) Receptionist, ServiceStaff, etc.
        // Just keep appending properties here as new roles appear.
    }
}
