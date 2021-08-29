using Mockingjay.Common.Handling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class SetEndpointStatsCommandHandler : IRequestHandler<SetEndpointStatsCommand>
    {
        private readonly IEndpointRepository _repository;

        public SetEndpointStatsCommandHandler(IEndpointRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> HandleAsync(SetEndpointStatsCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var info = await _repository.GetByIdAsync(command.Id);

            info.TotalRequest += 1;

            await _repository.SaveAsync(info);

            return Unit.Empty;
        }
    }
}
