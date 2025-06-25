using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Enums.Admin
{
    public enum MaritalStatus
    {
        Single,
        Married,
        Divorced,
        Widowed,
        Other
    }

    public enum EmploymentStatus
    {
        FullTime,
        PartTime,
        Contract,
        Temporary,
        Intern,
        Volunteer
    }

    public enum BloodGroup
    {
        A_Positive,
        A_Negative,
        B_Positive,
        B_Negative,
        AB_Positive,
        AB_Negative,
        O_Positive,
        O_Negative,
        Unknown
    }

    public enum Gender
    {
        Male,
        Female
    }
}
