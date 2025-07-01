using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Exceptions.Admin
{
    /// <summary>
    /// Exception thrown when an attempt is made to register a staff member
    /// with a username that already exists.
    /// </summary>
    public class DuplicateUsernameException : ApplicationServiceException
    {
        public DuplicateUsernameException()
            : base("The specified username is already taken. Please choose a different one.") { }

        public DuplicateUsernameException(string message)
            : base(message) { }

        public DuplicateUsernameException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
