using Infrastructure.Handling;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using LiteDB;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Security;
using Mockingjay.Common.Storage;
using Mockingjay.Features;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHandling();
            services.AddRepositories();
            services.AddSecurity();
            services.Storage();

            return services;
        }

        public static IServiceCollection AddHandling(this IServiceCollection services)
        {
            services.AddTransient<ICommandProcessor, CommandProcessor>();
            return services;
        }

        public static IServiceCollection Storage(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IEventStore<>), typeof(LiteDBEventstore<>));
            services.AddSingleton(x =>
            {
                Directory.CreateDirectory(EndpointDatafile.Directory);
                return new ConnectionString(EndpointDatafile.FullPath)
                {
                    Password = "1234",
                    Connection = ConnectionType.Shared,
                };
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
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            return services;
        }
    }
}
