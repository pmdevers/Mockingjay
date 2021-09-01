using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Mockingjay.Common.DomainModel;
using Mockingjay.Common.Storage;

namespace Mockingjay.Tests.Infrastructure.Storage
{
    public class InMemoryStorage<TId> : IEventStore<TId>
    {
        public List<EventDocument<TId>> Events { get; } = new ();

        public Task SaveAsync(EventBuffer<TId> buffer)
        {
            Guard.NotNull(buffer, nameof(buffer));

            var documents = buffer.SelectUncommitted(AsEventDocument);

            Events.AddRange(documents);

            buffer.MarkAllAsCommitted();

            return Task.CompletedTask;
        }

        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId)
        {
            var results = Events.Where(x => x.AggregateId.Equals(aggregateId));
            return Task.FromResult(EventBuffer<TId>.FromStorage(aggregateId, 0, results, FromEventDocument));
        }

        private EventDocument<TId> AsEventDocument(TId aggregateId, int version, object @event)
        {
            return new EventDocument<TId>
            {
                AggregateId = aggregateId,
                Version = version,
                EventType = @event.GetType().FullName,
                PayLoad = @event,
            };
        }

        private static object FromEventDocument(EventDocument<TId> storedEvent) => storedEvent.PayLoad;
    }

    public class EventDocument<TId>
    {
        public string Id => AggregateId.ToString() + ':' + Version.ToString(CultureInfo.InvariantCulture);
        public TId AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
        public object PayLoad { get; set; }
    }
}
