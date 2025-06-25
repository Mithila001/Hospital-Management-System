using HospitalManagementSystem.Core.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DataAccess.Repositories.Admin
{
    public class StaffRegistrationRepository : IStaffRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffRegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StaffMember> AddAsync(StaffMember staffMember)
        {
            await _context.StaffMembers.AddAsync(staffMember);
            await _context.SaveChangesAsync();
            return staffMember;
        }

        public async Task<StaffMember> GetByIdAsync(int id)
        {
            // FindAsync is efficient for retrieving by primary key
            return await _context.StaffMembers.FindAsync(id);
        }

        public async Task UpdateAsync(StaffMember staffMember)
        {
            _context.StaffMembers.Update(staffMember);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var staffMemberToDelete = await _context.StaffMembers.FindAsync(id);
            if (staffMemberToDelete != null)
            {
                _context.StaffMembers.Remove(staffMemberToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
