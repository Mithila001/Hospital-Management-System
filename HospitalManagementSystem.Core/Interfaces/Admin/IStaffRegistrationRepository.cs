using System.Threading.Tasks;
using HospitalManagementSystem.Core.DTOs.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;

namespace HospitalManagementSystem.Core.Interfaces.Admin
{
    public interface IStaffRegistrationRepository
    {
        /// <summary>
        /// Persists the new staff member (and role‑specific record),
        /// generates and stores credentials, then returns the plain credentials.
        /// </summary>
        Task<StaffCredentials> RegisterAsync(StaffRegistrationData_VDM data);
    }
}
