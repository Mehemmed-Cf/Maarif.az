using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.GroupsModule.Queries.GroupGetByIdQuery
{
    public class GroupGetByIdRequestHandler : IRequestHandler<GroupGetByIdRequest, GroupDetailsResponseDto>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public GroupGetByIdRequestHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }

        public async Task<GroupDetailsResponseDto> Handle(GroupGetByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await groupRepository.GetDetailsByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Group with id {request.Id} was not found.");

            return result;
        }
    }
}
