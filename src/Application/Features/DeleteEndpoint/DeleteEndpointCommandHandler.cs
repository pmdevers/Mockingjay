using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class DeleteEndpointCommandHandler : IRequestHandler<DeleteEndpointCommand>
    {
        private readonly IEventStore<EndpointId> _store;
        private readonly IEndpointRepository _repository;

        public DeleteEndpointCommandHandler(IEventStore<EndpointId> store, IEndpointRepository repository)
        {
            _store = store;
            _repository = repository;
        }
        public async Task<Unit> HandleAsync(DeleteEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var endpoint = await _store.Aggregate<Endpoint, EndpointId>(command.EndpointId);

            endpoint.Delete();

            await _store.SaveAsync(endpoint.Buffer);
            await _repository.DeleteAsync(endpoint.Id);

            return Unit.Empty;
        }
    }
}
