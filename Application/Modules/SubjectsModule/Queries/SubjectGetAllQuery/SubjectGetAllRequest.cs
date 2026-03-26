using MediatR;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery
{
    public class SubjectGetAllRequest : IRequest<IEnumerable<SubjectGetAllResponseDto>>
    {
    }
}