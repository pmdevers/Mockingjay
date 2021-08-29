using Mockingjay.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public partial interface IEndpointRepository
    {
        Task<EndpointInformation> GetByRequestAsync(string path, string method);
        Task<IEnumerable<EndpointInformation>> GetByMethodAsync(string method);
    }
}
