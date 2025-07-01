using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Exceptions
{
    /// <summary>
    /// Base class for all custom application-specific exceptions.
    /// Provides a consistent way to handle expected business logic errors.
    /// </summary>
    public class ApplicationServiceException : Exception
    {
        public ApplicationServiceException() { }

        public ApplicationServiceException(string message) : base(message) { }

        public ApplicationServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
