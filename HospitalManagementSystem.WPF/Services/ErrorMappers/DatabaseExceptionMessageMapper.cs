using HospitalManagementSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.ErrorMappers
{
    /// <summary>
    /// Handles mapping for generic database-related exceptions, primarily DbUpdateException
    /// that weren't already re-thrown as more specific business exceptions by the repository.
    /// </summary>
    public class DatabaseExceptionMessageMapper : IErrorToMessageMapper
    {
        public string GetMessage(Exception ex)
        {
            // If the repository has already translated DbUpdateException to a custom business exception,
            // then those will be caught by AdminSpecificExceptionMessageMapper or CoreExceptionMessageMapper.
            // This mapper catches any *remaining* DbUpdateExceptions that are purely generic.
            if (ex is DbUpdateException dbEx)
            {
                // Unpack inner exceptions to find the root cause, if not already handled by a specific exception.
                Exception inner = dbEx.InnerException;
                while (inner?.InnerException != null)
                {
                    inner = inner.InnerException;
                }

                // You might still put some generic checks here if you have DbUpdateExceptions
                // that aren't specific enough to warrant a custom Core exception,
                // but are still DB-related.
                // For instance, a very general integrity constraint violation that isn't
                // a duplicate key or FK violation on a known field.
                if (inner != null && (inner.Message.Contains("constraint", StringComparison.OrdinalIgnoreCase) ||
                                       inner.Message.Contains("integrity", StringComparison.OrdinalIgnoreCase)))
                {
                    return "There was an issue with data integrity. Some related information might be missing or incorrect.";
                }

                return "A general database error occurred. Please try again. If the problem persists, contact support.";
            }
            return null; // This mapper doesn't handle this exception type
        }
    }
}
