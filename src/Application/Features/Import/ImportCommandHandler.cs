using Microsoft.Extensions.Logging;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class ImportCommandHandler : IRequestHandler<ImportCommand>
    {
        private readonly ILogger<ImportCommandHandler> _logger;

        public ImportCommandHandler(ILogger<ImportCommandHandler> logger)
        {
            _logger = logger;
        }
        public Task<Unit> HandleAsync(ImportCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));

            File.WriteAllBytes(EndpointDatafile.FullPath, command.Bytes);

            _logger.LogInformation("Datafile imported!");
            return Task.FromResult(Unit.Empty);
        }
    }
}
