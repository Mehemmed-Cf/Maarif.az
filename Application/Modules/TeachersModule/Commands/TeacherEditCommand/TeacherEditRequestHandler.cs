using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.TeachersModule.Commands.TeacherEditCommand
{
    public class TeacherEditRequestHandler : IRequestHandler<TeacherEditRequest, TeacherResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;

        public TeacherEditRequestHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
        }

        public async Task<TeacherResponseDto> Handle(TeacherEditRequest request, CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetAll()
                    .Include(t => t.TeacherDepartments)
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            // 1. Update basic info
            mapper.Map(request, teacher);

            // 2. Sync Departments (The "Simple" Way)
            // Clear the old ones
            teacher.TeacherDepartments.Clear();

            // Add the new selection
            foreach (var id in request.DepartmentIds)
            {
                teacher.TeacherDepartments.Add(new TeacherDepartment { DepartmentId = id });
            }

            await teacherRepository.SaveAsync(cancellationToken);
            return mapper.Map<TeacherResponseDto>(teacher);
        }
    }
}
