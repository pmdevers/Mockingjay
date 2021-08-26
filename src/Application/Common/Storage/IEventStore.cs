using System.Security.Claims;
using System.Threading.Tasks;

using Application.Common.DomainModel;

namespace Application.Common.Storage
{
    public interface IEventStore<TId>
    {
        public Task SaveAsync(EventBuffer<TId> buffer);
        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId);
    }
}
