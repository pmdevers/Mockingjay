using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Mockingjay.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class GetEndpointByIdController : ApiControllerBase
    {
        [HttpGet("endpoint/{endpointId}")]
        [Produces(typeof(Endpoint))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> GetEndpoint([FromRoute] GetEndpointByIdCommand cmd, CancellationToken cancellationToken)
        {
            return Ok(await CommandProcessor.SendAsync<GetEndpointByIdCommand, Endpoint>(cmd, cancellationToken));
        }
    }
}
