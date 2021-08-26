using Mockingjay.Common.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay
{
    public class MockingjayClient : IMockingjayClient
    {
        public MockingjayClient(HttpClient httpClient, IOptions<JsonSerializerOptions> jsonOptions)
        {
            HttpClient = httpClient;
            Options = jsonOptions.Value;
        }

        public HttpClient HttpClient { get; }
        public ILogger<MockingjayClient> Logger { get; }
        public JsonSerializerOptions Options { get; }

        public async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await HandleErrorAsync(response, cancellationToken);

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<T>(stream, Options, cancellationToken);
            return result;
        }

        public async Task HandleErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var sr = new StreamReader(stream);
                var message = await sr.ReadToEndAsync();
                var exception = new MockingjayClientException(response.StatusCode, message);
                Logger.LogError((int)response.StatusCode, exception, message);
                throw exception;
            }
        }
    }
}
