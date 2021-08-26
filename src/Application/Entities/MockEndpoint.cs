using System;

using Application.Common.DomainModel;

using Microsoft.AspNetCore.Http;

using MockEndpointId = Application.Common.Identifiers.Id<Application.ValueObjects.ForMockEndpoint>;

namespace Application.Entities
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
