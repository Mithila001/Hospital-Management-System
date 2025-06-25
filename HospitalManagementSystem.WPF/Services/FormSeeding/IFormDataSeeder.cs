using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.FormSeeding
{
    /// <summary>
    /// Seeds initial values into your form view‐models.
    /// In Dev: sets defaults. In Prod: no‐op.
    /// </summary>
    public interface IFormDataSeeder
    {
        void SeedGeneral(GeneralFormViewModel vm);
        // in future, you could add SeedDoctor, SeedNurse, etc.
    }
}

