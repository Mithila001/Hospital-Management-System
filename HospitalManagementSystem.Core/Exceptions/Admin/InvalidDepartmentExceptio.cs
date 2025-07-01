using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Exceptions.Admin
{
    /// <summary>
    /// Exception thrown when an attempt is made to assign a staff member
    /// to an invalid or non-existent department.
    /// </summary>
    public class InvalidDepartmentException : ApplicationServiceException
    {
        public InvalidDepartmentException()
            : base("The selected department is invalid or does not exist.") { }

        public InvalidDepartmentException(string message)
            : base(message) { }

        public InvalidDepartmentException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
