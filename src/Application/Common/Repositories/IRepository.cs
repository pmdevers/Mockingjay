using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mockingjay.Common.Repositories
{
    public interface IRepository<T>
    {
        Task<T> FindByIdAsync(string id);
        Task SaveAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> CountAsync();
        Task<IEnumerable<T>> PagedAsync(int page, int itemsPerPage);
    }
}
