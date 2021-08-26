using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpointInformation
{
    public static class EndpointRepositoryGetByRequest
    {
        public static async Task<EndpointInformation> GetByRequestAsync(this IRepository<EndpointInformation> repository, string path, string method)
        {
            Guard.NotNull(repository, nameof(repository));
            var result = repository.AsQueryable().FirstOrDefault(x => x.Path == path && x.Method == method);
            return await Task.FromResult(result);
        }
    }
}
