using Microsoft.AspNetCore.Routing;
using Mockingjay.Entities;

namespace Mockingjay.Features
{
    public class GetByRequestResponse
    {
        public GetByRequestResponse(Endpoint endpoint, RouteValueDictionary routeValues)
        {
            Endpoint = endpoint;
            RouteValues = routeValues;
        }

        public Endpoint Endpoint { get; }
        public RouteValueDictionary RouteValues { get; }
    }
}
