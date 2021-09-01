using AutoFixture;
using FluentAssertions;
using Mockingjay.Common.DomainModel;
using Mockingjay.Entities;
using Mockingjay.Features;
using NUnit.Framework;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Tests.Features
{
    using static Testing;
    public class UpdateEndpointTests: TestBase
    {
        [Test]
        public async Task Update_endpoint()
        {
            var fixture = new Fixture();
            var endpointId = EndpointId.Next();

            await ResetStateAsync();

            await AddEventAsync(endpointId, fixture.Build<EndpointCreated>().With(x => x.Id, endpointId).Create());
            await AddAsync(fixture.Build<EndpointInformation>().With(x => x.Id, endpointId).Create());

            var command = fixture.Build<UpdateEndpointCommand>().With(x => x.EndpointId, endpointId).Create();
            await SendAsync<UpdateEndpointCommand, EndpointId>(command);

            EventStore.Events.Count.Should().Be(2);
            Repository.Endpoints.Count.Should().Be(1);
        }
    }
}
