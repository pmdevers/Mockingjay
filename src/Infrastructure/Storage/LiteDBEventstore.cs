using LiteDB;
using Mockingjay;
using Mockingjay.Common.DomainModel;
using Mockingjay.Common.Security;
using Mockingjay.Common.Storage;
using System;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Infrastructure.Storage
{
    public class LiteDBEventstore<TId> : IEventStore<TId>
    {
        private readonly IUserService _userService;
        private readonly ConnectionString _connectionString;

        public LiteDBEventstore(IUserService userService, ConnectionString connectionString)
        {
            _userService = userService;
            _connectionString = connectionString;
            BsonMapper.Global.RegisterType(
                serialize: (endpointId) => endpointId.ToString(),
                deserialize: (bson) => EndpointId.Parse(bson.AsString));
        }

        public Task SaveAsync(EventBuffer<TId> buffer)
        {
            Guard.NotNull(buffer, nameof(buffer));

            using var database = new LiteDatabase(_connectionString);

            var collection = database.GetCollection<EventDocument>("events");
            collection.EnsureIndex(x => x.AggregateId);

            var documents = buffer.SelectUncommitted(AsEventDocument);
            collection.InsertBulk(documents);

            buffer.MarkAllAsCommitted();
            return Task.CompletedTask;
        }

        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId)
        {
            using var database = new LiteDatabase(_connectionString);

            var collection = database.GetCollection<EventDocument>("events");
            collection.EnsureIndex(x => x.AggregateId);
            var results = collection.Query()
                .Where(x => x.AggregateId == aggregateId.ToString())
                .OrderBy(x => x.Version)
                .ToEnumerable();

            return Task.FromResult(EventBuffer<TId>.FromStorage(aggregateId, 0, results, FromEventDocument));
        }

        private EventDocument AsEventDocument<TId>(TId aggregateId, int version, object @event)
        {
            return new EventDocument
            {
                AggregateId = aggregateId.ToString(),
                Version = version,
                EventType = @event.GetType().FullName,
                PayLoad = @event,
                User = _userService.CurrentUser,
            };
        }

        private static object FromEventDocument(EventDocument storedEvent) => storedEvent.PayLoad;
    }
}
