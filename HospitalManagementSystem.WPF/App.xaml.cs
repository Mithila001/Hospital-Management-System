using FluentValidation;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.Core.Validation.Admin;
using HospitalManagementSystem.DataAccess;
using HospitalManagementSystem.DataAccess.Repositories;
using HospitalManagementSystem.DataAccess.Repositories.Admin;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.Services.ErrorMappers;
using HospitalManagementSystem.WPF.Services.ErrorMappers.Admin;
using HospitalManagementSystem.WPF.Services.ErrorMappers.Common;
using HospitalManagementSystem.WPF.ViewModels;
using HospitalManagementSystem.WPF.ViewModels.Admin;
using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using HospitalManagementSystem.WPF.Views;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows;

namespace HospitalManagementSystem.WPF
{
    public partial class App : Application
    {
        // Public property for DI container
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureServices();

            // Apply migrations
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            var mainWindow = ServiceProvider.GetRequiredService<AdminDashboardView>();
            //var mainWindow = ServiceProvider.GetRequiredService<AddNewStaffMemberView>();
            mainWindow.Show();
        }

        private void ConfigureServices()
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);

            // --- ADD LOGGING CONFIGURATION  ---
            services.AddLogging(builder =>
            {
                // This line adds logging to the console (or debug output in VS)
                // You can configure different logging providers here (e.g., File, Debug, Console)
                builder.AddDebug(); // Logs to the Debug Output window in Visual Studio
                // builder.AddConsole(); // Logs to console if your application has one
                // You can also set a minimum logging level for the application or specific categories
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);

            // Repositories & Services
            services.AddTransient<IStaffRegistrationRepository, StaffRegistrationRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>(); // <- this might be incorrect; 

            // --- Dialog Services ---
            services.AddSingleton<IWpfDialogService, DialogService>();
            services.AddSingleton<IDialogService>(provider => 
                        provider.GetRequiredService<IWpfDialogService>()); // tell to use above WPFService when ask for IDialogService

            // --- Error Handling Mappers ---
            services.AddTransient<IErrorToMessageMapper, CoreExceptionMessageMapper>();
            services.AddTransient<IErrorToMessageMapper, DatabaseExceptionMessageMapper>();
            services.AddTransient<IErrorToMessageMapper, AdminSpecificExceptionMessageMapper>();
            // Register the composite mapper, which takes all IErrorToMessageMappers
            services.AddTransient<IExceptionMessageMapper, CompositeExceptionMessageMapper>();

            // --- Validators ---
            services.AddTransient<IValidator<StaffRegistrationData_VDM>, StaffRegistrationDataValidator>();

            // ViewModels
            services.AddTransient<HomeViewModel>();
            services.AddTransient<StaffManagementViewModel>();
            services.AddTransient<AddNewStaffMemberViewModel>();
            services.AddTransient<GeneralFormViewModel>();
            services.AddTransient<DoctorFormViewModel>();
            services.AddTransient<ViewStaffMemberInfoViewModel>();


            services.AddTransient<Func<AddNewStaffMemberViewModel>>(
                provider => () => provider.GetRequiredService<AddNewStaffMemberViewModel>());

            services.AddTransient<Func<HospitalManagementSystem.Core.Models.Admin.StaffMember, ViewStaffMemberInfoViewModel>>(
                provider => (staffMember) => new ViewStaffMemberInfoViewModel(staffMember));

            services.AddTransient<AdminDashboardViewModel>(sp =>
            {
                var homeVm = sp.GetRequiredService<HomeViewModel>();
                var staffVm = sp.GetRequiredService<StaffManagementViewModel>();
                return new AdminDashboardViewModel(homeVm, staffVm);
            });

            // Factories
            services.AddTransient<Func<StaffRegistrationData_VDM, GeneralFormViewModel>>(sp =>
            {
                return (dataVdm) => new GeneralFormViewModel(dataVdm, sp.GetRequiredService<IValidator<StaffRegistrationData_VDM>>());
            });
            services.AddTransient<Func<StaffRegistrationData_VDM, DoctorFormViewModel>>(sp =>
            {
                // Assuming DoctorFormViewModel has a constructor: DoctorFormViewModel(StaffRegistrationData_VDM data, IValidator<DoctorSpecificData_VDM> validator)
                // For now, let's assume it only takes StaffRegistrationData_VDM if no specific validator for Doctor/Nurse is set up yet.
                // If it *does* take a validator, you'd register IValidator<DoctorSpecificData_VDM> and inject it here.
                return (dataVdm) => new DoctorFormViewModel(dataVdm /*, sp.GetRequiredService<IValidator<DoctorSpecificData_VDM>>() */);
            });
            services.AddTransient<Func<StaffRegistrationData_VDM, NurseFormViewModel>>(sp =>
            {
                return (dataVdm) => new NurseFormViewModel(dataVdm /*, sp.GetRequiredService<IValidator<NurseSpecificData_VDM>>() */);
            });


            // View Data Models
            services.AddTransient<StaffRegistrationData_VDM>();

            // Views
            services.AddTransient<AdminDashboardView>();
            services.AddTransient<HomeView>();
            services.AddTransient<ViewStaffMemberInfoView>();
            //services.AddTransient<AddNewStaffMemberView>();

            services.AddTransient<AddNewStaffMemberView>(sp =>
            {
                var view = new AddNewStaffMemberView();
                view.DataContext = sp.GetRequiredService<AddNewStaffMemberViewModel>();
                return view;
            });

            //services.AddSingleton<MainWindow>();

            // Finalize DI container
            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            (ServiceProvider as IDisposable)?.Dispose();
            base.OnExit(e);
        }
    }
}
