using System.Threading;
using System.Threading.Tasks;

using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class CreateEndpointCommandHandler : IRequestHandler<CreateEndpointCommand, EndpointId>
    {
        private readonly IEventStore<EndpointId> _eventStore;
        private readonly IEndpointRepository _repository;

        public CreateEndpointCommandHandler(IEventStore<EndpointId> eventStore, IEndpointRepository repository)
        {
            _eventStore = eventStore;
            _repository = repository;
        }

        public async Task<EndpointId> HandleAsync(CreateEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var e = Endpoint.Create(
                command.Path,
                command.Method,
                command.StatusCode,
                command.ContentType,
                command.Content);

            await _eventStore.SaveAsync(e.Buffer);
            await _repository.SaveAsync(Project(e));

            return e.Id;
        }

        private EndpointInformation Project(Endpoint e) => new ()
        {
                Id = e.Id,
                Path = e.Path,
                Method = e.Method,
                StatusCode = e.StatusCode,
                ContentType = e.ContentType,
                Response = e.Content,
        };
    }
}
