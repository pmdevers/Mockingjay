using Mockingjay;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ReplaceTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> initialize)
            where TService : class
        {
            Guard.NotNull(services, nameof(services));
            var oldService = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));
            services.Remove(oldService);
            services.AddTransient(initialize);
        }

        public static void ReplaceSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> initialize)
            where TService : class
        {
            Guard.NotNull(services, nameof(services));
            var oldService = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));
            services.Remove(oldService);
            services.AddSingleton(initialize);
        }
    }
}
