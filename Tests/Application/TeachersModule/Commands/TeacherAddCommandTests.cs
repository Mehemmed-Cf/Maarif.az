using Application.Modules.TeachersModule;
using Application.Modules.TeachersModule.Commands.TeacherAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace Tests.Application.TeachersModule.Commands
{
    public class TeacherAddCommandTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly UserManager<AppUser> _userManagerMock;
        private readonly TeacherAddRequestHandler _handler;

        public TeacherAddCommandTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();
            _mapperMock = Substitute.For<IMapper>();

            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            _teacherRepositoryMock.GetAll().Returns(Enumerable.Empty<Teacher>().AsQueryable());

            _handler = new TeacherAddRequestHandler(_teacherRepositoryMock, _mapperMock, _userManagerMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new TeacherAddRequest
            {
                Email = "teacher@test.edu",
                DepartmentIds = new List<int> { 1, 2 }
            };
            var teacher = new Teacher { Id = 10 };
            var responseDto = new TeacherResponseDto { Id = 10 };

            _userManagerMock.FindByEmailAsync("teacher@test.edu").Returns((AppUser?)null);
            _userManagerMock.FindByNameAsync(Arg.Any<string>()).Returns((AppUser?)null);
            _userManagerMock.CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .Returns(ci =>
                {
                    ci.ArgAt<AppUser>(0).Id = 88;
                    return Task.FromResult(IdentityResult.Success);
                });
            _userManagerMock.AddToRoleAsync(Arg.Any<AppUser>(), "TEACHER")
                .Returns(Task.FromResult(IdentityResult.Success));

            _mapperMock.Map<Teacher>(request).Returns(teacher);
            _mapperMock.Map<TeacherResponseDto>(teacher).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            teacher.TeacherDepartments.Should().HaveCount(2);
            teacher.UserId.Should().Be(88);
            teacher.TeacherNumber.Should().NotBeNullOrEmpty();
            teacher.Email.Should().Be("teacher@test.edu");
            await _teacherRepositoryMock.Received(1).AddAsync(teacher, Arg.Any<CancellationToken>());
            await _teacherRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
