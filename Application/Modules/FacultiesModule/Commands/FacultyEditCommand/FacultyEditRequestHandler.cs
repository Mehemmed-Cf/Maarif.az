using Application.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Modules.FacultiesModule.Commands.FacultyEditCommand
{
    public class FacultyEditRequestHandler : IRequestHandler<FacultyEditRequest, FacultyEditResponseDto>
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMapper mapper;

        public FacultyEditRequestHandler(IFacultyRepository facultyRepository, IMapper mapper)
        {
            this.facultyRepository = facultyRepository;
            this.mapper = mapper;
        }

        public async Task<FacultyEditResponseDto> Handle(FacultyEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await facultyRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            entity.Name = request.Name;
            entity.LastModifiedAt = DateTime.UtcNow;

            await facultyRepository.EditAsync(entity);
            await facultyRepository.SaveAsync(cancellationToken);

            return mapper.Map<FacultyEditResponseDto>(entity);
        }
    }
}
