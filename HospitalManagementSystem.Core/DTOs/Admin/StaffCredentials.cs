using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.DTOs.Admin
{
    public class StaffCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }  // plain‑text only for display
    }

}
