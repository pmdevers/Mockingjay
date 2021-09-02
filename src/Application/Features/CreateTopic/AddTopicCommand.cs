using Mockingjay.Common.Handling;
using Mockingjay.Common.Identifiers;
using Mockingjay.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class CreateTopicCommand : ICommand<Id<ForTopic>>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }

    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Id<ForTopic>>
    {
        private readonly ITopicRepository _repository;

        public CreateTopicCommandHandler(ITopicRepository repository)
        {
            _repository = repository;
        }

        public async Task<Id<ForTopic>> HandleAsync(CreateTopicCommand command, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(command, nameof(command));
            var topic = new Topic
            {
                Id = Id<ForTopic>.Next(),
                From = command.From,
                To = command.To,
                Content = command.Content,
            };
            await _repository.SaveAsync(topic);

            return topic.Id;
        }
    }

    public partial interface ITopicRepository
    {
        Task SaveAsync(Topic topic);
    }

    public partial class Topic
    {
        public Id<ForTopic> Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content {get; set; }
    }
}
