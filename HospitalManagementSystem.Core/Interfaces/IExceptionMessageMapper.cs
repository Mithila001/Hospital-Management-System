using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces
{
    /// <summary>
    /// The primary interface for ViewModels and other presentation-layer components
    /// to request user-friendly error messages. Delegates to a chain of specialized mappers.
    /// </summary>
    public interface IExceptionMessageMapper
    {
        /// <summary>
        /// Retrieves a user-friendly message for the given exception,
        /// using a chain of specialized mappers and logging the full details.
        /// </summary>
        /// <param name="ex">The exception to map.</param>
        /// <returns>A user-friendly error message string.</returns>
        string GetUserFriendlyMessage(Exception ex);
    }
}
