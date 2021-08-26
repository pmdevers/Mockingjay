using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Application;
using Application.Common.DomainModel;
using Application.Common.Security;
using Application.Common.Storage;

namespace Infrastructure.Storage
{
    public class InMemoryEventstore<TId> : IEventStore<TId>
    {
        private readonly IUserService _userService;
        private readonly List<EventDocument<TId>> _events = new ();

        public InMemoryEventstore(IUserService userService)
        {
            _userService = userService;
        }

        public Task SaveAsync(EventBuffer<TId> buffer)
        {
            Guard.NotNull(buffer, nameof(buffer));

            var documents = buffer.SelectUncommitted(AsEventDocument);

            _events.AddRange(documents);

            buffer.MarkAllAsCommitted();

            return Task.CompletedTask;
        }

        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId)
        {
            var results = _events.Where(x => x.AggregateId.Equals(aggregateId));
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
                User = _userService.CurrentUser,
            };
        }

        private static object FromEventDocument(EventDocument<TId> storedEvent) => storedEvent.PayLoad;
    }

    internal class EventDocument<TId>
    {
        public string Id => AggregateId.ToString() + ':' + Version.ToString(CultureInfo.InvariantCulture);
        public TId AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
        public object PayLoad { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
