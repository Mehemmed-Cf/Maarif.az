using Domain.Models.Stables;

namespace Domain.Models.ValueObjects
{
    public class FinData
    {
        public string FinCode { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderType Gender { get; set; }

        public DepartmentType Department { get; set; }
        public EducationType Education { get; set; }
    }
}