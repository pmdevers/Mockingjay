using Infrastructure.Handling;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using LiteDB;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Security;
using Mockingjay.Common.Storage;
using Mockingjay.Features;
using System;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHandling(this IServiceCollection services)
        {
            services.AddTransient<ICommandProcessor, CommandProcessor>();
            return services;
        }

        public static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            services.AddTransient(typeof(IEventStore<>), typeof(LiteDBEventstore<>));

            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mockingjay");
            Directory.CreateDirectory(fileName);

            fileName = Path.Combine(fileName, "Mockingjay.db");
            services.AddSingleton(new ConnectionString(fileName)
            {
                Password = "1234",
                Connection = ConnectionType.Shared,
            });

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IEndpointRepository, EndpointInformationRepository>();
            return services;
        }
    }
}
