using FluentValidation;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Validation.Admin
{
    public class StaffRegistrationDataValidator : AbstractValidator<StaffRegistrationData_VDM>
    {
        public StaffRegistrationDataValidator()
        {
            // Rule for FirstName
            RuleFor(vdm => vdm.FirstName)
                .NotEmpty().WithMessage("First Name is requiredssss.")
                .Length(2, 50).WithMessage("First Name must be between 2 and 50 characters.");

            // Rule for MiddleName
            RuleFor(vdm => vdm.MiddleName)
                // Middle Name is optional, so we don't use NotEmpty()
                .MaximumLength(50).WithMessage("Middle Name cannot exceed 50 characters.")
                // You can add more rules if there are specific requirements, e.g.,
                // .Matches("^[a-zA-Z -]*$").WithMessage("Middle Name contains invalid characters.");
                ;
        }
    }
}
