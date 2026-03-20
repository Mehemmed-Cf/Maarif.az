using MediatR;
using System.Security.Claims;

namespace Application.Modules.AccountsModule.Commands.PrincipalFillCommand
{
    public class PrincipalFillRequest : IRequest
    {
        public PrincipalFillRequest(ClaimsIdentity identity) => Identity = identity;

        internal ClaimsIdentity Identity { get; set; }
    }
}
