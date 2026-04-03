using Application.Modules.TeachersModule.Commands.TeacherEditCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using Application.Modules.TeachersModule;

namespace Tests.Application.TeachersModule.Commands
{
    public class TeacherEditCommandTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly TeacherEditRequestHandler _handler;

        public TeacherEditCommandTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new TeacherEditRequestHandler(_teacherRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new TeacherEditRequest { Id = 1, DepartmentIds = new List<int> { 3, 4 } };
            
            var existingTeacher = new Teacher 
            { 
                Id = 1, 
                TeacherDepartments = new List<TeacherDepartment> { new TeacherDepartment { DepartmentId = 1 } }
            };
            
            var sourceList = new List<Teacher> { existingTeacher };
            var mockQueryable = sourceList.AsQueryable().BuildMock();

            _teacherRepositoryMock.GetAll().Returns(mockQueryable);

            var responseDto = new TeacherResponseDto { Id = 1 };
            _mapperMock.Map<TeacherResponseDto>(existingTeacher).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            existingTeacher.TeacherDepartments.Should().HaveCount(2);
            existingTeacher.TeacherDepartments.Any(td => td.DepartmentId == 3).Should().BeTrue();
            await _teacherRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
