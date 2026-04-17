using Domain.Models.Concrates;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Teacher : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        /// <summary>Set when the teacher self-registers via government verification; optional for admin-created rows.</summary>
        public string? FinCode { get; set; }
        /// <summary>Normalized document serial from self-registration; null for admin-created rows.</summary>
        public string? DocumentSerialNumber { get; set; }
        /// <summary>Login id (Identity user name); set on self-registration.</summary>
        public string? TeacherNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double Experience { get; set; }
        public int UserId { get; set; }

        // BUG FIX: Removed GroupCount and ActiveLessons stored columns.
        // These were computed values that had to be manually kept in sync with
        // LessonGroup and Lessons tables — a guaranteed source of stale data.
        // Use the [NotMapped] computed properties below instead, which derive
        // live values from the navigation collections (populated by EF includes).
        // For performance-sensitive reads, use the Dapper queries shown in the
        // repository (a single SQL aggregation is faster than loading collections).

        [NotMapped]
        public int GroupCount => LessonGroups?.Select(lg => lg.GroupId).Distinct().Count() ?? 0;

        [NotMapped]
        public int ActiveLessons => Lessons?.Count ?? 0;

        public ICollection<TeacherDepartment> TeacherDepartments { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Attendance> MarkedAttendances { get; set; }

        // Helper navigation — not a direct FK, reached via Lessons → LessonGroups
        [NotMapped]
        private IEnumerable<LessonGroup> LessonGroups =>
            Lessons?.SelectMany(l => l.LessonGroups) ?? [];
    }
}