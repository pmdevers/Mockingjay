using Mockingjay.Common.Handling;
using Mockingjay.Entities;

namespace Mockingjay.Features
{
    public class GetEndpointByPathCommand : ICommand<EndpointInformation>
    {
        public string Path { get; set; }
        public string Method { get; set; }
    }
}
