using Application.Modules.StudentsModule.Queries.StudentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using Xunit;

namespace Tests.Application.StudentsModule.Queries
{
    public class StudentGetByIdQueryTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly StudentGetByIdRequestHandler _handler;

        public StudentGetByIdQueryTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new StudentGetByIdRequestHandler(_studentRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_NotFound_ThrowsNotFoundException()
        {
            var request = new StudentGetByIdRequest { Id = 1 };
            _studentRepositoryMock.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Returns((Student)null);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new StudentGetByIdRequest { Id = 1 };
            var student = new Student { Id = 1, FullName = "Test" };
            var responseDto = new StudentGetByIdResponseDto { Id = 1 };

            _studentRepositoryMock.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Returns(student);
            _mapperMock.Map<StudentGetByIdResponseDto>(student).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
        }
    }
}
