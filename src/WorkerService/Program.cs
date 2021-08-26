using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WorkerService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = ".NET Joke Service";
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5050");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
