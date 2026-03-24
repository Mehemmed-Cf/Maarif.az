using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

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
            var group = await groupRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken)
                ?? throw new NotFoundException($"Group with id {request.Id} was not found.");

            mapper.Map(request, group);

            await groupRepository.EditAsync(group);
            await groupRepository.SaveAsync(cancellationToken);

            var response = mapper.Map<GroupResponseDto>(group);

            return response;
        }
    }
}
