using Mockingjay.Common.Handling;
using Mockingjay.Common.Identifiers;
using Mockingjay.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class GetAllTopicsCommand : ICommand<IEnumerable<Topic>>
    {
    }

    public class GetAllTopicsCommandHandler : IRequestHandler<GetAllTopicsCommand, IEnumerable<Topic>>
    {
        private readonly ITopicRepository _repository;

        public GetAllTopicsCommandHandler(ITopicRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Topic>> HandleAsync(GetAllTopicsCommand command, CancellationToken cancellationToken = default)
        {
            var test = new List<Topic>
            {
                new Topic { Id = Id<ForTopic>.Next(), From = "test.topic", To = "test.topic2", Content = "Test Topic Content" },
                new Topic { Id = Id<ForTopic>.Next(), From = "test.topic2", To = "test.topic5", Content = "Test Topic Content" },
            };

            return Task.FromResult(test.AsEnumerable());

            return _repository.GetAll();
        }
    }

    public partial interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAll();
    }
}
