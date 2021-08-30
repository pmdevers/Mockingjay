using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;

using System.Threading;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class GetEndpointByIdCommandHandler : IRequestHandler<GetEndpointByIdCommand, Endpoint>
    {
        private readonly IEventStore<EndpointId> _store;

        public GetEndpointByIdCommandHandler(IEventStore<EndpointId> store)
        {
            _store = store;
        }

        public async Task<Endpoint> HandleAsync(GetEndpointByIdCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var result = await _store.Aggregate<Endpoint, EndpointId>(command.EndpointId);
            return result;
        }
    }
}
