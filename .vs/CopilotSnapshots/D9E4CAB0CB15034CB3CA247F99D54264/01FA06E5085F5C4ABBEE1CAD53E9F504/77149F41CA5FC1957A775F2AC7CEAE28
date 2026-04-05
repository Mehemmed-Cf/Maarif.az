using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.LessonsModule.Queries.LessonGetByIdQuery
{
    public class LessonGetByIdRequestHandler : IRequestHandler<LessonGetByIdRequest, LessonDetailResponseDto>
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public LessonGetByIdRequestHandler(ILessonRepository lessonRepository, IMapper mapper)
        {
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<LessonDetailResponseDto> Handle(LessonGetByIdRequest request, CancellationToken cancellationToken)
        {
            var lesson = await lessonRepository.GetDetailsByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("lesson tapılmadı");

            // FIX #7: Removed `if (faculty == null) return null` — it was dead code.
            // The ?? throw above already guarantees faculty is non-null here.
            // Keeping a null-return path after a throw confuses readers and hides intent.
            return mapper.Map<LessonDetailResponseDto>(lesson);
        }
    }
}
