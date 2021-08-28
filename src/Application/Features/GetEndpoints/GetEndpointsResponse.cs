using Mockingjay.Common;
using Mockingjay.Entities;
using System.Collections.Generic;

namespace Mockingjay.Features.GetEndpoints
{
    public class GetEndpointsResponse : PagedResponse<EndpointInformation>
    {
        public GetEndpointsResponse(
            IEnumerable<EndpointInformation> items,
            long currentPage,
            int resultsPerPage,
            long totalPages,
            long totalResults)
            : base(items, currentPage, resultsPerPage, totalPages, totalResults)
        {
        }
    }
}
