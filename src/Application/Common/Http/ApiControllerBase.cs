using Mockingjay.Common.Handling;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Mockingjay.Common.Http
{
    [Route("api")]
    //[Authorize]
    public class ApiControllerBase : ControllerBase
    {
        private ICommandProcessor _commandProcessor;

        protected ApiControllerBase() { }

        protected ICommandProcessor CommandProcessor => _commandProcessor ??=
            HttpContext.RequestServices.GetService<ICommandProcessor>();
    }
}
