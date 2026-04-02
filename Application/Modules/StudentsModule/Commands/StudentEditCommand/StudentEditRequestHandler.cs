using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.StudentEditCommand
{
    public class StudentEditRequestHandler : IRequestHandler<StudentEditRequest, StudentEditResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public StudentEditRequestHandler(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<StudentEditResponseDto> Handle(
            StudentEditRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await studentRepository.GetAsync(s => s.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Tələbə tapılmadı (Id: {request.Id})");

            if (entity.DepartmentId != request.DepartmentId)
                _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);


            mapper.Map(request, entity);

            await studentRepository.EditAsync(entity);
            await studentRepository.SaveAsync(cancellationToken);

            return mapper.Map<StudentEditResponseDto>(entity);
        }
    }
}