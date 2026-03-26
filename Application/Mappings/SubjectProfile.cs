using Application.Modules.SubjectsModule.Commands.SubjectAddCommand;
using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery;
using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            // Add / Edit responses.
            // The handler reloads the entity with GetByIdWithDetailsAsync after
            // saving, so Department navigation is populated and DepartmentName maps cleanly.
            CreateMap<Subject, SubjectAddResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Subject, SubjectEditResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            // GetAll — ProjectTo translates all paths to SQL.
            // Department.Name, Department.Faculty.Name, and Lessons.Count
            // all become SQL expressions — no collection loading in memory.
            CreateMap<Subject, SubjectGetAllResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Department.Faculty.Name))
                .ForMember(dest => dest.LessonCount,
                    opt => opt.MapFrom(src => src.Lessons.Count));

            // GetById — entity loaded with full navigations by GetByIdWithDetailsAsync.
            CreateMap<Subject, SubjectGetByIdResponseDto>()
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Lessons,
                    opt => opt.MapFrom(src => src.Lessons));

            // Nested DTOs
            CreateMap<Department, SubjectDepartmentDto>()
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Faculty.Name));

            // Lesson → SubjectLessonDto: unwrap the Teacher navigation.
            CreateMap<Lesson, SubjectLessonDto>()
                .ForMember(dest => dest.TeacherFullName,
                    opt => opt.MapFrom(src => src.Teacher.FullName));
        }
    }
}