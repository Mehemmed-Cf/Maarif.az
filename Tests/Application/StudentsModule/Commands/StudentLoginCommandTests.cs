using Application.Modules.StudentsModule.Commands.StudentLoginCommand;
using Application.Repositories;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security.Claims;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentLoginCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly SignInManager<AppUser> _signInManagerMock;
        private readonly StudentLoginRequestHandler _handler;

        public StudentLoginCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();

            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            var contextAccessor = Substitute.For<IHttpContextAccessor>();
            var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManagerMock = Substitute.For<SignInManager<AppUser>>(_userManagerMock, contextAccessor, claimsFactory, null, null, null, null);

            _handler = new StudentLoginRequestHandler(
                _studentRepositoryMock,
                _userManagerMock,
                _signInManagerMock);
        }

        [Fact]
        public async Task Handle_StudentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new StudentLoginRequest { StudentNumber = "9999", Password = "password" };
            _studentRepositoryMock
                .GetByStudentNumberAsync("9999", Arg.Any<CancellationToken>())
                .Returns((Student)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*qeydiyyat tapılmadı*");
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new StudentLoginRequest { StudentNumber = "FIN123", Password = "password" };
            var studentId = 1;
            var student = new Student { UserId = studentId, StudentNumber = "FIN123" };

            _studentRepositoryMock
                .GetByStudentNumberAsync("FIN123", Arg.Any<CancellationToken>())
                .Returns(student);

            _userManagerMock.FindByIdAsync(studentId.ToString()).Returns((AppUser)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*İstifadəçi tapılmadı*");
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var request = new StudentLoginRequest { StudentNumber = "FIN123", Password = "wrongpass" };
            var studentId = 1;
            var student = new Student { UserId = studentId, StudentNumber = "FIN123" };
            var appUser = new AppUser { Id = studentId };

            _studentRepositoryMock.GetByStudentNumberAsync("FIN123", Arg.Any<CancellationToken>()).Returns(student);
            _userManagerMock.FindByIdAsync(studentId.ToString()).Returns(appUser);
            _userManagerMock.CheckPasswordAsync(appUser, "wrongpass").Returns(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage("*Şifrə yanlışdır*");
        }

        [Fact]
        public async Task Handle_ValidCredentials_Success()
        {
            // Arrange
            var request = new StudentLoginRequest { StudentNumber = "FIN123", Password = "password" };
            var studentId = 1;
            var student = new Student { UserId = studentId, StudentNumber = "FIN123", FullName = "John Doe" };
            var appUser = new AppUser { Id = studentId };

            _studentRepositoryMock.GetByStudentNumberAsync("FIN123", Arg.Any<CancellationToken>()).Returns(student);
            _userManagerMock.FindByIdAsync(studentId.ToString()).Returns(appUser);
            _userManagerMock.CheckPasswordAsync(appUser, "password").Returns(true);
            _userManagerMock.GetRolesAsync(appUser).Returns(new List<string> { "STUDENT" });
            _signInManagerMock.SignInAsync(appUser, false).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StudentNumber.Should().Be("FIN123");
            result.FullName.Should().Be("John Doe");
            result.Role.Should().Be("STUDENT");

            await _signInManagerMock.Received(1).SignInAsync(appUser, false);
        }
    }
}
