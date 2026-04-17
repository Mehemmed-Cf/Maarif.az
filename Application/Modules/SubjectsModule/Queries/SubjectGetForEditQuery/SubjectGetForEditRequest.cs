using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetForEditQuery
{
    public class SubjectGetForEditRequest : IRequest<SubjectEditRequest>
    {
        public int Id { get; set; }
    }
}
