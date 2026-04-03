using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using Xunit;

namespace Tests.Application.SubjectsModule.Queries
{
    public class SubjectGetByIdQueryTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly IMapper _mapperMock;
        private readonly SubjectGetByIdRequestHandler _handler;

        public SubjectGetByIdQueryTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _handler = new SubjectGetByIdRequestHandler(_subjectRepositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_NotFound_ThrowsNotFoundException()
        {
            var request = new SubjectGetByIdRequest { Id = 1 };
            _subjectRepositoryMock.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Returns((Subject)null);

            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_ValidRequest_Success()
        {
            var request = new SubjectGetByIdRequest { Id = 1 };
            var subject = new Subject { Id = 1, Name = "Math" };
            var responseDto = new SubjectGetByIdResponseDto { Id = 1 };

            _subjectRepositoryMock.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Returns(subject);
            _mapperMock.Map<SubjectGetByIdResponseDto>(subject).Returns(responseDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(responseDto);
        }
    }
}
