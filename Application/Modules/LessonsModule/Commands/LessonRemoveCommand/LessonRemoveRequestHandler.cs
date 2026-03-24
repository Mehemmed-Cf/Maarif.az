using Application.Repositories;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.LessonsModule.Commands.LessonRemoveCommand
{
    public class LessonRemoveRequestHandler : IRequestHandler<LessonRemoveRequest>
    {
        private readonly ILessonRepository lessonRepository;

        public LessonRemoveRequestHandler(ILessonRepository lessonRepository)
        {
            this.lessonRepository = lessonRepository;
        }

        public async Task Handle(LessonRemoveRequest request, CancellationToken cancellationToken)
        {
            Lesson entity;

            entity = await lessonRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            lessonRepository.Remove(entity);
            await lessonRepository.SaveAsync(cancellationToken);
        }
    }
}
