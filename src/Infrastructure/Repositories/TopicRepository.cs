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
            BsonMapper.Global.RegisterType(
                serialize: (id) => id.ToString(),
                deserialize: (bson) => Id<ForTopic>.Parse(bson.AsString));
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
           using var database = new LiteDatabase(_connectionString);
           var collection = database.GetCollection<Topic>();
           return await Task.FromResult(collection.FindAll());
        }

        public Task SaveAsync(Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}
