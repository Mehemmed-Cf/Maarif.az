using Application.Modules.AccountsModule.Commands.SignOutCommand;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Tests.Application.AccountsModule.Commands
{
    public class SignOutCommandTests
    {
        private readonly SignInManager<AppUser> _signInManagerMock;
        private readonly IHttpContextAccessor _httpContextAccessorMock;
        private readonly SignOutRequestHandler _handler;

        public SignOutCommandTests()
        {
            var store = Substitute.For<IUserStore<AppUser>>();
            var userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);
            var contextAccessor = Substitute.For<IHttpContextAccessor>();
            var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<AppUser>>();
            
            _signInManagerMock = Substitute.For<SignInManager<AppUser>>(userManagerMock, contextAccessor, claimsFactory, null, null, null, null);
            _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

            _handler = new SignOutRequestHandler(_signInManagerMock, _httpContextAccessorMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var httpContext = new DefaultHttpContext();
            var authServiceMock = Substitute.For<IAuthenticationService>();
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(authServiceMock)
                .BuildServiceProvider();

            _httpContextAccessorMock.HttpContext.Returns(httpContext);

            var result = await _handler.Handle(new SignOutRequest(), CancellationToken.None);

            result.Should().BeTrue();
            await authServiceMock.Received(1).SignOutAsync(httpContext, Arg.Any<string>(), Arg.Any<AuthenticationProperties>());
        }
    }
}
