using HospitalManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.ErrorMappers
{
    /// <summary>
    /// Orchestrates the process of mapping exceptions to user-friendly messages
    /// by delegating to a collection of specialized IErrorToMessageMappers.
    /// Also responsible for comprehensive logging of the raw exception.
    /// </summary>
    public class CompositeExceptionMessageMapper : IExceptionMessageMapper
    {
        private readonly IEnumerable<IErrorToMessageMapper> _mappers;
        private readonly ILogger<CompositeExceptionMessageMapper> _logger; // Logger for technical details

        public CompositeExceptionMessageMapper(
            IEnumerable<IErrorToMessageMapper> mappers,
            ILogger<CompositeExceptionMessageMapper> logger)
        {
            _mappers = mappers;
            _logger = logger;
        }

        public string GetUserFriendlyMessage(Exception ex)
        {
            // --- CRITICAL: Log the FULL exception details for debugging ---
            // This is the single, central point where every exception that hits the UI layer is logged.
            _logger.LogError(ex, "An exception occurred while processing a user operation. Details below:");

            // --- Attempt to get a user-friendly message from specialized mappers ---
            // Order of mappers: More specific ones first.
            // For example, Admin-specific before database, before general core.
            // You can refine this ordering if needed using a more complex sort or direct enumeration.
            foreach (var mapper in _mappers.OrderByDescending(m => GetMapperPriority(m)))
            {
                string message = mapper.GetMessage(ex);
                if (message != null)
                {
                    return message; // Found a specific message
                }
            }

            // --- Fallback for unhandled exceptions ---
            // If no specific mapper handles the exception.
            string userFriendlyMessage = "An unexpected error occurred. Please try again. If the the problem persists, contact support.";

            // Optionally, generate a support ID for traceability
            // string supportId = Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant();
            // _logger.LogError($"Generated support ID {supportId} for unhandled error.");
            // userFriendlyMessage += $" (Reference: {supportId})";

            return userFriendlyMessage;
        }

        /// <summary>
        /// Defines an arbitrary priority for mappers to influence their processing order.
        /// Higher priority mappers are processed first.
        /// </summary>
        private int GetMapperPriority(IErrorToMessageMapper mapper)
        {
            // Process Admin-specific mappers first, as they contain domain-specific knowledge
            if (mapper is ErrorMappers.Admin.AdminSpecificExceptionMessageMapper) return 3;
            // Then database errors (they are quite specific but less than domain errors)
            if (mapper is ErrorMappers.DatabaseExceptionMessageMapper) return 2;
            // Common/generic mappers last
            if (mapper is ErrorMappers.Common.CoreExceptionMessageMapper) return 1;

            return 0; // Default low priority
        }
    }
}
