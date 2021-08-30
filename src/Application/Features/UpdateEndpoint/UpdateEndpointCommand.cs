using Mockingjay.Common.Handling;
using System.Net;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class UpdateEndpointCommand : ICommand<EndpointId>
    {
        public EndpointId EndpointId { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
