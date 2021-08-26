using System.Threading;
using System.Threading.Tasks;

using Mockingjay.Common.Handling;
using Mockingjay.Common.Repositories;
using Mockingjay.Entities;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointCommandHandler : IRequestHandler<AddEndpointCommand, EndpointId>
    {
        private readonly IRepository<EndpointInformation> _repository;

        public AddEndpointCommandHandler(IRepository<EndpointInformation> repository)
        {
            _repository = repository;
        }

        public async Task<EndpointId> HandleAsync(AddEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            var endpoint = new EndpointInformation
            {
                Id = EndpointId.Next(),
                Path = command.Path,
                Method = command.Method,
                StatusCode = command.StatusCode,
                ContentType = command.ContentType,
                Response = command.Response,
            };

            await _repository.SaveAsync(endpoint);

            return endpoint.Id;
        }
    }
}
