using FluentValidation;
using Mockingjay.Common.Behaviours;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            services.AddCommandHandlers(typeof(ServiceCollectionExtensions).Assembly);

            services.AddSingleton<IRouteMatcher, RouteMatcher>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            return services;
        }
    }
}
