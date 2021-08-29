using Mockingjay.Entities;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public partial interface IEndpointRepository
    {
        Task SaveAsync(EndpointInformation endpoint);
    }
}
