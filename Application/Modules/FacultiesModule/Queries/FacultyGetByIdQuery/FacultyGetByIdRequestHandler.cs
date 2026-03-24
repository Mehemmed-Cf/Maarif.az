using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery
{
    public class FacultyGetByIdRequestHandler : IRequestHandler<FacultyGetByIdRequest, FacultyGetByIdResponseDto>
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMapper mapper;

        public FacultyGetByIdRequestHandler(IFacultyRepository facultyRepository, IMapper mapper)
        {
            this.facultyRepository = facultyRepository;
            this.mapper = mapper;
        }

        public async Task<FacultyGetByIdResponseDto> Handle(
            FacultyGetByIdRequest request,
            CancellationToken cancellationToken)
        {
            var faculty = await facultyRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Fakültə tapılmadı");

            // FIX #7: Removed `if (faculty == null) return null` — it was dead code.
            // The ?? throw above already guarantees faculty is non-null here.
            // Keeping a null-return path after a throw confuses readers and hides intent.
            return mapper.Map<FacultyGetByIdResponseDto>(faculty);
        }
    }
}