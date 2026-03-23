using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyRemoveCommand
{
    public class FacultyRemoveRequestHandler : IRequestHandler<FacultyRemoveRequest>
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMapper mapper;

        public FacultyRemoveRequestHandler(IFacultyRepository facultyRepository, IMapper mapper)
        {
            this.facultyRepository = facultyRepository;
            this.mapper = mapper;
        }

        public async Task Handle(FacultyRemoveRequest request, CancellationToken cancellationToken)
        {
            Faculty entity;

            entity = await facultyRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            facultyRepository.Remove(entity);
            await facultyRepository.SaveAsync(cancellationToken);
        }
    }
}
