using Application.Modules.SubjectsModule.Commands.SubjectAddCommand;
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
    public class SubjectAddCommandTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly IDepartmentRepository _departmentRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly SubjectAddRequestHandler _handler;

        public SubjectAddCommandTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new SubjectAddRequestHandler(
                _subjectRepositoryMock,
                _departmentRepositoryMock,
                _mapperMock);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_ThrowsException()
        {
            var request = new SubjectAddRequest { DepartmentId = 99 };
            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Throws(new DirectoryNotFoundException());

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new SubjectAddRequest { Name = "Math", DepartmentId = 1 };
            var department = new Department { Id = 1 };
            
            // To capture the entity passed to AddAsync so we can set its ID for GetByIdWithDetailsAsync
            _subjectRepositoryMock.When(x => x.AddAsync(Arg.Any<Subject>(), Arg.Any<CancellationToken>()))
                .Do(info => { var sub = info.Arg<Subject>(); sub.Id = 10; });

            _departmentRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(department);

            var createdSubject = new Subject { Id = 10, Name = "Math" };
            _subjectRepositoryMock.GetByIdWithDetailsAsync(10, Arg.Any<CancellationToken>()).Returns(createdSubject);

            var responseDto = new SubjectAddResponseDto { Id = 10 };
            _mapperMock.Map<SubjectAddResponseDto>(createdSubject).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
            await _subjectRepositoryMock.Received(1).AddAsync(Arg.Any<Subject>(), Arg.Any<CancellationToken>());
            await _subjectRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
