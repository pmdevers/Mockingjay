using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mockingjay;
using Mockingjay.Common.Json;
using MockingjayApp.Middleware;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MockingjayApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockingjay(this IServiceCollection services)
        {
            var assembly = typeof(Main).Assembly;
            foreach (var type in assembly.GetExportedTypes().Where(tp => !tp.IsAbstract))
            {
                services.AddForms(type);
            }

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.SetApplicationDefaultSettings();
            }).AddApplicationPart(typeof(Guard).Assembly);

            services.AddTransient<ExceptionHandlerMiddleware>();
            services.AddTransient<MockingjayMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseMockingjay(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseMiddleware<MockingjayMiddleware>();
            return app;
        }

        private static IServiceCollection AddForms(this IServiceCollection services, Type type)
        {
            if (type.IsAssignableTo(typeof(Form))) {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
