using Mockingjay.Common.Security;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class UserService : IUserService
    {
        public ClaimsPrincipal CurrentUser => new ClaimsPrincipal();
    }
}
