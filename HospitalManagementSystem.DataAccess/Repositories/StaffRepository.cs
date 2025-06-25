using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Models.Admin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DataAccess.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StaffMember> GetByIdAsync(int id)
        {
            return await _context.StaffMembers.FindAsync(id);
        }

        public async Task<IEnumerable<StaffMember>> GetAllAsync()
        {
            return await _context.StaffMembers.ToListAsync();
        }

        public async Task AddAsync(StaffMember staffMember)
        {
            await _context.StaffMembers.AddAsync(staffMember);
        }

        public async Task UpdateAsync(StaffMember staffMember)
        {
            _context.StaffMembers.Update(staffMember);
            // Entity Framework Core tracks changes, so Update is enough for tracked entities.
            // For disconnected entities, you might need _context.Entry(staffMember).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                _context.StaffMembers.Remove(staffMember);
            }
        }

        public async Task<StaffMember> GetByUsernameAsync(string username)
        {
            return null;
        }


    }
}