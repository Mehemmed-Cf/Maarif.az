using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Infrastructure.Exceptions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Tests.Application.DepartmentsModule
{
    /// <summary>Unit tests for DepartmentGetByIdRequestHandler.</summary>
    public class DepartmentGetByIdRequestHandlerTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly DepartmentGetByIdRequestHandler _handler;

        public DepartmentGetByIdRequestHandlerTests()
        {
            _departmentRepository = Substitute.For<IDepartmentRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DepartmentGetByIdRequestHandler(_departmentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ExistingDepartment_ReturnsConvertedDto()
        {
            // Arrange
            var request = new DepartmentGetByIdRequest { Id = 1 };
            var existingEntity = new Department { Id = 1, Name = "IT", FacultyId = 1 };
            var responseDto = new DepartmentGetByIdResponseDto { Id = 1, Name = "IT", FacultyId = 1 };

            _departmentRepository.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Returns(existingEntity);
            
            _mapper.Map<DepartmentGetByIdResponseDto>(existingEntity).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("IT");

            await _departmentRepository.Received(1).GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<DepartmentGetByIdResponseDto>(existingEntity);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new DepartmentGetByIdRequest { Id = 99 };
            
            _departmentRepository.GetByIdWithDetailsAsync(99, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
            
            await _departmentRepository.Received(1).GetByIdWithDetailsAsync(99, Arg.Any<CancellationToken>());
            _mapper.DidNotReceive().Map<DepartmentGetByIdResponseDto>(Arg.Any<Department>());
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var request = new DepartmentGetByIdRequest { Id = 1 };
            
            _departmentRepository.GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>())
                .Throws<InvalidOperationException>();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            await _departmentRepository.Received(1).GetByIdWithDetailsAsync(1, Arg.Any<CancellationToken>());
        }
    }
}
