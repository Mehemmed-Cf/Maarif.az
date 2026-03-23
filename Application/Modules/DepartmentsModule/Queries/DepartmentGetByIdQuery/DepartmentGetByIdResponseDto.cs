using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;

namespace Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery
{
    public class DepartmentGetByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public DepartmentFacultyDto Faculty { get; set; }
        public IReadOnlyList<DepartmentStudentDto> Students { get; set; }
        public IReadOnlyList<DepartmentGroupDto> Groups { get; set; }
        public IReadOnlyList<DepartmentSubjectDto> Subjects { get; set; }
        public IReadOnlyList<DepartmentTeacherDto> Teachers { get; set; }
    }
}
