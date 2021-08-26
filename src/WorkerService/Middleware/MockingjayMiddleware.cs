using Microsoft.AspNetCore.Http;
using Mockingjay;
using Mockingjay.Common.Handling;
using Mockingjay.Entities;
using Mockingjay.Features.GetEndpoint;
using System.Threading.Tasks;

namespace WorkerService.Middleware
{
    public class MockingjayMiddleware : IMiddleware
    {
        private readonly ICommandProcessor _commandProcessor;

        public MockingjayMiddleware(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(next, nameof(next));

            var endpointInfo = await _commandProcessor.SendAsync<GetEndpointCommand, EndpointInformation>(
                new GetEndpointCommand
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                });

            if (endpointInfo != null)
            {
                context.Response.StatusCode = endpointInfo.StatusCode;
                context.Response.ContentType = endpointInfo.ContentType;
                await context.Response.WriteAsync(endpointInfo.Response);

                return;
            }

            await next.Invoke(context);
        }
    }
}
