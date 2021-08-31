using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class ImportEndpointsCommandHandler : IRequestHandler<ImportEndpointsCommand>
    {
        public Task<Unit> HandleAsync(ImportEndpointsCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            File.Copy(command.Filename, EndpointDatafile.FullPath, true);
            return Task.FromResult(Unit.Empty);
        }
    }
}
