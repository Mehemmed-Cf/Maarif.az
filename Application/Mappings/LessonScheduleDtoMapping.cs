using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public static class LessonScheduleDtoMapping
    {
        public static LessonScheduleDto ToDto(LessonSchedule s)
        {
            var room = s.Room;
            string roomDisplay;
            if (room is null)
                roomDisplay = "—";
            else
            {
                var b = room.Building?.Name;
                roomDisplay = string.IsNullOrWhiteSpace(b)
                    ? $"Korpus {room.BuildingId} · otaq {room.Number}"
                    : $"{b} · otaq {room.Number}";
            }

            return new LessonScheduleDto
            {
                Id = s.Id,
                LessonId = s.LessonId,
                SubjectName = s.Lesson.Subject.Name,
                TeacherFullName = s.Lesson.Teacher.FullName,
                GroupId = s.GroupId,
                GroupName = s.Group.Name,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomId = s.RoomId,
                RoomDisplay = roomDisplay,
                LessonType = s.LessonType,
                WeekType = s.WeekType
            };
        }
    }
}
