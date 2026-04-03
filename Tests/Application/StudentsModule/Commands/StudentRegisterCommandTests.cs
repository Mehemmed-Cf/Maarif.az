using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Application.Repositories;
using Application.Services;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentRegisterCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IGovernmentIdentityService _governmentIdentityServiceMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly StudentRegisterRequestHandler _handler;

        public StudentRegisterCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _governmentIdentityServiceMock = Substitute.For<IGovernmentIdentityService>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();

            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            _handler = new StudentRegisterRequestHandler(
                _studentRepositoryMock,
                _governmentIdentityServiceMock,
                _departmentRepositoryMock,
                _userManagerMock);
        }

        [Fact]
        public async Task Handle_InvalidFin_ThrowsBadRequestException()
        {
            var request = new StudentRegisterRequest { SerialNumber = "AZ12345", FinCode = "INVALID" };
            _governmentIdentityServiceMock.VerifyAsync("AZ12345", "INVALID", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<Domain.Models.ValueObjects.FinData>(null));

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task Handle_AlreadyRegistered_ThrowsConflictException()
        {
            var request = new StudentRegisterRequest { SerialNumber = "AZ12345", FinCode = "123FIN" };
            _governmentIdentityServiceMock.VerifyAsync(request.SerialNumber, request.FinCode, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new Domain.Models.ValueObjects.FinData { FullName = "Test Test" }));
                
            _studentRepositoryMock.GetByFinCodeAsync("123FIN", Arg.Any<CancellationToken>())
                .Returns(new Student { FinCode = "123FIN" });

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>();
        }
        
    }
}
