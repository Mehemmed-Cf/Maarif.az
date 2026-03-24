using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery
{
    public class FacultyGetAllRequestHandler : IRequestHandler<FacultyGetAllRequest, IEnumerable<FacultyGetAllResponseDto>>
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMapper mapper;

        public FacultyGetAllRequestHandler(IFacultyRepository facultyRepository, IMapper mapper)
        {
            this.facultyRepository = facultyRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FacultyGetAllResponseDto>> Handle(
            FacultyGetAllRequest request,
            CancellationToken cancellationToken)
        {
            // FIX #6: Previous code called GetAll() twice — first built a `query` variable
            // with a Where filter (then abandoned it), then built a second chain on a fresh
            // GetAll() call. HasQueryFilter handles soft-delete; ProjectTo handles mapping.
            return await facultyRepository
                .GetAll()
                .ProjectTo<FacultyGetAllResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}