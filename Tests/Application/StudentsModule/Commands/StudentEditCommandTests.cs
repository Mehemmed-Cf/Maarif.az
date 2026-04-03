using Application.Modules.StudentsModule.Commands.StudentEditCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentEditCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly StudentEditRequestHandler _handler;

        public StudentEditCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new StudentEditRequestHandler(
                _studentRepositoryMock,
                _departmentRepositoryMock,
                _mapperMock);
        }

        [Fact]
        public async Task Handle_StudentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new StudentEditRequest { Id = 1 };
            _studentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Student, bool>>>(), Arg.Any<CancellationToken>())
                .Returns((Student)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Tələbə tapılmadı*");
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            // Arrange
            var request = new StudentEditRequest { Id = 1, DepartmentId = 1 };
            var existingStudent = new Student { Id = 1, DepartmentId = 1 };
            var responseDto = new StudentEditResponseDto { Id = 1 };

            _studentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Student, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingStudent);

            _mapperMock.Map<StudentEditResponseDto>(existingStudent).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(responseDto);
            _mapperMock.Received(1).Map(request, existingStudent);
            await _studentRepositoryMock.Received(1).EditAsync(existingStudent);
            await _studentRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_DepartmentChanged_ChecksDepartmentThrowsNotFound()
        {
            // Arrange
            var request = new StudentEditRequest { Id = 1, DepartmentId = 2 };
            var existingStudent = new Student { Id = 1, DepartmentId = 1 };

            _studentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Student, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingStudent);

            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Throws(new DirectoryNotFoundException());

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DirectoryNotFoundException>();
        }
    }
}
