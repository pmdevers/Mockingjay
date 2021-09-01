using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Mockingjay.Common.DomainModel;
using Mockingjay.Common.Storage;

namespace Application.Tests.Infrastructure.Storage
{
    public class InMemoryEventStore<TId> : IEventStore<TId>
    {
        private readonly List<EventDocument<TId>> _events = new ();

        public Task SaveAsync(EventBuffer<TId> buffer)
        {
            throw new NotImplementedException();
        }

        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId)
        {
            throw new NotImplementedException();
        }
    }

    internal class EventDocument<TId>
    {
        public string Id => AggregateId.ToString() + ':' + Version.ToString(CultureInfo.InvariantCulture);
        public TId AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
        public object PayLoad { get; set; }
    }
}
