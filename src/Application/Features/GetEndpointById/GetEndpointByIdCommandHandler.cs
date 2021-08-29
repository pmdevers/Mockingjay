using Mockingjay.Common.Handling;
using Mockingjay.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class GetEndpointByIdCommandHandler : IRequestHandler<GetEndpointByIdCommand, EndpointInformation>
    {
        private readonly IEndpointRepository _repository;

        public GetEndpointByIdCommandHandler(IEndpointRepository repository)
        {
            _repository = repository;
        }

        public async Task<EndpointInformation> HandleAsync(GetEndpointByIdCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var result = await _repository.GetByIdAsync(command.EndpointId);
            return result;
        }
    }
}
