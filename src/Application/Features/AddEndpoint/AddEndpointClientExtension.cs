using Mockingjay.Common.Http;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MockEndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public static class AddEndpointClientExtension
    {
        public static async Task<MockEndpointId> AddEndpointAsync(this MockingjayClient client, AddEndpointCommand request, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var content = JsonSerializer.Serialize(request, client.Options);
            var response = await client.HttpClient.PostAsync(
                "api/endpoint",
                new StringContent(content, Encoding.UTF8, "application/json"),
                cancellationToken);

            return await client.HandleResponseAsync<MockEndpointId>(response, cancellationToken);
        }
    }
}
