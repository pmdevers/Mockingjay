using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Swashbuckle.AspNetCore.Annotations;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class CreateEndpointController : ApiControllerBase
    {
        [HttpPost("endpoint")]
        [Produces(typeof(EndpointId))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> CreateEndpoint(
            [FromBody] CreateEndpointCommand cmd,
            CancellationToken cancellationToken = default)
        {
            return Ok(await CommandProcessor.SendAsync<CreateEndpointCommand, EndpointId>(cmd, cancellationToken));
        }
    }
}
