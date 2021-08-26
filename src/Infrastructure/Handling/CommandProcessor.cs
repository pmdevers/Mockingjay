using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Mockingjay;
using Mockingjay.Common.Handling;

namespace Infrastructure.Handling
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _provider;

        public CommandProcessor(IServiceProvider provider)
        {
            _provider = Guard.NotNull(provider, nameof(provider));
        }

        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand<Unit>
        {
            await SendAsync<TCommand, Unit>(command, cancellationToken);
        }

        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand<TResult>
        {
            Guard.NotNull(command, nameof(command));
            using var scope = _provider.CreateScope();

            var handler = GetCommandHandler<IRequestHandler<TCommand, TResult>>(scope);

            if (handler is null)
            {
                throw new InvalidOperationException($"Could not resolve the handler of the type ICommandHandler<{typeof(TCommand)}>.");
            }

            var pipeline = GetPipeline(scope, command, cancellationToken, () => handler.HandleAsync(command, cancellationToken));

            return await pipeline;
        }

        private static THandler GetCommandHandler<THandler>(IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<THandler>();
        }

        private static Task<TResult> GetPipeline<TCommand, TResult>(IServiceScope scope, TCommand command, CancellationToken cancellationToken, Func<Task<TResult>> handler)
        {
            var behaviors = scope.ServiceProvider.GetServices<IPipelineBehavour<TCommand, TResult>>()
                                    .Reverse();

            var aggregate =
                behaviors.Aggregate(
                    handler,
                    (next, pipeline) => () => pipeline.ProcessAsync(command, cancellationToken, next));

            return aggregate();
        }
    }
}
