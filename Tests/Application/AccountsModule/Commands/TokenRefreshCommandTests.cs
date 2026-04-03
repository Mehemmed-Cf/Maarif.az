using Application.Modules.AccountsModule.Commands.TokenRefreshCommand;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Infrastructure.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Security.Claims;
using Xunit;
using System.IdentityModel.Tokens.Jwt;

namespace Tests.Application.AccountsModule.Commands
{
    public class TokenRefreshCommandTests
    {
        private readonly IJwtService _jwtServiceMock;
        private readonly IHttpContextAccessor _httpContextAccessorMock;
        private readonly DbContext _dbContextMock;
        private readonly TokenRefreshRequestHandler _handler;

        public TokenRefreshCommandTests()
        {
            _jwtServiceMock = Substitute.For<IJwtService>();
            _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            _dbContextMock = Substitute.For<DbContext>();

            _handler = new TokenRefreshRequestHandler(_jwtServiceMock, _httpContextAccessorMock, _dbContextMock);
        }

        [Fact]
        public async Task Handle_NoAuthHeader_ThrowsUnauthorizedAccessException()
        {
            var request = new TokenRefreshRequest { RefreshToken = "refreshtoken" };
            
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.HttpContext.Returns(httpContext);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
