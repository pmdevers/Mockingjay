using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoint
{
    public static class GetEndpointRepositoryExtensions
    {
        public static async Task<EndpointInformation> GetEndpointByRequestAsync(this IRepository<EndpointInformation> repository, string path, string method)
        {
            Guard.NotNull(repository, nameof(repository));
            //var result = repository.AsQueryable().FirstOrDefault(x => x.Path == path && x.Method == method);
            return await Task.FromResult(new EndpointInformation());
        }
    }
}
