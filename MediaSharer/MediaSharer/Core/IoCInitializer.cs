using System;
using Microsoft.Extensions.DependencyInjection;
using MediaSharer.Views;

namespace MediaSharer.Core
{
    public class IoCInitializer
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Repositories
            // services.AddSingleton<IMyRepository, MyRepository>();

            // Services
            // services.AddSingleton(typeof(MyService));

            // ViewModels
            services.AddSingleton(typeof(DashboardPageViewModel));

            return services.BuildServiceProvider();
        }
    }
}