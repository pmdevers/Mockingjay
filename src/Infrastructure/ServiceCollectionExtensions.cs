﻿using Infrastructure.Handling;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Storage;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Repositories;
using Mockingjay.Common.Security;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;

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
            services.AddTransient(typeof(IEventStore<>), typeof(InMemoryEventstore<>));

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
            services.AddSingleton<IRepository<EndpointInformation>, EndpointInformationRepository>();
            return services;
        }
    }
}
