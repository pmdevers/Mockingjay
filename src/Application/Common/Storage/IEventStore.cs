using System.Threading.Tasks;

using Mockingjay.Common.DomainModel;

namespace Mockingjay.Common.Storage
{
    public interface IEventStore<TId>
    {
        public Task SaveAsync(EventBuffer<TId> buffer);
        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId);
    }

    public static class EventStorageExtensions
    {
        public static async Task<TAggregate> Aggregate<TAggregate, TId>(this IEventStore<TId> storage, TId aggregateId)
            where TAggregate : AggregateRoot<TAggregate, TId>, new()
        {
            Guard.NotNull(storage, nameof(storage));
            var buffer = await storage.LoadAsync(aggregateId);
            return AggregateRoot.FromStorage<TAggregate, TId>(buffer);
        }
    }
}
