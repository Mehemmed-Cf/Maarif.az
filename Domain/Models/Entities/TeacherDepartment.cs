namespace Domain.Models.Entities
{
    public class TeacherDepartment
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
