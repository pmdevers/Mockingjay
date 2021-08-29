using Mockingjay.Common.Handling;
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
            var items = await _repository.GetEndpointsAsync();

            return new GetEndpointsResponse(items);
        }
    }
}
