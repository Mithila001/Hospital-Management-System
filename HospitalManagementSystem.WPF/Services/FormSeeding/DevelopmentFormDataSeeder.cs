using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.Services.FormSeeding
{
    public class DevelopmentFormDataSeeder : IFormDataSeeder
    {
        private readonly IConfiguration _config;
        public DevelopmentFormDataSeeder(IConfiguration config)
            => _config = config;

        public void SeedGeneral(GeneralFormViewModel vm)
        {
            //// you can also read from _config.GetSection("FormDefaults:General"), if you like
            //vm.SelectedRole = vm.Roles.Count > 0 ? vm.Roles[0] : default;
            //vm.FirstName = "DevFirst";
            //vm.LastName = "DevLast";
            //vm.DOB = DateTime.Today.AddYears(-30);
            //vm.SelectedGender = vm.Genders.Count > 1 ? vm.Genders[1] : vm.Genders[0];
            //vm.ContactNo = "+94 77 1234 567";
            //vm.Email = "dev@hospital.local";
        }
    }
}
