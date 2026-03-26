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
            // Add / Edit command responses — flat maps, no navigation needed
            // because the entity was just created/updated and navigations aren't loaded.
            CreateMap<Student, StudentAddResponseDto>();
            CreateMap<Student, StudentEditResponseDto>();

            // GetAll — ProjectTo translates these paths to SQL JOINs.
            // Department and Faculty must be included in GetAll SQL via ProjectTo;
            // no explicit Include() needed in the repository for this query.
            CreateMap<Student, StudentGetAllResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Department.Faculty.Name));

            // GetById — entity is loaded with navigations by GetByIdWithDetailsAsync.
            CreateMap<Student, StudentGetByIdResponseDto>()
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Groups,
                    opt => opt.MapFrom(src => src.StudentGroups));

            // Nested DTOs
            CreateMap<Department, StudentDepartmentDto>()
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Faculty.Name));

            // StudentGroup junction → GroupDto: unwrap the Group navigation.
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