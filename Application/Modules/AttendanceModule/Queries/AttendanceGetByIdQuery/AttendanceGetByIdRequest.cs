using MediatR;

namespace Application.Modules.AttendanceModule.Queries.AttendanceGetByIdQuery
{
    public class AttendanceGetByIdRequest : IRequest<AttendanceDetailsDto>
    {
        public int Id { get; set; }
    }
}
