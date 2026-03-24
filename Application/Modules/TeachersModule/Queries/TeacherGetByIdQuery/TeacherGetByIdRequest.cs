using MediatR;

namespace Application.Modules.TeachersModule.Queries.TeacherGetByIdQuery
{
    public class TeacherGetByIdRequest : IRequest<TeacherResponseDto>
    {
        public int Id { get; set; }
    }
}
