using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Modules.AccountsModule.Commands.TokenRefreshCommand
{
    public class TokenRefreshRequest : IRequest<TokenRefreshRequestDto>
    {
        [FromHeader]
        public string RefreshToken { get; set; }
    }
}
