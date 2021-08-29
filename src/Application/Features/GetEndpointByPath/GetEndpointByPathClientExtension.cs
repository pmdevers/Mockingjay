using Mockingjay.Common.Http;
using Mockingjay.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public static class GetEndpointByPathClientExtension
    {
        public static async Task<EndpointInformation> GetEndpointByPath(this MockingjayClient client, string path, string method, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await client.HttpClient.GetAsync(
                $"api/endpoint?path={path}&method={method}",
                cancellationToken);

            return await client.HandleResponseAsync<EndpointInformation>(response, cancellationToken);
        }
    }
}
