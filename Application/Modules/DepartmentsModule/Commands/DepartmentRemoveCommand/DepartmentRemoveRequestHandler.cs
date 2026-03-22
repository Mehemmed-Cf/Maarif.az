using Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentRemoveCommand
{
    public class DepartmentRemoveRequestHandler : IRequestHandler<DepartmentRemoveRequest>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentRemoveRequestHandler(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public async Task Handle(DepartmentRemoveRequest request, CancellationToken cancellationToken)
        {
            Department entity;

            entity = await departmentRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            departmentRepository.Remove(entity);
            await departmentRepository.SaveAsync(cancellationToken);
        }
    }
}