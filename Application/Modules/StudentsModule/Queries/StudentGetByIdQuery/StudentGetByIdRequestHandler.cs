using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Queries.StudentGetByIdQuery
{
    public class StudentGetByIdRequestHandler : IRequestHandler<StudentGetByIdRequest, StudentGetByIdResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentGetByIdRequestHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<StudentGetByIdResponseDto> Handle(
            StudentGetByIdRequest request,
            CancellationToken cancellationToken)
        {
            // GetByIdWithDetailsAsync loads Department → Faculty and StudentGroups → Group.
            // HasQueryFilter on StudentConfiguration ensures soft-deleted students
            // are never returned.
            var entity = await studentRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Tələbə tapılmadı (Id: {request.Id})");

            return mapper.Map<StudentGetByIdResponseDto>(entity);
        }
    }
}