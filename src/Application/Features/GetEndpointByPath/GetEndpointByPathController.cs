using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Mockingjay.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features
{
    public class GetEndpointByPathController : ApiControllerBase
    {
        [HttpGet("endpoint")]
        [Produces(typeof(EndpointId))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> GetEndpoint([FromQuery]GetEndpointByPathCommand cmd, CancellationToken cancellationToken)
        {
            return Ok(await CommandProcessor.SendAsync<GetEndpointByPathCommand, EndpointInformation>(cmd, cancellationToken));
        }
    }
}
