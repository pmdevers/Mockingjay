using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class ExportCommandHandler : IRequestHandler<ExportCommand, byte[]>
    {
        public Task<byte[]> HandleAsync(ExportCommand command, CancellationToken cancellationToken = default)
        {
            return File.ReadAllBytesAsync(EndpointDatafile.FullPath, cancellationToken);
        }
    }
}
