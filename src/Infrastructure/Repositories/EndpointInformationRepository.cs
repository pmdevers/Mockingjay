using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Mockingjay.Entities;
using Mockingjay.Features;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Infrastructure.Repositories
{
    public class EndpointInformationRepository : IEndpointRepository
    {
        private readonly ConnectionString _connectionString;

        public EndpointInformationRepository(ConnectionString connectionString)
        {
            BsonMapper.Global.RegisterType(
                serialize: (endpointId) => endpointId.ToString(),
                deserialize: (bson) => EndpointId.Parse(bson.AsString));
            _connectionString = connectionString;
        }

        public async Task<int> CountAsync()
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            return await Task.FromResult(collection.Count());
        }

        public async Task DeleteAsync(EndpointId endpointId)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            collection.Delete(endpointId.ToString());
            await Task.CompletedTask;
        }
        public async Task<EndpointInformation> GetByIdAsync(EndpointId endpointId)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            var result = collection.FindOne(x => x.Id == endpointId);
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<EndpointInformation>> GetByMethodAsync(string method)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            var result = collection.Find(x => x.Method == method);
            return await Task.FromResult(result);
        }

        public async Task<EndpointInformation> GetByRequestAsync(string path, string method)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            var result = collection.FindOne(x => x.Path == path && x.Method == method);
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<EndpointInformation>> GetEndpointsAsync()
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            var result = collection
                .Query()
                .OrderByDescending(x => x.TotalRequest)
                .ToEnumerable();
            return await Task.FromResult(result);
        }

        public async Task ResetRequestsAsync()
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();

            collection.UpdateMany(
                x => new EndpointInformation { TotalRequest = 0 },
                x => true);

            await Task.CompletedTask;
        }

        public async Task SaveAsync(EndpointInformation endpoint)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<EndpointInformation>();
            collection.Upsert(endpoint);
            await Task.CompletedTask;
        }
    }
}
