using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mockingjay.Common.DomainModel;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;
using Mockingjay.Features;
using Mockingjay.Tests.Infrastructure;
using Mockingjay.Tests.Infrastructure.Storage;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Tests
{
    [SetUpFixture]
    public class Testing
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static InMemoryStorage<EndpointId> EventStore { get; } = new InMemoryStorage<EndpointId>();
        public static InMemoryEndpointRepository Repository { get; } = new InMemoryEndpointRepository();
        private static IServiceScopeFactory _scopeFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddApplication();
            services.AddInfrastructure();
            services.AddLogging();

            services.AddSingleton(Mock.Of<IHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "WebApi"));

            services.ReplaceSingleton<IEndpointRepository>(x => Repository);
            services.ReplaceSingleton<IEventStore<EndpointId>>(x => EventStore);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        public static async Task AddAsync(EndpointInformation entity)
        {
            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetService<IEndpointRepository>();

            await repository.SaveAsync(entity);
        }

        public static async Task AddEventAsync(EndpointId endpointId, object @event)
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetService<IEventStore<EndpointId>>();

            var buffer = new EventBuffer<EndpointId>(endpointId);
            buffer.Add(new[] { @event });
            await store.SaveAsync(buffer);
        }

        public static async Task SendAsync<TCommand>(TCommand request)
            where TCommand : class, ICommand
        {
            using var scope = _scopeFactory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<ICommandProcessor>();

            await processor.SendAsync(request);
        }

        public static async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request)
            where TCommand : class, ICommand<TResponse>
        {
            using var scope = _scopeFactory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<ICommandProcessor>();

            return await processor.SendAsync<TCommand, TResponse>(request);
        }

        public static Task ResetStateAsync()
        {
            Repository.Endpoints.Clear();
            EventStore.Events.Clear();
            return Task.CompletedTask;
        }
    }
}
