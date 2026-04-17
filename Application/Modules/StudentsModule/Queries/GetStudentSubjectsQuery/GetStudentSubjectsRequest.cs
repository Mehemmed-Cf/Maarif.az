using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentSubjectsQuery
{
    public class GetStudentSubjectsRequest : IRequest<IEnumerable<GetStudentSubjectsRequestDto>>
    {
        public int UserId { get; set; }
    }
}