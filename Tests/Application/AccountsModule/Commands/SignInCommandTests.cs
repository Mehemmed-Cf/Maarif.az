using Application.Modules.AccountsModule.Commands.SignInCommand;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Application.AccountsModule.Commands
{
    public class SignInCommandTests
    {
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly SignInManager<AppUser> _signInManagerMock;
        private readonly IHttpContextAccessor _httpContextAccessorMock;
        private readonly SignInRequestHandler _handler;

        public SignInCommandTests()
        {
            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            var contextAccessor = Substitute.For<IHttpContextAccessor>();
            var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManagerMock = Substitute.For<SignInManager<AppUser>>(_userManagerMock, contextAccessor, claimsFactory, null, null, null, null);

            _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

            _handler = new SignInRequestHandler(_userManagerMock, _signInManagerMock, _httpContextAccessorMock);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
        {
            var request = new SignInRequest { Email = "test@test.com", Password = "pw" };
            _userManagerMock.FindByEmailAsync("test@test.com").Returns((AppUser)null);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task Handle_IncorrectPassword_ThrowsUserNotFoundException()
        {
            var request = new SignInRequest { Email = "test@test.com", Password = "wrongpw" };
            var hash = new PasswordHasher<AppUser>().HashPassword(null, "correctpw");
            var user = new AppUser { Email = "test@test.com", PasswordHash = hash };

            _userManagerMock.FindByEmailAsync("test@test.com").Returns(user);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidCredentials_Success()
        {
            var request = new SignInRequest { Email = "test@test.com", Password = "correctpw" };
            var hash = new PasswordHasher<AppUser>().HashPassword(null, "correctpw");
            var user = new AppUser { Id = 1, Email = "test@test.com", PasswordHash = hash };

            _userManagerMock.FindByEmailAsync("test@test.com").Returns(user);

            var httpContext = new DefaultHttpContext();
            var authServiceMock = Substitute.For<IAuthenticationService>();
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(authServiceMock)
                .BuildServiceProvider();

            _httpContextAccessorMock.HttpContext.Returns(httpContext);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeOfType<ClaimsPrincipal>();
            result.HasClaim(c => c.Type == ClaimTypes.Email && c.Value == "test@test.com").Should().BeTrue();
            await authServiceMock.Received(1).SignInAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme, Arg.Any<ClaimsPrincipal>(), Arg.Any<AuthenticationProperties>());
        }
    }
}
