using MediatR;

namespace Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery
{
    public class FacultyGetByIdRequest : IRequest<FacultyGetByIdResponseDto>
    {
        public int Id { get; set; }
    }
}
