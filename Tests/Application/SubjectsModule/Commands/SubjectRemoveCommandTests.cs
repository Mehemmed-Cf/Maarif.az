using Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand;
using Application.Repositories;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using Xunit;

namespace Tests.Application.SubjectsModule.Commands
{
    public class SubjectRemoveCommandTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly ISubjectTopicRepository _topicRepositoryMock;
        private readonly ISubjectMaterialRepository _materialRepositoryMock;
        private readonly ISubjectLiteratureRepository _literatureRepositoryMock;
        private readonly SubjectRemoveRequestHandler _handler;

        public SubjectRemoveCommandTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _topicRepositoryMock = Substitute.For<ISubjectTopicRepository>();
            _materialRepositoryMock = Substitute.For<ISubjectMaterialRepository>();
            _literatureRepositoryMock = Substitute.For<ISubjectLiteratureRepository>();
            _handler = new SubjectRemoveRequestHandler(
                _subjectRepositoryMock,
                _topicRepositoryMock,
                _materialRepositoryMock,
                _literatureRepositoryMock);
        }

        [Fact]
        public async Task Handle_SubjectNotFound_ThrowsNotFoundException()
        {
            var request = new SubjectRemoveRequest { Id = 1 };
            _subjectRepositoryMock.GetByIdWithDetailsTrackedAsync(1, Arg.Any<CancellationToken>())
                .Returns((Subject)null!);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Fənn tapılmadı*");
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new SubjectRemoveRequest { Id = 1 };
            var existing = new Subject
            {
                Id = 1,
                Topics = new List<SubjectTopic>(),
                Materials = new List<SubjectMaterial>(),
                Literatures = new List<SubjectLiterature>()
            };

            _subjectRepositoryMock.GetByIdWithDetailsTrackedAsync(1, Arg.Any<CancellationToken>())
                .Returns(existing);

            await _handler.Handle(request, CancellationToken.None);

            _subjectRepositoryMock.Received(1).Remove(existing);
            await _subjectRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
