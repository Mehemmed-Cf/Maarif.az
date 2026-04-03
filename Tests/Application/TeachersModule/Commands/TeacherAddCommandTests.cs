using Application.Modules.TeachersModule.Commands.TeacherAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Application.Modules.TeachersModule;

namespace Tests.Application.TeachersModule.Commands
{
    public class TeacherAddCommandTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly TeacherAddRequestHandler _handler;

        public TeacherAddCommandTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new TeacherAddRequestHandler(_teacherRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new TeacherAddRequest { DepartmentIds = new List<int> { 1, 2 } };
            var teacher = new Teacher { Id = 10 };
            var responseDto = new TeacherResponseDto { Id = 10 };

            _mapperMock.Map<Teacher>(request).Returns(teacher);
            _mapperMock.Map<TeacherResponseDto>(teacher).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            teacher.TeacherDepartments.Should().HaveCount(2);
            await _teacherRepositoryMock.Received(1).AddAsync(teacher, Arg.Any<CancellationToken>());
            await _teacherRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
