﻿using System;
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

        public void Dispose()
        {
            _database.Dispose();
        }

        public Task<EndpointInformation> GetByIdAsync(EndpointId endpointId)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            var result = collection.FindOne(x => x.Id == endpointId);
            return Task.FromResult(result);
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

        public Task SaveAsync(EndpointInformation endpoint)
        {
            var collection = _database.GetCollection<EndpointInformation>();
            collection.Upsert(endpoint);
            return Task.CompletedTask;
        }
    }
}
