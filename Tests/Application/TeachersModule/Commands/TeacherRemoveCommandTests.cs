using Application.Modules.TeachersModule.Commands.TeacherRemoveCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.TeachersModule.Commands
{
    public class TeacherRemoveCommandTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly TeacherRemoveRequestHandler _handler;

        public TeacherRemoveCommandTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new TeacherRemoveRequestHandler(_teacherRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new TeacherRemoveRequest { Id = 1 };
            var teacher = new Teacher { Id = 1 };

            _teacherRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Teacher, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(teacher);

            await _handler.Handle(request, CancellationToken.None);

            _teacherRepositoryMock.Received(1).Remove(teacher);
            await _teacherRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
