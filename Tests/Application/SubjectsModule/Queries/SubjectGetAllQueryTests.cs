using Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Tests.Application.SubjectsModule.Queries
{
    public class SubjectGetAllQueryTests
    {
        private readonly ISubjectRepository _subjectRepositoryMock;
        private readonly IMapper _mapper;
        private readonly SubjectGetAllRequestHandler _handler;

        public SubjectGetAllQueryTests()
        {
            _subjectRepositoryMock = Substitute.For<ISubjectRepository>();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Subject, SubjectGetAllResponseDto>()
                   .ForMember(d => d.DepartmentName, m => m.MapFrom(s => s.Department != null ? s.Department.Name : null));
            });
            var serviceProvider = services.BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _handler = new SubjectGetAllRequestHandler(_subjectRepositoryMock, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsMappedList()
        {
            var subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Math", Department = new Department { Name = "Science", Faculty = new Faculty { Name = "Science" } } },
            };

            var mockQueryable = subjects.AsQueryable().BuildMock();
            _subjectRepositoryMock.GetAll().Returns(mockQueryable);

            var result = await _handler.Handle(new SubjectGetAllRequest(), CancellationToken.None);

            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Math");
        }
    }
}
