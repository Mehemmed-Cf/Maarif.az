using Domain.Models.Concrates;

namespace Domain.Models.Entities
{
    public class Subject : AuditableEntity
    {
        public int Id { get; set; }
        // --- Header Info ---
        public string Name { get; set; }
        public string Term { get; set; } // e.g., "2026 Yaz"
        public string GroupName { get; set; } // e.g., "622Sa2" or "ITT_P_Tex_25"

        // --- Teachers ---
        public string? LectureTeacher { get; set; }    // Mühazirə müəllimi
        public string? SeminarTeacher { get; set; }    // Seminar müəllimi
        public string? LabTeacher { get; set; }        // Laboratoriya müəllimi

        // --- Statistics ---
        public int StudentCount { get; set; }          // Tələbələrin sayı
        public int Credits { get; set; }               // Kredit
        public int TotalHours { get; set; }            // Saatlar
        public int WeekCount { get; set; }             // Həftə

        // --- Descriptions ---
        public string? Purpose { get; set; }           // Fənnin məqsədi
        public string? TeacherMethods { get; set; }    // Müəllimin metodları
        public string? SyllabusUrl { get; set; }       // Sillabus linki

         //--- Grading(Qiymətləndirmə üsulu) ---
        public int FreeWorkScore { get; set; }         // Sərbəst iş balı
        public int SeminarScore { get; set; }          // Məşğələ balı
        public int LabScore { get; set; }              // Laboratoriya balı
        public int AttendanceScore { get; set; }       // Davamiyyət balı
        public int ExamScore { get; set; }             // İmtahan balı (usually 50)

        // --- Navigation ---
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<SubjectMaterial> Materials { get; set; }
        public ICollection<SubjectLiterature> Literatures { get; set; } // New: "Ədəbiyyat" section
        public ICollection<SubjectTopic> Topics { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
