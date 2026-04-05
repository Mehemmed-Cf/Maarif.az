using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Stables;

namespace Presentation.AppCode.ViewModels
{
    public class TeacherPortalViewModel : IWeeklyScheduleViewModel
    {
        public string FullName { get; set; } = "Teacher";
        public string TeacherNumber { get; set; } = "-";

        public WeekType CurrentWeekType { get; set; }
        public List<ScheduleDayDto> Days { get; set; } = new();

        public string Initials => PortalText.InitialsFrom(FullName);

        public DateTime WeekStart
        {
            get
            {
                var today = DateTime.Today;
                var diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                return today.AddDays(-diff);
            }
        }

        public IReadOnlyList<LessonScheduleDto> LessonsFor(DayOfWeek dow)
        {
            var day = Days.FirstOrDefault(d => d.DayOfWeek == dow);
            return day?.Lessons ?? (IReadOnlyList<LessonScheduleDto>)Array.Empty<LessonScheduleDto>();
        }
    }
}
