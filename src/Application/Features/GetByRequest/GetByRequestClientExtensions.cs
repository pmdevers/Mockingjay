using Microsoft.AspNetCore.Http;
using Mockingjay.Common.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public static class GetByRequestClientExtensions
    {
        public static async Task<GetByRequestResponse> GetByRequestAsync(
            this MockingjayClient client,
            string path,
            HttpStatusCode statusCode,
            IQueryCollection query,
            CancellationToken cancellationToken = default)
        {
            Guard.NotNull(client, nameof(client));
            cancellationToken.ThrowIfCancellationRequested();

            var response = await client.HttpClient.GetAsync(
                $"api/endpoint",
                cancellationToken);

            return await client.HandleResponseAsync<GetByRequestResponse>(response, cancellationToken);
        }
    }
}
