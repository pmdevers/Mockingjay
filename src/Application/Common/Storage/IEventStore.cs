using System.Security.Claims;
using System.Threading.Tasks;

using Mockingjay.Common.DomainModel;

namespace Mockingjay.Common.Storage
{
    public interface IEventStore<TId>
    {
        public Task SaveAsync(EventBuffer<TId> buffer);
        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId);
    }
}
