using System.Threading;
using System.Threading.Tasks;

using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Entities;

using MockEndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointCommandHandler : IRequestHandler<AddEndpointCommand, MockEndpointId>
    {
        private readonly IEventStore<MockEndpointId> _eventStore;

        public AddEndpointCommandHandler(IEventStore<MockEndpointId> eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<MockEndpointId> HandleAsync(AddEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var endpoint = MockEndpoint.Create(command.Path);

            await _eventStore.SaveAsync(endpoint.Buffer);

            return endpoint.Id;
        }
    }
}
