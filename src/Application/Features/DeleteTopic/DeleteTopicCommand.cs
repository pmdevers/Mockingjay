using Mockingjay.Common.Handling;
using Mockingjay.Common.Identifiers;
using Mockingjay.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class DeleteTopicCommand : ICommand
    {
        public Id<ForTopic> TopicId { get; set; }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteTopicCommand>
    {
        private readonly ITopicRepository _repository;

        public DeleteCommandHandler(ITopicRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> HandleAsync(DeleteTopicCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            await _repository.DeleteAsync(command.TopicId);
            return Unit.Empty;
        }
    }

    public partial interface ITopicRepository
    {
        Task DeleteAsync(Id<ForTopic> id);
    }
}
