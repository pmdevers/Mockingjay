using Mockingjay.Common.Handling;
using System.Net;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointCommand : ICommand<EndpointId>
    {
        public string Path { get; set; }
        public string Method { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }

        public string Response { get; set; }
    }
}
