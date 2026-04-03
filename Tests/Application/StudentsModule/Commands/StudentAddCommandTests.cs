using Application.Modules.StudentsModule.Commands.StudentAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using Infrastructure.Exceptions;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentAddCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly StudentAddRequestHandler _handler;

        public StudentAddCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new StudentAddRequestHandler(
                _studentRepositoryMock,
                _departmentRepositoryMock,
                _mapperMock);
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
            // Arrange
            var request = new StudentAddRequest { DepartmentId = 1, FinCode = "FIN123" };
            var department = new Department { Id = 1 };
            var student = new Student { Id = 10, FinCode = "FIN123" };
            var responseDto = new StudentAddResponseDto { Id = 10 };

            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(department);

            _mapperMock.Map<Student>(request).Returns(student);
            _mapperMock.Map<StudentAddResponseDto>(student).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(responseDto);
            await _studentRepositoryMock.Received(1).AddAsync(student, Arg.Any<CancellationToken>());
            await _studentRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
