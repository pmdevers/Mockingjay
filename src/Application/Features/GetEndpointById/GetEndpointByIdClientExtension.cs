using Mockingjay.Common.Http;
using Mockingjay.Entities;
using System.Threading;
using System.Threading.Tasks;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;


namespace Mockingjay.Features
{
    public static class GetEndpointByIdClientExtension
    {
        public static async Task<EndpointInformation> GetEndpointById(this MockingjayClient client, EndpointId endpointId, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await client.HttpClient.GetAsync(
                $"api/endpoint/{endpointId}",
                cancellationToken);

            return await client.HandleResponseAsync<EndpointInformation>(response, cancellationToken);
        }
    }
}
