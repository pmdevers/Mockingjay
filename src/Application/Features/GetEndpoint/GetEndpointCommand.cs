using Mockingjay.Common.Handling;
using Mockingjay.Entities;

namespace Mockingjay.Features.GetEndpoint
{
    public class GetEndpointCommand : ICommand<EndpointInformation>
    {
        public string Path { get; set; }
        public string Method { get; set; }
    }
}
