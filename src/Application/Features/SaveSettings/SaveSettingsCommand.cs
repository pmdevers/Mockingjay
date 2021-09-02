using Mockingjay.Common;
using Mockingjay.Common.Handling;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class SaveSettingsCommand : Settings, ICommand
    {
    }

    public class SaveSettingsCommandHandler : IRequestHandler<SaveSettingsCommand>
    {
        private readonly ISettingsRepository _repository;
        private readonly SettingsService _settingService;

        public SaveSettingsCommandHandler(ISettingsRepository repository, SettingsService settingService)
        {
            _repository = repository;
            _settingService = settingService;
        }

        public Task<Unit> HandleAsync(SaveSettingsCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            _repository.Save(command);
            _settingService.Reset(command);
            return Unit.Task;
        }
    }

    public partial interface ISettingsRepository
    {
        void Save(Settings settings);
    }
}
