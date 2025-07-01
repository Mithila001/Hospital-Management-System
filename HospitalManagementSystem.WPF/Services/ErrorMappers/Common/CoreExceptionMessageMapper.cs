using HospitalManagementSystem.Core.Exceptions;
using HospitalManagementSystem.Core.Exceptions.Admin;
using HospitalManagementSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.ErrorMappers.Common
{
    /// <summary>
    /// Handles mapping for common, application-wide exceptions (e.g., null arguments, cancellations, base application errors).
    /// </summary>
    public class CoreExceptionMessageMapper : IErrorToMessageMapper
    {
        public string GetMessage(Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                return "The operation was cancelled.";
            }
            if (ex is ArgumentNullException argNullEx)
            {
                // General message for a missing required value
                return $"A required field or value is missing. Please ensure all necessary information is provided.";
            }
            // If it's our generic base exception, use its message directly (assuming it's user-friendly)
            if (ex is ApplicationServiceException appServiceEx && !(appServiceEx is AdminOperationFailedException)) // Prevent double-handling specific ones
            {
                return appServiceEx.Message;
            }
            // Add more general, framework-level exceptions here if needed
            // e.g., if you had custom NetworkException in Core
            // if (ex is Core.Exceptions.NetworkException) return "Network connection lost. Please check your internet.";

            return null; // This mapper doesn't handle this specific exception type
        }
    }
}
