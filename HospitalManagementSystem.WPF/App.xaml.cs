using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.DataAccess;
using HospitalManagementSystem.DataAccess.Repositories;
using HospitalManagementSystem.DataAccess.Repositories.Admin;
using HospitalManagementSystem.Core.Interfaces.Admin;
using HospitalManagementSystem.WPF.Services;
using HospitalManagementSystem.WPF.ViewModels;
using HospitalManagementSystem.WPF.ViewModels.Admin;
using HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister;
using HospitalManagementSystem.WPF.Views;
using HospitalManagementSystem.WPF.Views.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            //var mainWindow = ServiceProvider.GetRequiredService<AdminDashboardView>();
            var mainWindow = ServiceProvider.GetRequiredService<AddNewStaffMemberView>();
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

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);

            // Repositories & Services
            services.AddTransient<IUnitOfWork, UnitOfWork>(); // <- this might be incorrect; 
            services.AddSingleton<IWpfDialogService, DialogService>();
            services.AddSingleton<IDialogService>(provider => 
                        provider.GetRequiredService<IWpfDialogService>());
            services.AddTransient<IStaffRegistrationRepository, StaffRegistrationRepository>();

            // ViewModels
            services.AddTransient<HomeViewModel>();
            services.AddTransient<StaffManagementViewModel>();
            services.AddTransient<AddNewStaffMemberViewModel>();
            services.AddTransient<AddNewStaffMemberView>(sp =>
            {
                var view = new AddNewStaffMemberView();
                view.DataContext = sp.GetRequiredService<AddNewStaffMemberViewModel>();
                return view;
            });

            services.AddTransient<Func<AddNewStaffMemberViewModel>>(
                provider => () => provider.GetRequiredService<AddNewStaffMemberViewModel>());

            services.AddTransient<AdminDashboardViewModel>(sp =>
            {
                var homeVm = sp.GetRequiredService<HomeViewModel>();
                var staffVm = sp.GetRequiredService<StaffManagementViewModel>();
                return new AdminDashboardViewModel(homeVm, staffVm);
            });

            // Views
            services.AddTransient<AdminDashboardView>();
            services.AddTransient<HomeView>();
            //services.AddTransient<AddNewStaffMemberView>();

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
