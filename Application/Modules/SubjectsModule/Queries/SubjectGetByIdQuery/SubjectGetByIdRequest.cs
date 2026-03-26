using MediatR;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery
{
    public class SubjectGetByIdRequest : IRequest<SubjectGetByIdResponseDto>
    {
        public int Id { get; set; }
    }
}