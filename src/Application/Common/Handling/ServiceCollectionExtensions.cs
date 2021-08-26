using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Mockingjay.Common.Handling
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
        {
            Guard.NotNull(assembly, nameof(assembly));

            foreach (var type in assembly.GetExportedTypes().Where(tp => !tp.IsAbstract))
            {
                services.AddCommandHandler(type);
            }

            return services;
        }

        private static IServiceCollection AddCommandHandler(this IServiceCollection services, Type type)
        {
            foreach (var args in type
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => i.GetGenericArguments()))
            {
                var requestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(args);
                services.AddSingleton(requestHandlerType, type);
            }

            return services;
        }
    }
}
