using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Application.Modules.AccountsModule.Commands.SignUpCommand
{
    public class SignUpRequest : IRequest<ClaimsPrincipal>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")] //6
        public string Password { get; set; }
    }
}
