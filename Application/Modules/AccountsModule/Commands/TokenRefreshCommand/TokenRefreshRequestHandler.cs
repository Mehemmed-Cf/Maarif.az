using Infrastructure.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Domain.Models.Entities.Membership;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Modules.AccountsModule.Commands.TokenRefreshCommand
{
    public class TokenRefreshRequestHandler : IRequestHandler<TokenRefreshRequest, TokenRefreshRequestDto>
    {
        private readonly IJwtService jwtService;
        private readonly IHttpContextAccessor ctx;
        private readonly DbContext db;

        public TokenRefreshRequestHandler(IJwtService jwtService, IHttpContextAccessor ctx, DbContext db)
        {
            this.jwtService = jwtService;
            this.ctx = ctx;
            this.db = db;
        }

        public async Task<TokenRefreshRequestDto> Handle(TokenRefreshRequest request, CancellationToken cancellationToken)
        {
            if (!ctx.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues accessTokenPairs))
                throw new UnauthorizedAccessException();

            string accessToken = accessTokenPairs.FirstOrDefault()?.Replace("Bearer ", string.Empty);

            if (!jwtService.GenerateRefreshToken(accessToken).Equals(request.RefreshToken))
                throw new UnauthorizedAccessException();

            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            var userId = Convert.ToInt32(token.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier)).Value);

            var table = db.Set<AppUserToken>();

            var record = table.FirstOrDefault(m =>
            m.IsActive
            && m.Expired > DateTime.UtcNow
            && m.UserId == userId
            && m.Name.Equals(request.RefreshToken)
            && m.LoginProvider.Equals("REFRESH_TOKEN"));

            if (record is null)
                throw new UnauthorizedAccessException();

            var user = await db.Set<AppUser>().FirstOrDefaultAsync(m => m.Id == record.UserId, cancellationToken);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var response = new TokenRefreshRequestDto
            {
                AccessToken = jwtService.GenerateAccessToken(new ClaimsPrincipal(claimsIdentity))
            };

            response.RefreshToken = jwtService.GenerateRefreshToken(response.AccessToken);

            record.IsActive = false;
            await db.SaveChangesAsync(cancellationToken);

            var tokenRecord = new AppUserToken
            {
                UserId = record.UserId,
                LoginProvider = "REFRESH_TOKEN",
                Name = response.RefreshToken,
                Value = "REFRESH_TOKEN",
                IsActive = true,
                Expired = DateTime.UtcNow.AddDays(1),
            };

            table.Add(tokenRecord);
            await db.SaveChangesAsync();

            return response;
        }
    }
}
