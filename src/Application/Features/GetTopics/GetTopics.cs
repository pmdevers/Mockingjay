using Mockingjay.Common.Handling;
using System.Collections.Generic;
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
            return _repository.GetAll();
        }
    }

    public partial interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAll();
    }
}
