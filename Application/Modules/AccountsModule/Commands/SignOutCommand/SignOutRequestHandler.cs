using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Entities.Membership;

namespace Application.Modules.AccountsModule.Commands.SignOutCommand
{
    public class SignOutRequestHandler : IRequestHandler<SignOutRequest, bool>
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SignOutRequestHandler(SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(SignOutRequest request, CancellationToken cancellationToken)
        {
            await httpContextAccessor.HttpContext.SignOutAsync();

            return true;
        }
    }
}
