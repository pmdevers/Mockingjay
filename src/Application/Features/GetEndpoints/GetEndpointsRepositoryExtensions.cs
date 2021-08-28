using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoints
{
    public static class GetEndpointsRepositoryExtensions
    {
        public static Task<IQueryable<EndpointInformation>> GetEndpointsAsync(this IRepository<EndpointInformation> repository, int page, int itemsPerPage)
        {
            Guard.NotNull(repository, nameof(repository));

            return Task.FromResult(
                repository.AsQueryable()
                .Skip(page * (page - 1))
                .Take(itemsPerPage));
        }

        public static Task<int> CountAsync(this IRepository<EndpointInformation> repository)
        {
            Guard.NotNull(repository, nameof(repository));

            return Task.FromResult(repository.AsQueryable().Count());
        }
    }
}
