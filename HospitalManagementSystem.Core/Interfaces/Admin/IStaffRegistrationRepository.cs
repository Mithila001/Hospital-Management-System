using HospitalManagementSystem.Core.DTOs.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces.Admin
{
    public interface IStaffRegistrationRepository
    {
        /// <summary>
        /// Persists the new staff member (and role‑specific record),
        /// generates and stores credentials, then returns the plain credentials.
        /// </summary>
        Task<StaffCredentials> RegisterAsync(StaffRegistrationData_VDM data);

        // <summary>
        /// Returns all staff members from the database.
        /// </summary>
        Task<List<StaffMember>> GetAllAsync();
    }
}
