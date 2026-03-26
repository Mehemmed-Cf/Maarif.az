namespace Application.Modules.TeachersModule
{
    public class TeacherResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double Experience { get; set; }
        // We return a list of where this teacher works
        public List<TeacherDepartmentDto> Departments { get; set; }
    }
}
