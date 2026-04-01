using Domain.Models.Stables;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentAddCommand
{
    public class StudentAddRequest : IRequest<StudentAddResponseDto>
    {
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string FinCode { get; set; }
        public string StudentNumber { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EducationType EducationType { get; set; }
        public StatusType Status { get; set; }
        public byte Year { get; set; }
        public GradeType Grade { get; set; }
        public int DepartmentId { get; set; }
        //public int UserId { get; set; }
    }
}