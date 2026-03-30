using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.StudentsModule.Commands.StudentRegisterCommand
{
    public class StudentRegisterRequest : IRequest<StudentRegisterResponseDto>
    {
        public string SerialNumber { get; set; }
        public string FinCode { get; set; }
    }
}
