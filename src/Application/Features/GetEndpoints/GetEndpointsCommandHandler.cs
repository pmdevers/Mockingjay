using Mockingjay.Common.Handling;
using Mockingjay.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class GetEndpointsCommandHandler : IRequestHandler<GetEndpointsCommand, GetEndpointsResponse>
    {
        private readonly IEndpointRepository _repository;

        public GetEndpointsCommandHandler(IEndpointRepository repository)
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
