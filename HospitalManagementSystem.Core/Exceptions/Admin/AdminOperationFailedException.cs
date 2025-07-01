using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Exceptions.Admin
{
    /// <summary>
    /// Generic exception for failures within the Admin module's operations
    /// where a more specific exception is not appropriate or available.
    /// </summary>
    public class AdminOperationFailedException : ApplicationServiceException
    {
        public AdminOperationFailedException()
            : base("An administrative operation failed due to an unexpected issue. Please try again.") { }

        public AdminOperationFailedException(string message)
            : base(message) { }

        public AdminOperationFailedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
