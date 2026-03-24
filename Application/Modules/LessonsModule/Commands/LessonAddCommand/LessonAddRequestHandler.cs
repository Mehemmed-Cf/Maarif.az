using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonAddCommand
{
    public class LessonAddRequestHandler : IRequestHandler<LessonAddRequest, LessonResponseDto>
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public LessonAddRequestHandler(ILessonRepository lessonRepository, IMapper mapper)
        {
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<LessonResponseDto> Handle(LessonAddRequest request, CancellationToken cancellationToken)
        {
            var lesson = mapper.Map<Lesson>(request);

            await lessonRepository.AddAsync(lesson, cancellationToken);
            await lessonRepository.SaveAsync(cancellationToken);

            var response = mapper.Map<LessonResponseDto>(lesson);

            return response;
        }
    }
}
