using Application.Common.Handling;

using Microsoft.AspNetCore.Http;

using EndpointId = Application.Common.Identifiers.Id<Application.ValueObjects.ForMockEndpoint>;

namespace Application.Features.AddEndpoint
{
    public class AddEndpointCommand : ICommand<EndpointId>
    {
        public PathString Path { get; set; }
    }
}
