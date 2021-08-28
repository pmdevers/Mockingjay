using Mockingjay.Common.Handling;
using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoints
{
    public class GetEndpointsCommandHandler : IRequestHandler<GetEndpointsCommand, GetEndpointsResponse>
    {
        private readonly IRepository<EndpointInformation> _repository;

        public GetEndpointsCommandHandler(IRepository<EndpointInformation> repository)
        {
            _repository = repository;
        }
        public async Task<GetEndpointsResponse> HandleAsync(GetEndpointsCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            cancellationToken.ThrowIfCancellationRequested();
            var totalItems = await _repository.CountAsync();
            var items = await _repository.GetEndpointsAsync(command.Page, command.ItemsPerPage);

            return new GetEndpointsResponse(
                    items.ToList(),
                    command.Page,
                    command.ItemsPerPage,
                    (totalItems / command.ItemsPerPage) + 1,
                    totalItems);
        }
    }
}
