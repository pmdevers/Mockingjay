using Mockingjay.Common.Http;
using Mockingjay.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoint
{
    public static class GetEndpointClientExtension
    {
        public static async Task<EndpointInformation> GetEndpointByRequest(this MockingjayClient client, GetEndpointCommand request, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await client.HttpClient.GetAsync(
                $"api/endpoint?path={request.Path}&method={request.Method}",
                cancellationToken);

            return await client.HandleResponseAsync<EndpointInformation>(response, cancellationToken);
        }
    }
}
