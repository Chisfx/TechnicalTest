using AspNetCoreHero.Results;
using System.Threading.Tasks;
using TechnicalTest.Application.DTOs;
namespace TechnicalTest.Application.Interfaces.Repositories
{
    public interface IIdentityService
    {
        Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);
        Task<Result<string>> RegisterAsync(RegisterRequest request, bool ValidateExists = true);
    }
}
