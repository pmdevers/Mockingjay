using Mockingjay.Common.Handling;
using Mockingjay.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class GetEndpointByPathCommandHandler : IRequestHandler<GetEndpointByPathCommand, EndpointInformation>
    {
        private readonly IEndpointRepository _repository;

        public GetEndpointByPathCommandHandler(IEndpointRepository repository)
        {
            _repository = repository;
        }

        public async Task<EndpointInformation> HandleAsync(GetEndpointByPathCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var result = await _repository.GetByRequestAsync(command.Path, command.Method);
            return result;
        }
    }
}
