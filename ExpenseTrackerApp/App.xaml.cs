using System;
using System.Windows;
using dotenv.net;
using ExpenseTrackerApp.Data;
using ExpenseTrackerApp.Services;
using ExpenseTrackerApp.ViewModels;
using ExpenseTrackerApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerApp
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public App()
        {
            DotEnv.Load(); 

            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();
            services.AddSingleton<CurrencyService>();
            services.AddSingleton<DatabaseService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<CryptoService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ExpenseFormViewModel>();
            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = Services.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }
    }
}