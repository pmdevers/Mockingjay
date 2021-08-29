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

        public IQueryable<EndpointInformation> AsQueryable()
        {
            return _entities.AsQueryable();
        }

        public Task DeleteAsync(EndpointInformation entity)
        {
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<EndpointInformation> FindByIdAsync(string id)
        {
            return Task.FromResult(_entities.FirstOrDefault(x => x.Id == EndpointId.Parse(id)));
        }

        public Task SaveAsync(EndpointInformation entity)
        {
            _entities.Add(entity);
            return Task.CompletedTask;
        }
    }
}
