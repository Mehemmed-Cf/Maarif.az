using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyAddCommand
{
    public class FacultyAddRequestHandler : IRequestHandler<FacultyAddRequest, FacultyAddResponseDto>
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMapper mapper;

        public FacultyAddRequestHandler(IFacultyRepository facultyRepository, IMapper mapper)
        {
            this.facultyRepository = facultyRepository;
            this.mapper = mapper;
        }

        public async Task<FacultyAddResponseDto> Handle(FacultyAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Faculty
            {
                Name = request.Name,
            };

            await facultyRepository.AddAsync(entity);
            await facultyRepository.SaveAsync(cancellationToken);

            return mapper.Map<FacultyAddResponseDto>(entity);
        }
    }
}
