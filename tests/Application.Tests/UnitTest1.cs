using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Application.Common.Handling;
using Application.Common.Messaging;
using Application.Common.Storage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

using NUnit.Framework;

using ICommand = System.Windows.Input.ICommand;

namespace Mockingjay.Tests
{
   [SetUpFixture]
    public class Testing
    {
        public static IConfigurationRoot Configuration;
        private static IServiceScopeFactory _scopeFactory;
        public static TestNotifier Notifier = new TestNotifier();

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
            
            services.AddSingleton(Mock.Of<IHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "WebApi"));

            ReplaceTransient(services, c => Mock.Of<IMessagePublisher>());
            ReplaceSingleton<IEventStore<DossierId>>(services, x => new InMemoryEventStore<DossierId>());
            
            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetService<IRepository<TEntity>>();

            await repository.SaveAsync(entity);
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
            where TResponse : class
        {
            using var scope = _scopeFactory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<ICommandProcessor>();

            return await processor.SendAsync<TCommand, TResponse>(request);
        }

        public static Task ResetStateAsync()
        {
            return Task.CompletedTask;
        }

        private static void ReplaceTransient<TService>(ServiceCollection services, Func<IServiceProvider, TService> initialize)
            where TService : class
        {
            var oldService = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));
            services.Remove(oldService);
            services.AddTransient(initialize);
        }

        private static void ReplaceSingleton<TService>(ServiceCollection services, Func<IServiceProvider, TService> initialize)
            where TService : class
        {
            var oldService = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));
            services.Remove(oldService);
            services.AddSingleton(initialize);
        }
    }
}
