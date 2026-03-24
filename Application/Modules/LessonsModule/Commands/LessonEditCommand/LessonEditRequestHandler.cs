using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonEditCommand
{
    public class LessonEditRequestHandler : IRequestHandler<LessonEditRequest, LessonResponseDto>
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public LessonEditRequestHandler(ILessonRepository lessonRepository, IMapper mapper)
        {
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<LessonResponseDto> Handle(LessonEditRequest request, CancellationToken cancellationToken)
        {
            var group = await lessonRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            mapper.Map(request, group);

            await lessonRepository.EditAsync(group);
            await lessonRepository.SaveAsync(cancellationToken);

            var response = mapper.Map<LessonResponseDto>(group);

            return response;
        }
    }
}
