using Application.Modules.SubjectsModule;

namespace Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery
{
    public class SubjectGetByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string? LectureTeacher { get; set; }
        public string? SeminarTeacher { get; set; }
        public string? LabTeacher { get; set; }
        public int StudentCount { get; set; }
        public int Credits { get; set; }
        public int TotalHours { get; set; }
        public int WeekCount { get; set; }
        public string? Purpose { get; set; }
        public string? TeacherMethods { get; set; }
        public string? SyllabusUrl { get; set; }
        public int FreeWorkScore { get; set; }
        public int SeminarScore { get; set; }
        public int LabScore { get; set; }
        public int AttendanceScore { get; set; }
        public int ExamScore { get; set; }
        public int DepartmentId { get; set; }
        public SubjectDepartmentDto Department { get; set; } = null!;
        public IReadOnlyList<SubjectLessonDto> Lessons { get; set; } = Array.Empty<SubjectLessonDto>();
        public IReadOnlyList<SubjectTopicRowDto> Topics { get; set; } = Array.Empty<SubjectTopicRowDto>();
        public IReadOnlyList<SubjectMaterialRowDto> Materials { get; set; } = Array.Empty<SubjectMaterialRowDto>();
        public IReadOnlyList<SubjectLiteratureRowDto> Literatures { get; set; } = Array.Empty<SubjectLiteratureRowDto>();
    }

    public class SubjectDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
    }

    public class SubjectLessonDto
    {
        public int Id { get; set; }
        public string TeacherFullName { get; set; } = string.Empty;
    }
}
