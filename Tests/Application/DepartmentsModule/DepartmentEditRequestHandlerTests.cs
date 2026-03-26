using System.Linq.Expressions;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
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
    /// <summary>Unit tests for DepartmentEditRequestHandler.</summary>
    public class DepartmentEditRequestHandlerTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly DepartmentEditRequestHandler _handler;

        public DepartmentEditRequestHandlerTests()
        {
            _departmentRepository = Substitute.For<IDepartmentRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DepartmentEditRequestHandler(_departmentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsEditedResponse()
        {
            // Arrange
            var request = new DepartmentEditRequest { Id = 1, Name = "New IT", FacultyId = 2 };
            var existingEntity = new Department { Id = 1, Name = "Old IT", FacultyId = 1 };
            var responseDto = new DepartmentEditResponseDto { Id = 1, Name = "New IT", FacultyId = 2 };

            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingEntity);
            
            _mapper.Map<DepartmentEditResponseDto>(Arg.Any<Department>()).Returns(responseDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("New IT");

            await _departmentRepository.Received(1).EditAsync(Arg.Is<Department>(d => d.Name == "New IT" && d.FacultyId == 2));
            await _departmentRepository.Received(1).SaveAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var request = new DepartmentEditRequest { Id = 99, Name = "Missing", FacultyId = 1 };
            
            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
            await _departmentRepository.DidNotReceive().EditAsync(Arg.Any<Department>());
            await _departmentRepository.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_SaveFails_ThrowsException()
        {
            // Arrange
            var request = new DepartmentEditRequest { Id = 1, Name = "Name", FacultyId = 1 };
            var existingEntity = new Department { Id = 1, Name = "Old", FacultyId = 1 };

            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingEntity);
            _departmentRepository.When(x => x.SaveAsync(Arg.Any<CancellationToken>()))
                .Throw<InvalidOperationException>();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            await _departmentRepository.Received(1).EditAsync(Arg.Is<Department>(d => d.Id == 1));
        }
    }
}
