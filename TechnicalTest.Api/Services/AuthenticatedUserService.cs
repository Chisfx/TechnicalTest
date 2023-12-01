using System.Security.Claims;
using TechnicalTest.Application.Interfaces.Shared;

namespace TechnicalTest.Api.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor?.HttpContext?.User?.FindFirstValue("uid");
        }

        public string? UserId { get; }
    }
}
