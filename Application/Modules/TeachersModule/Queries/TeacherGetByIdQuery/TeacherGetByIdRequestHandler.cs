using Application.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.TeachersModule.Queries.TeacherGetByIdQuery
{
    public class TeacherGetByIdRequestHandler : IRequestHandler<TeacherGetByIdRequest, TeacherResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;

        public TeacherGetByIdRequestHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
        }

        public async Task<TeacherResponseDto> Handle(TeacherGetByIdRequest request, CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository.GetAll()
            .Include(t => t.TeacherDepartments)
                .ThenInclude(td => td.Department) // Jump from the bridge to the Department
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            return mapper.Map<TeacherResponseDto>(teacher);
        }
    }
}
