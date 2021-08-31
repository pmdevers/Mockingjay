using Mockingjay.Common.Handling;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class ResetRequestsCommandHandler : IRequestHandler<ResetRequestsCommand>
    {
        private readonly IEndpointRepository _repository;

        public ResetRequestsCommandHandler(IEndpointRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> HandleAsync(ResetRequestsCommand command, CancellationToken cancellationToken = default)
        {
            await _repository.ResetRequestsAsync();

            return Unit.Empty;
        }
    }
}
