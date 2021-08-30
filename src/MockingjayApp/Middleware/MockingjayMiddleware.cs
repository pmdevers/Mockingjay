using Microsoft.AspNetCore.Http;
using Mockingjay;
using Mockingjay.Common.Handling;
using Mockingjay.Features;
using Serilog;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

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

            var response = await _commandProcessor.SendAsync<GetByRequestCommand, GetByRequestResponse>(
                new GetByRequestCommand
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Query = context.Request.Query,
                });

            if (response.Endpoint != null)
            {
                var endpoint = response.Endpoint;

                Log.Information($"Handling route '{endpoint.Path}'");

                await _commandProcessor.SendAsync(
                    new SetEndpointStatsCommand
                    {
                        Id = endpoint.Id,
                    });
                context.Response.StatusCode = (int)endpoint.StatusCode;
                context.Response.ContentType = endpoint.ContentType;

                if (endpoint.Content != null)
                {
                    var content = endpoint.Content;
                    foreach (var item in response.RouteValues)
                    {
                        var tag = "{" + item.Key + "}";
                        content = content.Replace(tag, item.Value.ToString(), System.StringComparison.InvariantCultureIgnoreCase);
                    }

                    await context.Response.WriteAsync(content);
                }

                return;
            }

            await _commandProcessor.SendAsync<AddEndpointCommand, EndpointId>(new AddEndpointCommand
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                ContentType = context.Request.ContentType,
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Content = "Mockingjay endpoint not found.",
            });

            await next.Invoke(context);
        }
    }
}
