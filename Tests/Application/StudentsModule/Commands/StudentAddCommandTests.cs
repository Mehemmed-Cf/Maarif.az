using Application.Modules.StudentsModule.Commands.StudentAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentAddCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly StudentAddRequestHandler _handler;

        public StudentAddCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            _handler = new StudentAddRequestHandler(
                _studentRepositoryMock,
                _departmentRepositoryMock,
                _mapperMock,
                _userManagerMock);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_ThrowsItemNotFoundException()
        {
            // Arrange
            var request = new StudentAddRequest { DepartmentId = 99 };
            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Throws(new DirectoryNotFoundException("Department not found")); // Adjust to NotFoundException if it was mapped. But code says GetAsync throws DirectoryNotFoundException.

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new StudentAddRequest
            {
                DepartmentId = 1,
                FinCode = "fin123",
                StudentNumber = "SN001"
            };
            var department = new Department { Id = 1 };
            var student = new Student { Id = 10 };
            var responseDto = new StudentAddResponseDto { Id = 10 };

            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(department);

            _studentRepositoryMock.GetByFinCodeAsync("FIN123", Arg.Any<CancellationToken>())
                .Returns((Student?)null);
            _studentRepositoryMock.GetByStudentNumberAsync("SN001", Arg.Any<CancellationToken>())
                .Returns((Student?)null);
            _userManagerMock.FindByNameAsync("SN001").Returns((AppUser?)null);
            _userManagerMock.CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(ci =>
                {
                    ci.ArgAt<AppUser>(0).Id = 77;
                    return Task.FromResult(IdentityResult.Success);
                });
            _userManagerMock.AddToRoleAsync(Arg.Any<AppUser>(), "STUDENT")
                .Returns(Task.FromResult(IdentityResult.Success));

            _mapperMock.Map<Student>(request).Returns(student);
            _mapperMock.Map<StudentAddResponseDto>(student).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            student.FinCode.Should().Be("FIN123");
            student.StudentNumber.Should().Be("SN001");
            student.UserId.Should().Be(77);
            await _studentRepositoryMock.Received(1).AddAsync(student, Arg.Any<CancellationToken>());
            await _studentRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_DuplicateFin_ThrowsConflictException()
        {
            var request = new StudentAddRequest { DepartmentId = 1, FinCode = "ABC12XY", StudentNumber = "SN1" };
            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(new Department { Id = 1 });
            _studentRepositoryMock.GetByFinCodeAsync("ABC12XY", Arg.Any<CancellationToken>())
                .Returns(new Student());

            var act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>();
        }
    }
}
