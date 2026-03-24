using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.StudentsModule
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string StudentNumber { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public byte Year { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public int GroupCount { get; set; }
    }
}
