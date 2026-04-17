using Application.Modules.SubjectsModule;
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
            CreateMap<Subject, SubjectAddResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Subject, SubjectEditResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Subject, SubjectGetAllResponseDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Department.Faculty.Name))
                .ForMember(dest => dest.LessonCount,
                    opt => opt.MapFrom(src => src.Lessons != null ? src.Lessons.Count : 0));

            CreateMap<SubjectTopic, SubjectTopicRowDto>();
            CreateMap<SubjectMaterial, SubjectMaterialRowDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<SubjectLiterature, SubjectLiteratureRowDto>();

            CreateMap<Subject, SubjectGetByIdResponseDto>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Lessons,
                    opt => opt.MapFrom(src => src.Lessons ?? Enumerable.Empty<Lesson>()))
                .ForMember(dest => dest.Topics,
                    opt => opt.MapFrom(src => src.Topics != null
                        ? src.Topics.OrderBy(t => t.WeekNumber).ThenBy(t => t.Id)
                        : Enumerable.Empty<SubjectTopic>()))
                .ForMember(dest => dest.Materials,
                    opt => opt.MapFrom(src => src.Materials != null
                        ? src.Materials.OrderBy(m => m.Id)
                        : Enumerable.Empty<SubjectMaterial>()))
                .ForMember(dest => dest.Literatures,
                    opt => opt.MapFrom(src => src.Literatures != null
                        ? src.Literatures.OrderBy(l => l.Id)
                        : Enumerable.Empty<SubjectLiterature>()));

            CreateMap<Subject, SubjectEditRequest>()
                .ForMember(dest => dest.Topics,
                    opt => opt.MapFrom(src => (src.Topics ?? Enumerable.Empty<SubjectTopic>())
                        .OrderBy(t => t.WeekNumber).ThenBy(t => t.Id)))
                .ForMember(dest => dest.Materials,
                    opt => opt.MapFrom(src => (src.Materials ?? Enumerable.Empty<SubjectMaterial>())
                        .OrderBy(m => m.Id)))
                .ForMember(dest => dest.Literatures,
                    opt => opt.MapFrom(src => (src.Literatures ?? Enumerable.Empty<SubjectLiterature>())
                        .OrderBy(l => l.Id)));

            CreateMap<Department, SubjectDepartmentDto>()
                .ForMember(dest => dest.FacultyName,
                    opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Lesson, SubjectLessonDto>()
                .ForMember(dest => dest.TeacherFullName,
                    opt => opt.MapFrom(src => src.Teacher.FullName));
        }
    }
}
