using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;
using System.Drawing;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentAddResponseDto>()
            .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Department, DepartmentEditResponseDto>()
            .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Department, DepartmentGetAllResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name))
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.GroupCount, opt => opt.MapFrom(src => src.Groups.Count));

            CreateMap<Department, DepartmentGetByIdResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name))
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.GroupCount, opt => opt.MapFrom(src => src.Groups.Count))
            .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherDepartments.Select(td => td.Teacher.FullName).ToList()))
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects.Select(s => s.Name).ToList()));
        }
    }
}
