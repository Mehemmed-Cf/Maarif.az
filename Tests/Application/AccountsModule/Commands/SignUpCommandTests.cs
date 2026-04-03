using Application.Modules.AccountsModule.Commands.SignUpCommand;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using System.Security.Claims;

namespace Tests.Application.AccountsModule.Commands
{
    public class SignUpCommandTests
    {
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly IHttpContextAccessor _httpContextAccessorMock;
        private readonly SignUpRequestHandler _handler;

        public SignUpCommandTests()
        {
            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);
            _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

            _handler = new SignUpRequestHandler(_userManagerMock, _httpContextAccessorMock);
        }

        [Fact]
        public async Task Handle_Success_ReturnsClaimsPrincipal()
        {
            var request = new SignUpRequest { Email = "test@test.com", Username = "user", Password = "pw" };
            
            _userManagerMock.CreateAsync(Arg.Any<AppUser>(), "pw").Returns(IdentityResult.Success);

            var httpContext = new DefaultHttpContext();
            var authServiceMock = Substitute.For<IAuthenticationService>();
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(authServiceMock)
                .BuildServiceProvider();

            _httpContextAccessorMock.HttpContext.Returns(httpContext);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeOfType<ClaimsPrincipal>();
            await authServiceMock.Received(1).SignInAsync(httpContext, Arg.Any<string>(), Arg.Any<ClaimsPrincipal>(), Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task Handle_FailedCreation_ThrowsException()
        {
            var request = new SignUpRequest { Email = "test@test.com", Username = "user", Password = "pw" };
            
            _userManagerMock.CreateAsync(Arg.Any<AppUser>(), "pw").Returns(IdentityResult.Failed(new IdentityError { Description = "Error occurred" }));

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("*Error occurred*");
        }
    }
}
