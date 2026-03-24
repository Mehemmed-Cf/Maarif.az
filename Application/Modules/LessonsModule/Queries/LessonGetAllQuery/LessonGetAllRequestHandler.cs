using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.LessonsModule.Queries.LessonGetAllQuery
{
    public class LessonGetAllRequestHandler : IRequestHandler<LessonGetAllRequest, IEnumerable<LessonResponseDto>>
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public LessonGetAllRequestHandler(ILessonRepository lessonRepository, IMapper mapper)
        {
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LessonResponseDto>> Handle(LessonGetAllRequest request, CancellationToken cancellationToken)
        {
            return await lessonRepository
                .GetAll()
                .ProjectTo<LessonResponseDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
