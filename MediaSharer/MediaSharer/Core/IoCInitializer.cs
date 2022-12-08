using System;
using Microsoft.Extensions.DependencyInjection;
using MediaSharer.Views;
using MediaSharer.Repositories.Interfaces;
using MediaSharer.Repositories.Implementations;

namespace MediaSharer.Core
{
    public class IoCInitializer
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Repositories
            // services.AddSingleton<IMyRepository, MyRepository>();
            services.AddSingleton<ISettingsRepository, SettingsRepository>();

            // Services
            // services.AddSingleton(typeof(MyService));

            // ViewModels
            services.AddSingleton(typeof(DashboardPageViewModel));
            services.AddSingleton(typeof(SettingsPageViewModel));

            return services.BuildServiceProvider();
        }
    }
}