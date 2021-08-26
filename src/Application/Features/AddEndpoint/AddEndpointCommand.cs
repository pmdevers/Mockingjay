using Mockingjay.Common.Handling;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointCommand : ICommand<EndpointId>
    {
        public string Path { get; set; }
        public string Method { get; set; }

        public int StatusCode { get; set; }
        public string ContentType { get; set; }

        public string Response { get; set; }
    }
}
