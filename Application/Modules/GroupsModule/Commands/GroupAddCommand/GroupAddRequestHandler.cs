using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.GroupsModule.Commands.GroupAddCommand
{
    public class GroupAddRequestHandler : IRequestHandler<GroupAddRequest, GroupResponseDto>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public GroupAddRequestHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }
        public async Task<GroupResponseDto> Handle(GroupAddRequest request, CancellationToken cancellationToken)
        {
            //var group = new Group
            //{
            //    Name = request.Name,
            //    Year = request.Year,
            //    DepartmentId = request.DepartmentId
            //};

            //await groupRepository.AddAsync(group, cancellationToken);
            //return new GroupResponseDto(group.Id, group.Name);

            var group = mapper.Map<Group>(request);

            await groupRepository.AddAsync(group, cancellationToken);
            await groupRepository.SaveAsync(cancellationToken);

            // 4. Map back to DTO
            // Note: Since this is a brand new group, DepartmentName might be null 
            // unless you include the Department or map it manually.
            var response = mapper.Map<GroupResponseDto>(group);

            return response;
        }
    }
}
