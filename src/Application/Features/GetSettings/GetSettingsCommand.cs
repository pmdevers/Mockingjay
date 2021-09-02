using Mockingjay.Common.Handling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public partial class Settings
    {
        public string BootstrapServices { get; set; } = "localhost:9092";
        public string GroupId { get; set; } = "MockingjayApp";
    }

    public class GetSettingsCommand : ICommand<Settings>
    {
    }

    public class GetSettingsCommandHandler : IRequestHandler<GetSettingsCommand, Settings>
    {
        private readonly ISettingsRepository _respository;

        public GetSettingsCommandHandler(ISettingsRepository respository)
        {
            _respository = respository;
        }
        public Task<Settings> HandleAsync(GetSettingsCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_respository.Get());
        }
    }

    public partial interface ISettingsRepository
    {
        Settings Get();
    }
}
