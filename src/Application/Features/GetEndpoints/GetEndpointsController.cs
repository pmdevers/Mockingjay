using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features.GetEndpoints
{
    public class GetEndpointsController : ApiControllerBase
    {
        [HttpGet("endpoint/q")]
        [Produces(typeof(GetEndpointsResponse))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> GetEndpoints([FromQuery] GetEndpointsCommand cmd, CancellationToken cancellationToken)
        {
            return Ok(await CommandProcessor.SendAsync<GetEndpointsCommand, GetEndpointsResponse>(cmd, cancellationToken));
        }
    }
}
