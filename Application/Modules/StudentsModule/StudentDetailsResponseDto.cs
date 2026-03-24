namespace Application.Modules.StudentsModule
{
    public class StudentDetailsResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string StudentNumber { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string EducationType { get; set; }
        public string Status { get; set; }
        public byte Year { get; set; }
        public string Grade { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public List<StudentGroupSmallDto> Groups { get; set; }
    }
}
