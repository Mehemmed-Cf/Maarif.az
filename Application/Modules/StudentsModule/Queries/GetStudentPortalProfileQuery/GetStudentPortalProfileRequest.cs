using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery
{
    public class GetStudentPortalProfileRequest : IRequest<StudentPortalProfileDto?>
    {
        public int UserId { get; set; }
    }
}
