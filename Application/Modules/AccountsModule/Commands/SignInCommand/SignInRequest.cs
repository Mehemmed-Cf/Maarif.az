using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Application.Modules.AccountsModule.Commands.SignInCommand
{
    public class SignInRequest : IRequest<ClaimsPrincipal>
    {
        [Required]
        public string Email { get; set; } // Username
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
