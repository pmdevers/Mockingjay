using Mockingjay.Common.Handling;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandlers(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
