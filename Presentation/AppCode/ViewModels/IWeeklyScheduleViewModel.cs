using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Stables;

namespace Presentation.AppCode.ViewModels
{
    public interface IWeeklyScheduleViewModel
    {
        WeekType CurrentWeekType { get; }
        DateTime WeekStart { get; }
        IReadOnlyList<LessonScheduleDto> LessonsFor(DayOfWeek dow);
    }
}
