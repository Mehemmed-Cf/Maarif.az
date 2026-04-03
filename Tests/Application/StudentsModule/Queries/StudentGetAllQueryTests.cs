using Application.Modules.StudentsModule.Queries.StudentGetAllQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Tests.Application.StudentsModule.Queries
{
    public class StudentGetAllQueryTests
    {
        private readonly IStudentRepository _studentRepositoryMock;
        private readonly IMapper _mapper;
        private readonly StudentGetAllRequestHandler _handler;

        public StudentGetAllQueryTests()
        {
            _studentRepositoryMock = Substitute.For<IStudentRepository>();
            
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Student, StudentGetAllResponseDto>()
                   .ForMember(d => d.DepartmentName, m => m.MapFrom(s => s.Department != null ? s.Department.Name : null));
            });
            var serviceProvider = services.BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _handler = new StudentGetAllRequestHandler(_studentRepositoryMock, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsMappedList()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FullName = "John Doe", Department = new Department { Name = "CS", Faculty = new Faculty { Name = "Engineering" } } },
                new Student { Id = 2, FullName = "Jane Doe", Department = new Department { Name = "IS", Faculty = new Faculty { Name = "Engineering" } } }
            };

            var mockQueryable = students.AsQueryable().BuildMock();
            _studentRepositoryMock.GetAll().Returns(mockQueryable);

            // Act
            var result = await _handler.Handle(new StudentGetAllRequest(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.FullName == "John Doe" && x.DepartmentName == "CS");
        }
    }
}
