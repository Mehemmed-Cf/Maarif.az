using MediatR;

namespace Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery
{
    public class GetTeacherPortalProfileRequest : IRequest<TeacherPortalProfileDto?>
    {
        public int UserId { get; set; }
    }
}
