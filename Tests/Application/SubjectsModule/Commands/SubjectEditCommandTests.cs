using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.SubjectsModule.Commands
{
    public class SubjectEditCommandTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly SubjectEditRequestHandler _handler;

        public SubjectEditCommandTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new SubjectEditRequestHandler(
                _subjectRepositoryMock,
                _departmentRepositoryMock,
                _mapperMock);
        }

        [Fact]
        public async Task Handle_SubjectNotFound_ThrowsNotFoundException()
        {
            var request = new SubjectEditRequest { Id = 1 };
            _subjectRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Subject, bool>>>(), Arg.Any<CancellationToken>())
                .Returns((Subject)null);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Fənn tapılmadı*");
        }

        [Fact]
        public async Task Handle_DepartmentChangedButNotFound_ThrowsException()
        {
            var request = new SubjectEditRequest { Id = 1, DepartmentId = 2 };
            var existing = new Subject { Id = 1, DepartmentId = 1 };

            _subjectRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Subject, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existing);

            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Throws(new DirectoryNotFoundException());

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new SubjectEditRequest { Id = 1, Name = "Physics", DepartmentId = 1 };
            var existing = new Subject { Id = 1, Name = "Math", DepartmentId = 1 };
            var responseDto = new SubjectEditResponseDto { Id = 1 };

            _subjectRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Subject, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existing);

            _subjectRepositoryMock.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>()).Returns(existing);
            _mapperMock.Map<SubjectEditResponseDto>(existing).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            existing.Name.Should().Be("Physics");
            await _subjectRepositoryMock.Received(1).EditAsync(existing);
            await _subjectRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
