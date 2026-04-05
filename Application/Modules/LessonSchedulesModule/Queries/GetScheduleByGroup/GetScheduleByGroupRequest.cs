using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup
{
    public class GetScheduleByGroupRequest : IRequest<GetScheduleByGroupResponseDto>
    {
        public int GroupId { get; set; }
        public WeekType? WeekType { get; set; }
    }
}
