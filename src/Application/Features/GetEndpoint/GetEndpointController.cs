using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Mockingjay.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace Mockingjay.Features.GetEndpoint
{
    public class GetEndpointController : ApiControllerBase
    {
        [HttpGet("endpoint")]
        [Produces(typeof(EndpointId))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> GetEndpoint([FromQuery]GetEndpointCommand cmd, CancellationToken cancellationToken)
        {
            return Ok(await CommandProcessor.SendAsync<GetEndpointCommand, EndpointInformation>(cmd, cancellationToken));
        }
    }
}
