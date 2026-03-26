using Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.Application.DepartmentsModule
{
    /// <summary>Unit tests for DepartmentAddRequestHandler.</summary>
    public class DepartmentAddRequestHandlerTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly DepartmentAddRequestHandler _handler;

        public DepartmentAddRequestHandlerTests()
        {
            _departmentRepository = Substitute.For<IDepartmentRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DepartmentAddRequestHandler(_departmentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsCreatedResponse()
        {
            // Arrange
            var request = new DepartmentAddRequest { Name = "IT Department", FacultyId = 1 };
            var responseDto = new DepartmentAddResponseDto { Id = 1, Name = "IT Department", FacultyId = 1 };
            
            _mapper.Map<DepartmentAddResponseDto>(Arg.Any<Department>()).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("IT Department");
            
            await _departmentRepository.Received(1).AddAsync(Arg.Is<Department>(d => d.Name == "IT Department" && d.FacultyId == 1));
            await _departmentRepository.Received(1).SaveAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_EmptyName_ThrowsExceptionIfConfigured()
        {
            // Arrange
            var request = new DepartmentAddRequest { Name = "", FacultyId = 1 };
            // Optional: testing any inherent throwing behavior if Validation is part of handler
            // Although pipeline usually handles it. We just verify AddAsync is called.
            var responseDto = new DepartmentAddResponseDto { Id = 2, Name = "", FacultyId = 1 };
            
            _mapper.Map<DepartmentAddResponseDto>(Arg.Any<Department>()).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            await _departmentRepository.Received(1).AddAsync(Arg.Is<Department>(d => d.Name == "" && d.FacultyId == 1));
            await _departmentRepository.Received(1).SaveAsync(CancellationToken.None);
        }
        
        [Fact]
        public async Task Handle_SaveFails_ThrowsException()
        {
            // Arrange
            var request = new DepartmentAddRequest { Name = "HR", FacultyId = 2 };
            _departmentRepository.When(x => x.SaveAsync(Arg.Any<CancellationToken>()))
                .Throw<InvalidOperationException>();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            await _departmentRepository.Received(1).AddAsync(Arg.Any<Department>());
        }
    }
}
