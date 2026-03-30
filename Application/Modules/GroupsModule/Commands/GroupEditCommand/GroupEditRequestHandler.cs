using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.GroupsModule.Commands.GroupEditCommand
{
    public class GroupEditRequestHandler : IRequestHandler<GroupEditRequest, GroupResponseDto>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public GroupEditRequestHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }

        public async Task<GroupResponseDto> Handle(GroupEditRequest request, CancellationToken cancellationToken)
        {
            await groupRepository.UpdateGroupAsync(request, cancellationToken);

            //var newStudentGroups = request.StudentIds
            //.Distinct()
            //.Select(id => new StudentGroup { GroupId = request.Id, StudentId = id })
            //.ToList();

            //var newLessonGroups = request.LessonIds
            //    .Distinct()
            //    .Select(id => new LessonGroup { GroupId = request.Id, LessonId = id })
            //    .ToList();

            return new GroupResponseDto
            {
                Id = request.Id,
                Name = request.Name,
                Year = request.Year,
                DepartmentId = request.DepartmentId
            };

            //var group = await groupRepository.GetAsync(
            //    m => m.Id == request.Id && m.DeletedAt == null,
            //    cancellationToken,
            //    include: q => q.Include(g => g.StudentGroups).Include(g => g.LessonGroups));

            //group.Name = request.Name;
            //group.Year = request.Year;
            //group.DepartmentId = request.DepartmentId;

            //// ✅ Clear first, let EF track the deletes
            //group.StudentGroups.Clear();
            //group.LessonGroups.Clear();
            //await groupRepository.SaveAsync(cancellationToken); // flush deletes

            //// ✅ Now assign the new ones
            //group.StudentGroups = request.StudentIds
            //    .Select(id => new StudentGroup { GroupId = group.Id, StudentId = id })
            //    .ToList();

            //group.LessonGroups = request.LessonIds
            //    .Select(id => new LessonGroup { GroupId = group.Id, LessonId = id })
            //    .ToList();

            //await groupRepository.SaveAsync(cancellationToken);

            //var response = mapper.Map<GroupResponseDto>(group);
            //return response;
        }
    }
}
