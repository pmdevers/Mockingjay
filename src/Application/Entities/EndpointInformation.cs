using System.Net;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Entities
{
    public class EndpointInformation
    {
        public EndpointId Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }

        public string Response { get; set; }
        public int TotalRequest { get; set; }
    }
}
