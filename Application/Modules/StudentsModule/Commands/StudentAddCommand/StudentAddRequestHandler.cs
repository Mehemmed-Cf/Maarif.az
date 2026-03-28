using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentAddCommand
{
    public class StudentAddRequestHandler : IRequestHandler<StudentAddRequest, StudentAddResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public StudentAddRequestHandler(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<StudentAddResponseDto> Handle(
            StudentAddRequest request,
            CancellationToken cancellationToken)
        {
            // Verify the department exists before assigning the student to it.
            // GetAsync throws DirectoryNotFoundException if not found — the exception
            // middleware should map this to 404; replace with NotFoundException if available.
            _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            var student = mapper.Map<Student>(request);

            await studentRepository.AddAsync(student, cancellationToken);
            await studentRepository.SaveAsync(cancellationToken);

            return mapper.Map<StudentAddResponseDto>(student);
        }
    }
}