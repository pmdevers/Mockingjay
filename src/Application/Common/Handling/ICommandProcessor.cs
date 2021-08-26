using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Handling
{
    public interface ICommandProcessor
    {
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand<Unit>;

        Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand<TResult>;
    }
}
