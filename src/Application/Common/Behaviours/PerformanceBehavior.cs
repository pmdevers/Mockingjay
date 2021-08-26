using Mockingjay.Common.Handling;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Common.Behaviours
{
    public class PerformanceBehavior<TRequest, TResults> : IPipelineBehavior<TRequest, TResults>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResults> ProcessAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResults>> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(
                    "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    requestName,
                    elapsedMilliseconds,
                    request);
            }

            return response;
        }
    }
}
