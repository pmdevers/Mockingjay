using FluentValidation;
using Mockingjay.Common.Behaviours;
using Mockingjay.Common.Handling;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            services.AddCommandHandlers(typeof(ServiceCollectionExtensions).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            
            return services;
        }
    }
}
