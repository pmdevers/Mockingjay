using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Handling
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S4023:Interfaces should not be empty",
        Justification = "This is a Placeholder Interface")]
    public interface IRequestHandler<in TCommand> : IRequestHandler<TCommand, Unit>
    {
    }

    public interface IRequestHandler<in TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
