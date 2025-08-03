using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionRegistrar;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }

            Application.Run(serviceProvider.GetRequiredService<MainForm>());
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.Registration();
            services.AddSingleton<GenresViewerFormFactory>();
            services.AddTransient<MainForm>();
        }
    }
}