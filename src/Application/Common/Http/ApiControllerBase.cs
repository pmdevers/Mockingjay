using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Mockingjay.Common.Handling;

namespace Mockingjay.Common.Http
{
    [ApiController]
    //[Authorize]
    public class ApiControllerBase : ControllerBase
    {
        private ICommandProcessor _commandProcessor;

        protected ApiControllerBase() { }

        protected ICommandProcessor CommandProcessor => _commandProcessor ??=
            HttpContext.RequestServices.GetService<ICommandProcessor>();
    }
}
