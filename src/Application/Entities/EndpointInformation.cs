using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace Mockingjay.Entities
{
    public class EndpointInformation
    {
        public EndpointId Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }

        public int StatusCode { get; set; }
        public string ContentType { get; set; }

        public string Response { get; set; }
    }
}
