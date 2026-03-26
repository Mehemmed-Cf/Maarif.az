using System.Linq.Expressions;
using Application.Modules.DepartmentsModule.Commands.DepartmentRemoveCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand;
using Application.Repositories;
using Domain.Models.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Tests.Application.DepartmentsModule
{
    /// <summary>Unit tests for DepartmentRemoveRequestHandler.</summary>
    public class DepartmentRemoveRequestHandlerTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentRemoveRequestHandler _handler;

        public DepartmentRemoveRequestHandlerTests()
        {
            _departmentRepository = Substitute.For<IDepartmentRepository>();
            _handler = new DepartmentRemoveRequestHandler(_departmentRepository);
        }

        [Fact]
        public async Task Handle_ExistingDepartment_RemovesSuccessfully()
        {
            // Arrange
            var request = new DepartmentRemoveRequest { Id = 1 };
            var existingEntity = new Department { Id = 1, Name = "IT" };

            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingEntity);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _departmentRepository.Received(1).Remove(Arg.Is<Department>(d => d.Id == 1));
            await _departmentRepository.Received(1).SaveAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_PassesNullToRemove()
        {
            // Arrange
            var request = new DepartmentRemoveRequest { Id = 99 };
            
            // Replicating actual behavior - it seems the handler doesn't check for null
            // and passes entity directly to Remove.
            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _departmentRepository.Received(1).Remove(null);
            await _departmentRepository.Received(1).SaveAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_SaveThrowsException_PropagatesException()
        {
            // Arrange
            var request = new DepartmentRemoveRequest { Id = 1 };
            var existingEntity = new Department { Id = 1, Name = "HR" };

            _departmentRepository.GetAsync(Arg.Any<Expression<Func<Department, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(existingEntity);
            _departmentRepository.When(x => x.SaveAsync(Arg.Any<CancellationToken>()))
                .Throw<InvalidOperationException>();

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            _departmentRepository.Received(1).Remove(existingEntity);
        }
    }
}
