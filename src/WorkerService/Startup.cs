using Infrastructure.Handling;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mockingjay;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Json;
using Mockingjay.Common.Repositories;
using Mockingjay.Common.Security;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;
using WorkerService.Middleware;

namespace WorkerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthorization();

            services.AddCommandHandlers(typeof(Guard).Assembly);
            services.AddTransient<ICommandProcessor, CommandProcessor>();
            services.AddTransient(typeof(IEventStore<>), typeof(InMemoryEventstore<>));
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<MockingjayMiddleware>();
            services.AddSingleton<IRepository<EndpointInformation>, EndpointInformationRepository>();

            // example for health checks
            services.AddHealthChecks();

            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.SetApplicationDefaultSettings();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<MockingjayMiddleware>();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("hello world");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
