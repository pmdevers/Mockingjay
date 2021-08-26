using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Swashbuckle.AspNetCore.Annotations;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace Mockingjay.Features.AddEndpoint
{
    public class AddEndpointController : ApiControllerBase
    {
        [HttpPost("endpoint")]
        [Produces(typeof(EndpointId))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> AddEndpoint(
            [FromBody] AddEndpointCommand cmd,
            CancellationToken cancellationToken = default)
        {
            return Ok(await CommandProcessor.SendAsync<AddEndpointCommand, EndpointId>(cmd, cancellationToken));
        }
    }
}
