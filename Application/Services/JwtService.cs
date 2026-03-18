using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Abstracts;
using Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfigurationOptions options;
        private readonly ICryptoService cryptoService;

        public JwtService(IOptions<JwtConfigurationOptions> options, ICryptoService cryptoService)
        {
            this.options = options.Value;
            this.cryptoService = cryptoService;
        }
        public int? GetPrincipialId()
        {
            return 1;
        }

        public string GenerateAccessToken(ClaimsPrincipal principal)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, principal.Claims,

                expires: DateTime.Now.AddMinutes(options.Expires), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string GenerateRefreshToken(string accessToken)
        {
            return cryptoService.ToSha1($"202${accessToken}_code_@cademy");
        }
    }
}
