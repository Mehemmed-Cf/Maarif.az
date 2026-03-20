using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Domain.Models.Entities.Membership;

namespace Application.Modules.AccountsModule.Commands.SignInCommand
{
    public class SignInRequestHandler : IRequestHandler<SignInRequest, ClaimsPrincipal>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signinManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SignInRequestHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClaimsPrincipal> Handle(SignInRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if(user is null)
                throw new UserNotFoundException();

            var hasher = new PasswordHasher<AppUser>();

            if (hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                var authenticationManager = httpContextAccessor.HttpContext;

                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                        new ClaimsPrincipal(claimsIdentity),
                                                        new AuthenticationProperties
                                                        {
                                                            IsPersistent = true,
                                                            ExpiresUtc = DateTime.UtcNow.AddDays(14) // AddMinutes(20)
                                                        });

                return new ClaimsPrincipal(claimsIdentity);
            } else
            {
                throw new UserNotFoundException();
            }
        }
    }
}
