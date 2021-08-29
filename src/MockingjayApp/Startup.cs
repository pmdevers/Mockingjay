using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mockingjay.Common.Handling;
using Mockingjay.Features.GetEndpoints;

namespace MockingjayApp
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

            services.AddScoped<GetEndpointsController>();
            services.AddAuthorization();

            services.AddCommandHandlers();
            services.AddHandling();
            services.AddEventStore();
            services.AddSecurity();
            services.AddRepositories();

            // example for health checks
            services.AddHealthChecks();

            services.AddSwaggerGen();
            services.AddControllers();
            services.AddMockingjay();
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


            app.UseMockingjay();

        }
    }
}
