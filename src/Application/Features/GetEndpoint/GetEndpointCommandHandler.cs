using Mockingjay.Common.Handling;
using Mockingjay.Common.Repositories;
using Mockingjay.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoint
{
    public class GetEndpointCommandHandler : IRequestHandler<GetEndpointCommand, EndpointInformation>
    {
        private readonly IRepository<EndpointInformation> _repository;

        public GetEndpointCommandHandler(IRepository<EndpointInformation> repository)
        {
            _repository = repository;
        }

        public async Task<EndpointInformation> HandleAsync(GetEndpointCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var result = await _repository.GetEndpointByRequestAsync(command.Path, command.Method);
            return result;
        }
    }
}
