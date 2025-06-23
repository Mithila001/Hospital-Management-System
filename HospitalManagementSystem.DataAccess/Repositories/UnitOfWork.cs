using HospitalManagementSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // IUnitOfWork Implementation
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
