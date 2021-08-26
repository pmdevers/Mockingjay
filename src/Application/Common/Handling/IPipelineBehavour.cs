using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Common.Handling
{
    public interface IPipelineBehavour<in TRequest, TResult>
    {
        Task<TResult> ProcessAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResult>> next);
    }
}
