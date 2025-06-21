using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.DataAccess;
using HospitalManagementSystem.DataAccess.Repositories;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels;
using HospitalManagementSystem.WPF.ViewModels.Admin;
using HospitalManagementSystem.WPF.Views;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System; // Make sure System namespace is included for IServiceProvider
using System.IO;
using System.Windows;

namespace HospitalManagementSystem.WPF
{
    public partial class App : Application
    {
        // Change from private field to public property
        public IServiceProvider ServiceProvider { get; private set; } 

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureServices();

            // Ensure database is created and migrations applied
            using (var scope = ServiceProvider.CreateScope()) // Use the public property
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate(); 
                // Optional: Add seed data if needed
                // SeedData.Seed(dbContext);
            }

            var mainWindow = ServiceProvider.GetRequiredService<AdminDashboardView>(); // Use the public property
            mainWindow.Show();
        }

        private void ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient 
            );

            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<IUnitOfWork, StaffRepository>(); 

            services.AddSingleton<IDialogService, DialogService>();

            // Register Views
            services.AddTransient<StaffManagementView>();
            services.AddTransient<AdminDashboardView>();

            // Register ViewModels
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<StaffManagementViewModel>(); 

            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider(); // Assign to the public property
        }

        protected override void OnExit(ExitEventArgs e)
        {
            (ServiceProvider as IDisposable)?.Dispose(); // Use the public property
            base.OnExit(e);
        }
    }
}