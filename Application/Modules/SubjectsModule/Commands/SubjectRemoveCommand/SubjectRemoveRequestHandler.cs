using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand
{
    public class SubjectRemoveRequestHandler : IRequestHandler<SubjectRemoveRequest>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly ISubjectTopicRepository subjectTopicRepository;
        private readonly ISubjectMaterialRepository subjectMaterialRepository;
        private readonly ISubjectLiteratureRepository subjectLiteratureRepository;

        public SubjectRemoveRequestHandler(
            ISubjectRepository subjectRepository,
            ISubjectTopicRepository subjectTopicRepository,
            ISubjectMaterialRepository subjectMaterialRepository,
            ISubjectLiteratureRepository subjectLiteratureRepository)
        {
            this.subjectRepository = subjectRepository;
            this.subjectTopicRepository = subjectTopicRepository;
            this.subjectMaterialRepository = subjectMaterialRepository;
            this.subjectLiteratureRepository = subjectLiteratureRepository;
        }

        public async Task Handle(SubjectRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await subjectRepository.GetByIdWithDetailsTrackedAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            entity.Topics ??= new List<SubjectTopic>();
            entity.Materials ??= new List<SubjectMaterial>();
            entity.Literatures ??= new List<SubjectLiterature>();

            foreach (var t in entity.Topics.ToList())
                subjectTopicRepository.Remove(t);

            foreach (var m in entity.Materials.ToList())
                subjectMaterialRepository.Remove(m);

            foreach (var l in entity.Literatures.ToList())
                subjectLiteratureRepository.Remove(l);

            subjectRepository.Remove(entity);
            await subjectRepository.SaveAsync(cancellationToken);
        }
    }
}
