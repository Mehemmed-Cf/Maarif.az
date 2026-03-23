using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        public async Task<FacultyGetByIdResponseDto> Handle(FacultyGetByIdRequest request, CancellationToken cancellationToken)
        {
            var faculty = await facultyRepository.GetAll()
                .Include(f => f.Departments)
                .FirstOrDefaultAsync(f => f.Id == request.Id && f.DeletedAt == null, cancellationToken);

            if (faculty == null) return null;

            // The Mapper transforms the Faculty + Department Entities 
            // into the FacultyGetByIdResponseDto + List<FacultyDepartmentDto>
            return mapper.Map<FacultyGetByIdResponseDto>(faculty);
        }
    }
}
