using Microsoft.AspNetCore.Http;
using Mockingjay;
using Mockingjay.Common.Handling;
using Mockingjay.Features;
using System.Threading.Tasks;

namespace MockingjayApp.Middleware
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

            var endpointInfo = await _commandProcessor.SendAsync<GetByRequestCommand, Mockingjay.Features.Endpoint>(
                new GetByRequestCommand
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Query = context.Request.Query
                });

            if (endpointInfo != null)
            {
                await _commandProcessor.SendAsync(
                    new SetEndpointStatsCommand {
                        Id = endpointInfo.Id,
                    });
                context.Response.StatusCode = (int)endpointInfo.StatusCode;
                context.Response.ContentType = endpointInfo.ContentType;
                if(endpointInfo.Content != null)
                {
                    await context.Response.WriteAsync(endpointInfo.Content);
                }
                

                return;
            }

            await next.Invoke(context);
        }
    }
}
