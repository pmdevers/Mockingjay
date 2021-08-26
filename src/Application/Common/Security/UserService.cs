using System.Security.Claims;

namespace Application.Common.Security
{
    public interface IUserService
    {
        ClaimsPrincipal CurrentUser { get; }
    }
}
