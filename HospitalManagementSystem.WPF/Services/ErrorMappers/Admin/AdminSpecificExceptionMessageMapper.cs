using HospitalManagementSystem.Core.Exceptions.Admin;
using HospitalManagementSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.ErrorMappers.Admin
{
    /// <summary>
    /// Handles mapping for exceptions specific to the Admin module's business rules and operations.
    /// </summary>
    public class AdminSpecificExceptionMessageMapper : IErrorToMessageMapper
    {
        public string GetMessage(Exception ex)
        {
            // Direct mapping for custom Admin exceptions
            if (ex is DuplicateUsernameException duplicateEx)
            {
                return duplicateEx.Message; // Use the message already defined in the exception
            }
            if (ex is InvalidDepartmentException invalidDeptEx)
            {
                return invalidDeptEx.Message;
            }
            if (ex is AdminOperationFailedException adminFailedEx)
            {
                return adminFailedEx.Message;
            }
            // Add more specific Admin-related exception mappings here if needed

            return null; // This mapper doesn't handle this exception type
        }
    }
}
