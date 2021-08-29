using Microsoft.AspNetCore.Mvc;
using Mockingjay.Common.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Features
{
    public class GetEndpointsController : ApiControllerBase
    {
        [HttpGet("endpoints")]
        [Produces(typeof(GetEndpointsResponse))]
        [SwaggerOperation(Tags = new[] { "Endpoints" })]
        public async Task<IActionResult> GetEndpoints([FromQuery] GetEndpointsCommand cmd, CancellationToken cancellationToken)
        {
            return Ok(await CommandProcessor.SendAsync<GetEndpointsCommand, GetEndpointsResponse>(cmd, cancellationToken));
        }
    }
}
