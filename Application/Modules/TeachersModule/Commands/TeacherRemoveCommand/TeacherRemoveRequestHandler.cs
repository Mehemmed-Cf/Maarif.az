using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherRemoveCommand
{
    public class TeacherRemoveRequestHandler : IRequestHandler<TeacherRemoveRequest>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;

        public TeacherRemoveRequestHandler(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
        }

        public async Task Handle(TeacherRemoveRequest request, CancellationToken cancellationToken)
        {
            Teacher entity;

            entity = await teacherRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            teacherRepository.Remove(entity);
            await teacherRepository.SaveAsync(cancellationToken);
        }
    }
}
