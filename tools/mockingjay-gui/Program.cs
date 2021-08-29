using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mockingjay.Dialogs;
using Mockingjay;
using Mockingjay.Common.Http;
using Mockingjay.Common.Json;
using System;
using System.Text.Json;
using System.Windows.Forms;

namespace mockingjay
{
    static class Program
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

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(x => x.AddConsole());
            services.AddHttpClient<MockingjayClient>(x => x.BaseAddress = new Uri("http://localhost:5050/api"));
            services.AddOptions<JsonSerializerOptions>().Configure(x => x.SetApplicationDefaultSettings());
            services.AddScoped<Main>();
            services.AddScoped<AddEndpointDialog>();
            
        }
    }
}
