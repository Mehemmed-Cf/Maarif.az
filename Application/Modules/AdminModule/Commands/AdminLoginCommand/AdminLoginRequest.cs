using Domain.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.AdminModule.Commands.AdminLoginCommand
{
    public class AdminLoginRequest : IRequest<AdminLoginResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }

    public class AdminLoginResponse
    {
        public bool Succeeded { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class AdminLoginRequestHandler : IRequestHandler<AdminLoginRequest, AdminLoginResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AdminLoginRequestHandler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<AdminLoginResponse> Handle(
            AdminLoginRequest request,
            CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return new AdminLoginResponse
                {
                    Succeeded = false,
                    ErrorMessage = "İstifadəçi adı və ya şifrə yanlışdır."
                };

            var isAdmin = await userManager.IsInRoleAsync(user, "SUPERADMIN");

            if (!isAdmin)
                return new AdminLoginResponse
                {
                    Succeeded = false,
                    ErrorMessage = "Bu səhifəyə giriş üçün icazəniz yoxdur."
                };

            var result = await signInManager.PasswordSignInAsync(
                user,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: true);

            if (!result.Succeeded)
                return new AdminLoginResponse
                {
                    Succeeded = false,
                    ErrorMessage = "İstifadəçi adı və ya şifrə yanlışdır."
                };

            return new AdminLoginResponse { Succeeded = true };
        }
    }
}
