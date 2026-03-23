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

        public async Task<IEnumerable<FacultyGetAllResponseDto>> Handle(FacultyGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = facultyRepository.GetAll();

            query = query.Where(m => m.DeletedAt == null);

            var result = await facultyRepository.GetAll()
                .Where(m => m.DeletedAt == null)
                .ProjectTo<FacultyGetAllResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
