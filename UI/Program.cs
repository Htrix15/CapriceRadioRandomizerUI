using Microsoft.Extensions.DependencyInjection;
using RadioServices;

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

            Application.Run(serviceProvider.GetRequiredService<MainForm>());
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.RegistrationServices();
            services.AddSingleton<GenresViewerFormFactory>();
            services.AddTransient<MainForm>();
        }
    }
}