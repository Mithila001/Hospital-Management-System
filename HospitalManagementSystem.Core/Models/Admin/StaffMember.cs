using HospitalManagementSystem.Core.Enums.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Models.Admin
{
    public class StaffMember
    {
        public int Id { get; set; }

        // Personal Identification
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }      // large list → keep string
        public string NationalIdNumber { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public BloodGroup BloodGroup { get; set; }

        // Contact Information
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string Email { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactPhone { get; set; }

        // Address Information
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // Employment Details
        public string EmployeeId { get; set; }
        public DateTime? DateOfHire { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public string ReportingManager { get; set; }

        // Bank Details
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankSwiftCode { get; set; }
        public string BankAccountHolder { get; set; }
    }
}
