using Mockingjay.Common.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public static class GetEndpointsClientExtensions
    {
        public static async Task<GetEndpointsResponse> GetEndpointsAsync(this MockingjayClient client, int page, int itemsPerPage, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await client.HttpClient.GetAsync(
                $"api/endpoint/q?page={page}&itemsPerPage={itemsPerPage}",
                cancellationToken);

            return await client.HandleResponseAsync<GetEndpointsResponse>(response, cancellationToken);
        }
    }
}
