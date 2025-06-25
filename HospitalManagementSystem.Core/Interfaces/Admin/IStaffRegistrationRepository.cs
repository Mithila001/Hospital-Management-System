using HospitalManagementSystem.Core.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DataAccess.Repositories.Admin
{
    public interface IStaffRegistrationRepository
    {
        Task<StaffMember> AddAsync(StaffMember staffMember);
        Task<StaffMember> GetByIdAsync(int id);
        Task UpdateAsync(StaffMember staffMember);
        Task DeleteAsync(int id);
    }
}
