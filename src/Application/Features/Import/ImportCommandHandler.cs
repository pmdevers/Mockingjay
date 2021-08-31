using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class ImportCommandHandler : IRequestHandler<ImportCommand>
    {
        public Task<Unit> HandleAsync(ImportCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            File.Copy(command.Filename, EndpointDatafile.FullPath, true);
            return Task.FromResult(Unit.Empty);
        }
    }
}
