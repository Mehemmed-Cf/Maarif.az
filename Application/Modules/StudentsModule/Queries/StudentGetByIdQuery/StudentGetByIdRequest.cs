using MediatR;

namespace Application.Modules.StudentsModule.Queries.StudentGetByIdQuery
{
    public class StudentGetByIdRequest : IRequest<StudentGetByIdResponseDto>
    {
        public int Id { get; set; }
    }
}