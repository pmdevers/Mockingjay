using Microsoft.AspNetCore.Http;
using Mockingjay.Common.Handling;

namespace Mockingjay.Features
{
    public class GetByRequestCommand : ICommand<GetByRequestResponse>
    {
        public string Path { get;set;}
        public string Method { get; set; }
        public IQueryCollection Query { get; set; }
    }
}
