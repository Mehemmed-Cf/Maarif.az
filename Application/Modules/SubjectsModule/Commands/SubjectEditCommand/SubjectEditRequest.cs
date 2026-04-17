using Application.Modules.SubjectsModule;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequest : IRequest<SubjectEditResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
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

        public List<SubjectTopicRowDto> Topics { get; set; } = new();
        public List<SubjectMaterialRowDto> Materials { get; set; } = new();
        public List<SubjectLiteratureRowDto> Literatures { get; set; } = new();
    }
}
