using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentRemoveCommand
{
    public class StudentRemoveRequestHandler : IRequestHandler<StudentRemoveRequest>
    {
        private readonly IStudentRepository studentRepository;

        public StudentRemoveRequestHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task Handle(StudentRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await studentRepository.GetAsync(s => s.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Tələbə tapılmadı (Id: {request.Id})");

            // Remove() triggers EntityState.Deleted, which DataContext.SaveChangesAsync
            // intercepts and converts to a soft-delete (sets DeletedAt + DeletedBy).
            studentRepository.Remove(entity);
            await studentRepository.SaveAsync(cancellationToken);
        }
    }
}