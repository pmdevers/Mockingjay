using LiteDB;
using Mockingjay.Common.Repositories;
using Mockingjay.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Infrastructure.Repositories
{
    public class EndpointInformationRepository : IRepository<EndpointInformation>
    {
        private readonly List<EndpointInformation> _entities = new List<EndpointInformation>();
        private readonly LiteDatabase _database;

        public EndpointInformationRepository()
        {
            _database = new LiteDatabase("Mockingjay.db");
            BsonMapper.Global.RegisterType<EndpointId>(
                serialize: (endpointId) => endpointId.ToString(),
                deserialize: (bson) => EndpointId.Parse(bson.AsString)
            );
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(GetCollection().Count());
        }

        public Task DeleteAsync(EndpointInformation entity)
        {
            GetCollection().Delete(entity.Id.ToString());
            return Task.CompletedTask;
        }

        public Task<EndpointInformation> FindByIdAsync(string id)
        {
            return Task.FromResult(GetCollection().FindOne(id));
        }

        public Task<IEnumerable<EndpointInformation>> PagedAsync(int page, int itemsPerPage)
        {
            var result = GetCollection()
                .Query()
                .Skip(page * (page - 1))
                .Limit(itemsPerPage)
                .ToEnumerable();
            return Task.FromResult(result);
        }

        public Task SaveAsync(EndpointInformation entity)
        {
            GetCollection().Insert(entity);
            return Task.CompletedTask;
        }

        private ILiteCollection<EndpointInformation> GetCollection() =>
            _database.GetCollection<EndpointInformation>();
    }
}
