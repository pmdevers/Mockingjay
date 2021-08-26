using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Repositories
{
    public interface IRepository<T>
    {
        Task<T> FindByIdAsync(string id);
        Task SaveAsync(T entity);
        Task DeleteAsync(T entity);

        IQueryable<T> AsQueryable();
    }
}
