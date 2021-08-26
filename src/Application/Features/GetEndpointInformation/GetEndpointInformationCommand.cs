using Mockingjay.Common.Handling;
using Mockingjay.Entities;

namespace Mockingjay.Features.GetEndpointInformation
{
    public class GetEndpointInformationCommand : ICommand<EndpointInformation>
    {
        public string Path { get; set; }
        public string Method { get; set; }
    }
}
