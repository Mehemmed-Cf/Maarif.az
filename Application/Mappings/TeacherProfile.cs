using Application.Modules.TeachersModule;
using Application.Modules.TeachersModule.Commands.TeacherAddCommand;
using Application.Modules.TeachersModule.Commands.TeacherEditCommand;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherResponseDto>()
                        .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.TeacherDepartments.Select(td => td.Department)));


            // This mapping is the "filter"
            // It ensures only Id and Name are pulled from the Department entity
            CreateMap<Department, TeacherDepartmentDto>();
            CreateMap<TeacherAddRequest, Teacher>();
            CreateMap<TeacherEditRequest, Teacher>();
        }
    }
}
