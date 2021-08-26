using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkerService.Middleware;

namespace WorkerService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockingjay(this IServiceCollection services)
        {
            services.AddTransient<MockingjayMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseMockingjay(this IApplicationBuilder app)
        {
            app.UseMiddleware<MockingjayMiddleware>();
            return app;
        }
    }
}
