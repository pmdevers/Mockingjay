using Mockingjay.Common.Identifiers;
using Mockingjay.Entities;
using Mockingjay.Features;
using Mockingjay.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mockingjay.Tests.Infrastructure
{
    public class InMemoryEndpointRepository : IEndpointRepository
    {
        public List<EndpointInformation> Endpoints { get; } = new List<EndpointInformation>();
        public Task<int> CountAsync()
        {
            return Task.FromResult(Endpoints.Count);
        }

        public Task DeleteAsync(Id<ForEndpoint> endpointId)
        {
            Endpoints.RemoveAll(x => x.Id == endpointId);
            return Task.CompletedTask;
        }

        public Task<EndpointInformation> GetByIdAsync(Id<ForEndpoint> endpointId)
        {
            return Task.FromResult(Endpoints.FirstOrDefault(x => x.Id == endpointId));
        }

        public Task<IEnumerable<EndpointInformation>> GetByMethodAsync(string method)
        {
            return Task.FromResult(Endpoints.Where(x => x.Method == method));
        }

        public Task<IEnumerable<EndpointInformation>> GetEndpointsAsync()
        {
            return Task.FromResult(Endpoints.AsEnumerable());
        }

        public Task ResetRequestsAsync()
        {
            Endpoints.ForEach(x => x.TotalRequest = 0);
            return Task.CompletedTask;
        }

        public Task SaveAsync(EndpointInformation endpoint)
        {
            DeleteAsync(endpoint.Id);
            Endpoints.Add(endpoint);
            return Task.CompletedTask;
        }
    }
}
