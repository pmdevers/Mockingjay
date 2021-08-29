using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoints
{
    public static class GetEndpointsRepositoryExtensions
    {
        public static Task<IEnumerable<EndpointInformation>> GetEndpointsAsync(this IRepository<EndpointInformation> repository, int page, int itemsPerPage)
        {
            Guard.NotNull(repository, nameof(repository));

            return repository.PagedAsync(page, itemsPerPage);
        }

        public static Task<int> CountAsync(this IRepository<EndpointInformation> repository)
        {
            Guard.NotNull(repository, nameof(repository));

            return repository.CountAsync();
        }
    }
}
