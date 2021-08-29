using Mockingjay.Common.Handling;

namespace Mockingjay.Features
{
    public class GetEndpointsCommand : ICommand<GetEndpointsResponse>
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
