using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Modules.GroupsModule.Queries.GroupGetAllQuery
{
    public class GroupGetAllRequestHandler : IRequestHandler<GroupGetAllRequest, IEnumerable<GroupResponseDto>>
    {
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public GroupGetAllRequestHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GroupResponseDto>> Handle(GroupGetAllRequest request, CancellationToken cancellationToken)
        {
            var result = await groupRepository.GetAllAsync(cancellationToken);
            return result;
        }
    }
}
