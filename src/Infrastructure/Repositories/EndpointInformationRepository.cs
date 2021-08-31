using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Mockingjay.Entities;
using Mockingjay.Features;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Infrastructure.Repositories
{
    public class EndpointInformationRepository : IEndpointRepository, IDisposable
    {
        private readonly LiteDatabase _database;
        private bool _disposedValue;

        public EndpointInformationRepository(ConnectionString connectionString)
        {
            _database = new LiteDatabase(connectionString);
            BsonMapper.Global.RegisterType(
                serialize: (endpointId) => endpointId.ToString(),
                deserialize: (bson) => EndpointId.Parse(bson.AsString));
        }

        public Task<int> CountAsync()
        {
            var collection = _database.GetCollection<EndpointInformation>();
            return Task.FromResult(collection.Count());
        }

        public Task DeleteAsync(EndpointId endpointId)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            collection.Delete(endpointId.ToString());
            return Task.CompletedTask;
        }
        public async Task<EndpointInformation> GetByIdAsync(EndpointId endpointId)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            var result = collection.FindOne(x => x.Id == endpointId);
            return await Task.FromResult(result);
        }

        public Task<IEnumerable<EndpointInformation>> GetByMethodAsync(string method)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            var result = collection.Find(x => x.Method == method);
            return Task.FromResult(result);
        }

        public Task<EndpointInformation> GetByRequestAsync(string path, string method)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            var result = collection.FindOne(x => x.Path == path && x.Method == method);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<EndpointInformation>> GetEndpointsAsync()
        {
            var collection = _database.GetCollection<EndpointInformation>();
            var result = collection
                .Query()
                .OrderByDescending(x => x.TotalRequest)
                .ToEnumerable();
            return Task.FromResult(result);
        }

        public Task ResetRequestsAsync()
        {
            var collection = _database.GetCollection<EndpointInformation>();

            collection.UpdateMany(
                x => new EndpointInformation { TotalRequest = 0 },
                x => true);

            return Task.CompletedTask;
        }

        public Task SaveAsync(EndpointInformation endpoint)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            collection.Upsert(endpoint);
            return Task.CompletedTask;
        }

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
