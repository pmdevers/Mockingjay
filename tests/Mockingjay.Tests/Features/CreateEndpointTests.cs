using FluentAssertions;
using Mockingjay.Features;
using NUnit.Framework;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Tests.Features
{
    using static Testing;

    public class CreateEndpointTests : TestBase
    {
        [Test]
        public async Task Should_Save_EndpointCreated()
        {
            await ResetStateAsync();

            var command = new CreateEndpointCommand
            {
                Path = "/test",
                Method = "GET",
                StatusCode = System.Net.HttpStatusCode.OK,
                ContentType = "application/json",
                Content = "Test Content",
            };

            var result = await SendAsync<CreateEndpointCommand, EndpointId>(command);

            result.Should().NotBeNull();
            EventStore.Events.Count.Should().Be(1);
            Repository.Endpoints.Count.Should().Be(1);
        }
    }
}
