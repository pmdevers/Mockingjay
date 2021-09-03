using LiteDB;
using Mockingjay.Common.Identifiers;
using Mockingjay.Features;
using Mockingjay.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ConnectionString _connectionString;

        public TopicRepository(ConnectionString connectionString)
        {
            BsonMapper.Global.RegisterType<Id<ForTopic>>(
                serialize: (id) => id.ToString(),
                deserialize: (bson) => Id<ForTopic>.Parse(bson.AsString));
            BsonMapper.Global.Entity<Topic>()
                .Id(x => x.Id);
            _connectionString = connectionString;
        }

        public Task DeleteAsync(Id<ForTopic> id)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<Topic>();
            collection.DeleteMany(x => x.Id == id);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
           using var database = new LiteDatabase(_connectionString);
           var collection = database.GetCollection<Topic>();
           return await Task.FromResult(collection.FindAll());
        }

        public Task SaveAsync(Topic topic)
        {
            using var database = new LiteDatabase(_connectionString);
            var collection = database.GetCollection<Topic>();
            collection.Upsert(topic);
            return Task.CompletedTask;
        }
    }
}
