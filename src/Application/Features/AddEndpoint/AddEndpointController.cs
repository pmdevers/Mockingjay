using System.Threading;
using System.Threading.Tasks;

using Application.Common.Http;
using Application.Common.Identifiers;
using Application.ValueObjects;

using Microsoft.AspNetCore.Mvc;

namespace Application.Features.AddEndpoint
{
    public class AddEndpointController : ApiControllerBase
    {
        [HttpPost("endpoint")]
        [Produces(typeof(Id<ForMockEndpoint>))]
        //[SwaggerOperation(Tags = new[] { "Dossier" })]
        public async Task<IActionResult> AddEndpoint(
            [FromBody] AddEndpointCommand cmd,
            CancellationToken cancellationToken = default)
        {
            return Ok(await CommandProcessor.SendAsync<AddEndpointCommand, Id<ForMockEndpoint>>(cmd, cancellationToken));
        }
    }
}
