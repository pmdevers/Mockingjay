using System.Security.Claims;

namespace Mockingjay.Common.Security
{
    public interface IUserService
    {
        ClaimsPrincipal CurrentUser { get; }
    }
}
