using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherAddCommand
{
    public class TeacherAddRequestHandler : IRequestHandler<TeacherAddRequest, TeacherResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public TeacherAddRequestHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }
        public async Task<TeacherResponseDto> Handle(TeacherAddRequest request, CancellationToken cancellationToken)
        {
            var teacher = mapper.Map<Teacher>(request);


            // Convert the list of IDs into the TeacherDepartment join entities
            teacher.TeacherDepartments = request.DepartmentIds.Select(id => new TeacherDepartment
            {
                DepartmentId = id
                // TeacherId is handled automatically by EF when you save the 'teacher' object
            }).ToList();

            await teacherRepository.AddAsync(teacher, cancellationToken);
            await teacherRepository.SaveAsync(cancellationToken);

            return mapper.Map<TeacherResponseDto>(teacher);
        }
    }
}
