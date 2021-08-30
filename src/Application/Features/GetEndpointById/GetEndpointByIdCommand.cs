using Mockingjay.Common.Handling;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class GetEndpointByIdCommand : ICommand<Endpoint>
    {
        public EndpointId EndpointId { get; set; }
    }
}
