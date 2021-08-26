using System.Threading;
using System.Threading.Tasks;

using Application.Common.Handling;
using Application.Common.Repositories;
using Application.Common.Security;
using Application.Common.Storage;
using Application.Entities;

using MockEndpointId = Application.Common.Identifiers.Id<Application.ValueObjects.ForMockEndpoint>;

namespace Application.Features.AddEndpoint
{
    public class AddEndpointCommandHandler : IRequestHandler<AddEndpointCommand, MockEndpointId>
    {
        private readonly IEventStore<MockEndpointId> _eventStore;
        private readonly IUserService _userService;

        public AddEndpointCommandHandler(IEventStore<MockEndpointId> eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<MockEndpointId> HandleAsync(AddEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var endpoint = MockEndpoint.Create(command.Path);

            await _eventStore.SaveAsync(endpoint.Buffer);

            return Task.FromResult(endpoint.Id);
        }
    }
}
