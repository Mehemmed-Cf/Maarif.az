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
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.StudentGroups.Count))
                .ForMember(d => d.LessonCount, o => o.MapFrom(s => s.LessonGroups.Count));

            // Mapping for GetById
            CreateMap<Group, GroupDetailsResponseDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name))
                .ForMember(d => d.Students, o => o.MapFrom(s => s.StudentGroups.Select(sg => sg.Student)));


            CreateMap<Student, StudentSmallDto>();
            CreateMap<Student, LessonSmallDto>();

            CreateMap<GroupAddRequest, Group>()
            .ForMember(d => d.StudentGroups, o => o.MapFrom(s =>
                s.StudentIds.Select(id => new StudentGroup { StudentId = id }).ToList()))
            .ForMember(d => d.LessonGroups, o => o.MapFrom(s =>
                s.LessonIds.Select(id => new LessonGroup { LessonId = id }).ToList()));

            CreateMap<GroupEditRequest, Group>();
        }
    }
}
