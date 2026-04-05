using Domain.Models.Stables;

namespace Domain.Models.ValueObjects
{
    public class TeacherGovernmentData
    {
        public string FinCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DepartmentType Department { get; set; }
        public double Experience { get; set; }
        public string? MobileNumber { get; set; }
    }
}
