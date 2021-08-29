using Mockingjay.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mockingjay.Features
{

    public partial interface IEndpointRepository
    {
        Task<IEnumerable<EndpointInformation>> GetEndpointsAsync();
        Task<int> CountAsync();
    }
}
