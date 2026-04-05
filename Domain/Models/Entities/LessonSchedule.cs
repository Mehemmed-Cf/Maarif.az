using Domain.Models.Concrates;
using Domain.Models.Stables;

namespace Domain.Models.Entities
{
    public class LessonSchedule : AuditableEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public LessonType LessonType { get; set; }  // Məşğələ, Laboratoriya, Mühazirə
        public WeekType WeekType { get; set; }       // Upper, Lower, Both
    }
}
