using Mockingjay.Common.Handling;
using Mockingjay.Common.Repositories;
using Mockingjay.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpointInformation
{
    public class GetEndpointInformationCommandHandler : IRequestHandler<GetEndpointInformationCommand, EndpointInformation>
    {
        private readonly IRepository<EndpointInformation> _repository;

        public GetEndpointInformationCommandHandler(IRepository<EndpointInformation> repository)
        {
            _repository = repository;
        }

        public async Task<EndpointInformation> HandleAsync(GetEndpointInformationCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var result = await _repository.GetByRequestAsync(command.Path, command.Method);
            return result;
        }
    }
}
