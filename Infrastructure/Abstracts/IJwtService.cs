using System.Security.Claims;

namespace Infrastructure.Abstracts
{
    public interface IJwtService
    {
        int? GetPrincipialId();

        string GenerateAccessToken(ClaimsPrincipal principal);
        string GenerateRefreshToken(string accessToken);
    }
}
