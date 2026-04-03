using Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand;
using Application.Repositories;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Application.SubjectsModule.Commands
{
    public class SubjectRemoveCommandTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly SubjectRemoveRequestHandler _handler;

        public SubjectRemoveCommandTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _handler = new SubjectRemoveRequestHandler(_subjectRepositoryMock);
        }

        [Fact]
        public async Task Handle_SubjectNotFound_ThrowsNotFoundException()
        {
            var request = new SubjectRemoveRequest { Id = 1 };
            _subjectRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Subject, bool>>>(), Arg.Any<CancellationToken>())
                .Returns((Subject)null);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Fənn tapılmadı*");
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new SubjectRemoveRequest { Id = 1 };
            var existing = new Subject { Id = 1 };

            _subjectRepositoryMock
                .GetAsync(Arg.Any<Expression<Func<Subject, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existing);

            await _handler.Handle(request, CancellationToken.None);

            _subjectRepositoryMock.Received(1).Remove(existing);
            await _subjectRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        }
    }
}
