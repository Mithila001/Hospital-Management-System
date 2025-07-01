using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for services that can map a technical exception
    /// to a user-friendly message for a specific category of errors.
    /// </summary>
    public interface IErrorToMessageMapper
    {
        /// <summary>
        /// Attempts to translate a technical exception into a user-friendly message.
        /// </summary>
        /// <param name="ex">The exception to translate.</param>
        /// <returns>A user-friendly message if handled by this mapper; otherwise, null.</returns>
        string GetMessage(Exception ex);
    }
}
