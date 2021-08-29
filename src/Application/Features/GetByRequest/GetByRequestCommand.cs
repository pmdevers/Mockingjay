using Microsoft.AspNetCore.Http;
using Mockingjay.Common.Handling;
using System.Collections.Generic;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;


namespace Mockingjay.Features
{
    public class GetByRequestCommand : ICommand<Endpoint>
    {
        public string Path { get;set;}
        public string Method { get; set; }
        public IQueryCollection Query { get; set; }
    }
}
