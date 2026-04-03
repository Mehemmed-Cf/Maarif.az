using Application.Modules.TeachersModule.Queries.TeacherGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using Application.Modules.TeachersModule;

namespace Tests.Application.TeachersModule.Queries
{
    public class TeacherGetByIdQueryTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly TeacherGetByIdRequestHandler _handler;

        public TeacherGetByIdQueryTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new TeacherGetByIdRequestHandler(_teacherRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new TeacherGetByIdRequest { Id = 1 };
            var existingTeacher = new Teacher { Id = 1 };
            var sourceList = new List<Teacher> { existingTeacher };
            var mockQueryable = sourceList.AsQueryable().BuildMock();

            _teacherRepositoryMock.GetAll().Returns(mockQueryable);

            var responseDto = new TeacherResponseDto { Id = 1 };
            _mapperMock.Map<TeacherResponseDto>(existingTeacher).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
        }
    }
}
