using System;

using Mockingjay.Common.DomainModel;

using Microsoft.AspNetCore.Http;

using MockEndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Entities
{
    public class MockEndpoint : AggregateRoot<MockEndpoint, MockEndpointId>
    {
        public MockEndpoint() : base(MockEndpointId.Next()) { }
        protected MockEndpoint(MockEndpointId aggregateId) : base(aggregateId)
        {
        }

        public PathString Path { get; internal set; }

        public static MockEndpoint Create(PathString path)
        {
            var endpoint = new MockEndpoint();
            return endpoint.ApplyEvent(new EndpointCreated(endpoint.Id, path));
        }

        internal void When(EndpointCreated e)
        {
            Path = e.path;
        }
    }

    public record EndpointCreated(MockEndpointId id, PathString path);
}
