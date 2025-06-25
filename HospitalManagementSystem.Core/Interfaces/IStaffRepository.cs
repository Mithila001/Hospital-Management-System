using HospitalManagementSystem.Core.Models.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces
{
    public interface IStaffRepository
    {
        Task<StaffMember> GetByIdAsync(int id);
        Task<IEnumerable<StaffMember>> GetAllAsync();
        Task AddAsync(StaffMember staffMember);
        Task UpdateAsync(StaffMember staffMember);
        Task DeleteAsync(int id);
        Task<StaffMember> GetByUsernameAsync(string username); // Useful for login if you extend
    }
}