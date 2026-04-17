using Application.Modules.SubjectsModule;
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
        private readonly ISubjectTopicRepository _topicRepositoryMock;
        private readonly ISubjectMaterialRepository _materialRepositoryMock;
        private readonly ISubjectLiteratureRepository _literatureRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly SubjectEditRequestHandler _handler;

        public SubjectEditCommandTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _departmentRepositoryMock = Substitute.For<IDepartmentRepository>();
            _topicRepositoryMock = Substitute.For<ISubjectTopicRepository>();
            _materialRepositoryMock = Substitute.For<ISubjectMaterialRepository>();
            _literatureRepositoryMock = Substitute.For<ISubjectLiteratureRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new SubjectEditRequestHandler(
                _subjectRepositoryMock,
                _departmentRepositoryMock,
                _topicRepositoryMock,
                _materialRepositoryMock,
                _literatureRepositoryMock,
                _mapperMock);
        }

        [Fact]
        public async Task Handle_SubjectNotFound_ThrowsNotFoundException()
        {
            var request = new SubjectEditRequest { Id = 1 };
            _subjectRepositoryMock.GetByIdWithDetailsTrackedAsync(1, Arg.Any<CancellationToken>())
                .Returns((Subject)null!);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Fənn tapılmadı*");
        }

        [Fact]
        public async Task Handle_DepartmentChangedButNotFound_ThrowsException()
        {
            var request = new SubjectEditRequest
            {
                Id = 1,
                Name = "N",
                DepartmentId = 2,
                Term = "T",
                GroupName = "G"
            };
            var existing = new Subject
            {
                Id = 1,
                DepartmentId = 1,
                Term = "T",
                GroupName = "G",
                Topics = new List<SubjectTopic>(),
                Materials = new List<SubjectMaterial>(),
                Literatures = new List<SubjectLiterature>()
            };

            _subjectRepositoryMock.GetByIdWithDetailsTrackedAsync(1, Arg.Any<CancellationToken>())
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
            var request = new SubjectEditRequest
            {
                Id = 1,
                Name = "Physics",
                DepartmentId = 1,
                Term = "2026",
                GroupName = "G1",
                Topics = new List<SubjectTopicRowDto>(),
                Materials = new List<SubjectMaterialRowDto>(),
                Literatures = new List<SubjectLiteratureRowDto>()
            };
            var existing = new Subject
            {
                Id = 1,
                Name = "Math",
                DepartmentId = 1,
                Term = "2025",
                GroupName = "G0",
                Topics = new List<SubjectTopic>(),
                Materials = new List<SubjectMaterial>(),
                Literatures = new List<SubjectLiterature>()
            };
            var responseDto = new SubjectEditResponseDto { Id = 1 };

            _subjectRepositoryMock.GetByIdWithDetailsTrackedAsync(1, Arg.Any<CancellationToken>())
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
