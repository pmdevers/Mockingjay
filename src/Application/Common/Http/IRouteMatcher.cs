using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Mockingjay.Common.Http
{
    public interface IRouteMatcher
    {
        RouteValueDictionary Match(string routeTemplate, string requestPath, IQueryCollection query);
    }
}
