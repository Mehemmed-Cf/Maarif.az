using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
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
            .ForMember(dest => dest.SubjectCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.GroupCount, opt => opt.MapFrom(src => src.Groups.Count));

            CreateMap<Department, DepartmentGetByIdResponseDto>()
            .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.Faculty))
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups))
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects))
            .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherDepartments));

            // Nested mappings
            CreateMap<Faculty, DepartmentFacultyDto>();

            CreateMap<Student, DepartmentStudentDto>();

            CreateMap<Group, DepartmentGroupDto>();

            CreateMap<Subject, DepartmentSubjectDto>();

            //CreateMap<Teacher, DepartmentTeacherDto>();
            CreateMap<TeacherDepartment, DepartmentTeacherDto>();
        }
    }
}
