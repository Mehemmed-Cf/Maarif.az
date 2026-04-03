using Application.Modules.StudentsModule.Commands.StudentRemoveCommand;
using Application.Repositories;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.StudentsModule.Commands
{
    public class StudentRemoveCommandTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly StudentRemoveRequestHandler _handler;

        public StudentRemoveCommandTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _handler = new StudentRemoveRequestHandler(_studentRepositoryMock);
        }

        [Fact]
        public async Task Handle_StudentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new StudentRemoveRequest { Id = 1 };
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
            var request = new StudentRemoveRequest { Id = 1 };
            var existingStudent = new Student { Id = 1 };

            _studentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Student, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingStudent);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _studentRepositoryMock.Received(1).Remove(existingStudent);
            await _studentRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
