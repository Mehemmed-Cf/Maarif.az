using Application.Modules.StudentsModule.Commands.StudentAddCommand;
using Application.Modules.StudentsModule.Commands.StudentEditCommand;
using Application.Modules.StudentsModule.Queries.StudentGetAllQuery;
using Application.Modules.StudentsModule.Queries.StudentGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentAddRequest, Student>();
            CreateMap<StudentEditRequest, Student>();

            CreateMap<Student, StudentAddResponseDto>();
            CreateMap<Student, StudentEditResponseDto>();

            CreateMap<Student, StudentGetAllResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<Student, StudentGetByIdResponseDto>()
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Groups,
                    opt => opt.MapFrom(src => src.StudentGroups));

            // Map from the Request (Command) to the Response (DTO) 
            // for when validation fails and we need to return to the View
            CreateMap<StudentEditRequest, StudentGetByIdResponseDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new StudentDepartmentDto { Id = src.DepartmentId }));

            CreateMap<Department, StudentDepartmentDto>()
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<StudentGroup, StudentGroupDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Group.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.Year,
                    opt => opt.MapFrom(src => src.Group.Year));
        }
    }
}