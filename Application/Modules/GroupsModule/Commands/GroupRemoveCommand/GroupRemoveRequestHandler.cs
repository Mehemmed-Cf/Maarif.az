using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.GroupsModule.Commands.GroupRemoveCommand
{
    public class GroupRemoveRequestHandler : IRequestHandler<GroupRemoveRequest>
    {
        private readonly IGroupRepository groupRepository;

        public GroupRemoveRequestHandler(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public async Task Handle(GroupRemoveRequest request, CancellationToken cancellationToken)
        {
            Group entity;

            entity = await groupRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            groupRepository.Remove(entity);
            await groupRepository.SaveAsync(cancellationToken);
        }
    }
}
