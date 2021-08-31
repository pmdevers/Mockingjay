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
    public class LiteDBEventstore<TId> : IEventStore<TId>, IDisposable
    {
        private readonly LiteDatabase _database;
        private readonly IUserService _userService;
        private bool _disposedValue;

        public LiteDBEventstore(IUserService userService, ConnectionString connectionString)
        {
            _database = new LiteDatabase(connectionString);
            _userService = userService;

            BsonMapper.Global.RegisterType(
                serialize: (endpointId) => endpointId.ToString(),
                deserialize: (bson) => EndpointId.Parse(bson.AsString));
        }

        public Task SaveAsync(EventBuffer<TId> buffer)
        {
            Guard.NotNull(buffer, nameof(buffer));

            var collection = _database.GetCollection<EventDocument>("events");
            collection.EnsureIndex(x => x.AggregateId);

            var documents = buffer.SelectUncommitted(AsEventDocument);
            collection.InsertBulk(documents);

            buffer.MarkAllAsCommitted();
            return Task.CompletedTask;
        }

        public Task<EventBuffer<TId>> LoadAsync(TId aggregateId)
        {
            var collection = _database.GetCollection<EventDocument>("events");
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

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _database.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
