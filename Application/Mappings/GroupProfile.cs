using Application.Modules.GroupsModule;
using Application.Modules.GroupsModule.Commands.GroupAddCommand;
using Application.Modules.GroupsModule.Commands.GroupEditCommand;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            // Mapping for GetAll
            CreateMap<Group, GroupResponseDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name))
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.StudentGroups.Count));

            // Mapping for GetById
            CreateMap<Group, GroupDetailsResponseDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name))
                .ForMember(d => d.Students, o => o.MapFrom(s => s.StudentGroups.Select(sg => sg.Student)));

            // Helper mapping for the student list
            //CreateMap<Student, StudentSmallDto>()
            //    .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));

            CreateMap<Student, StudentSmallDto>();

            CreateMap<GroupAddRequest, Group>();
            CreateMap<GroupEditRequest, Group>();
        }
    }
}
