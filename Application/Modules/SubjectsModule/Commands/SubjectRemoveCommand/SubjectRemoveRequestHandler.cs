using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand
{
    public class SubjectRemoveRequestHandler : IRequestHandler<SubjectRemoveRequest>
    {
        private readonly ISubjectRepository subjectRepository;

        public SubjectRemoveRequestHandler(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }

        public async Task Handle(SubjectRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await subjectRepository.GetAsync(s => s.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            // Soft-delete: DataContext.SaveChangesAsync intercepts EntityState.Deleted
            // and converts it to a Modified with DeletedAt/DeletedBy populated.
            subjectRepository.Remove(entity);
            await subjectRepository.SaveAsync(cancellationToken);
        }
    }
}