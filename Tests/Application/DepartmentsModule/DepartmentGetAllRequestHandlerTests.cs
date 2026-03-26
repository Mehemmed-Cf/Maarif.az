using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Tests.Application.DepartmentsModule
{
    /// <summary>Unit tests for DepartmentGetAllRequestHandler.</summary>
    public class DepartmentGetAllRequestHandlerTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly DepartmentGetAllRequestHandler _handler;

        public DepartmentGetAllRequestHandlerTests()
        {
            _departmentRepository = Substitute.For<IDepartmentRepository>();
            
            // Standard AutoMapper setup suitable for testing ProjectTo
            var configuration = new MapperConfiguration(cfg =>
            {
                // Typically you'd add the real profile here.
                // For unit isolation without mapping configurations, we rely on MockQueryable
                // Since handler uses ProjectTo, we configure a real Mapper engine.
                cfg.CreateMap<Department, DepartmentGetAllResponseDto>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new DepartmentGetAllRequestHandler(_departmentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_DepartmentsExist_ReturnsMappedDtoList()
        {
            // Arrange
            var request = new DepartmentGetAllRequest();
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "IT", FacultyId = 1 },
                new Department { Id = 2, Name = "HR", FacultyId = 2 }
            };

            var mockQueryable = departments.AsQueryable().BuildMock();
            _departmentRepository.GetAll().Returns(mockQueryable);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("IT");
            result.Last().Name.Should().Be("HR");

            _departmentRepository.Received(1).GetAll();
        }

        [Fact]
        public async Task Handle_NoDepartmentsExist_ReturnsEmptyList()
        {
            // Arrange
            var request = new DepartmentGetAllRequest();
            var departments = new List<Department>();

            var mockQueryable = departments.AsQueryable().BuildMock();
            _departmentRepository.GetAll().Returns(mockQueryable);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _departmentRepository.Received(1).GetAll();
        }

        [Fact]
        public async Task Handle_GetAllThrowsException_PropagatesException()
        {
            // Arrange
            var request = new DepartmentGetAllRequest();
            _departmentRepository.GetAll().Returns(_ => throw new InvalidOperationException());

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            _departmentRepository.Received(1).GetAll();
        }
    }
}
