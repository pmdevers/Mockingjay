using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;
using System.Threading;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class UpdateEndpointCommandCommandHandler : IRequestHandler<UpdateEndpointCommand, EndpointId>
    {
        private readonly IEventStore<EndpointId> _eventStore;
        private readonly IEndpointRepository _repository;

        public UpdateEndpointCommandCommandHandler(IEventStore<EndpointId> eventStore, IEndpointRepository repository)
        {
            _eventStore = eventStore;
            _repository = repository;
        }

        public async Task<EndpointId> HandleAsync(UpdateEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var endpoint = await _eventStore.Aggregate<Endpoint, EndpointId>(command.EndpointId);

            endpoint.Update(command.Path, command.Method, command.StatusCode, command.ContentType, command.Content);
            await _eventStore.SaveAsync(endpoint.Buffer);

            var ei = await _repository.GetByIdAsync(endpoint.Id);
            await _repository.SaveAsync(ProjectOn(ei, endpoint));

            return endpoint.Id;
        }

        private static EndpointInformation ProjectOn(EndpointInformation ei, Endpoint e)
        {
            ei.Path = e.Path;
            ei.Method = e.Method;
            ei.StatusCode = e.StatusCode;
            ei.ContentType = e.ContentType;
            ei.Response = e.Content;
            return ei;
        }
    }
}
