using Mockingjay.Common.Handling;
using Mockingjay.Common.Http;
using Mockingjay.Common.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class GetByRequestCommandHandler : IRequestHandler<GetByRequestCommand, Endpoint>
    {
        private readonly IEndpointRepository _repository;
        private readonly IEventStore<EndpointId> _eventStore;
        private readonly IRouteMatcher _matcher;

        public GetByRequestCommandHandler(
            IEndpointRepository repository,
            IEventStore<EndpointId> eventStore,
            IRouteMatcher matcher)
        {
            _repository = repository;
            _eventStore = eventStore;
            _matcher = matcher;
        }
        public async Task<Endpoint> HandleAsync(GetByRequestCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var items = await _repository.GetByMethodAsync(command.Method);
            var routes = items.ToDictionary(x => x.Id, y => y.Path);

            foreach (var route in routes)
            {
                var match = _matcher.Match(route.Value, command.Path, command.Query);
                if (match != null)
                {
                    return await _eventStore.Aggregate<Endpoint, EndpointId>(route.Key);
                }
            }

            return null;
        }
    }
}
