using Application.Modules.LessonsModule;
using Application.Modules.LessonsModule.Commands.LessonAddCommand;
using Application.Modules.LessonsModule.Commands.LessonEditCommand;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonResponseDto>()
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Teacher.FullName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.GroupCount, opt => opt.MapFrom(src => src.LessonGroups.Count));

            // 2. Entity to Detailed DTO (Used in GetById)
            // Note: We map the Groups by reaching through LessonGroups
            CreateMap<Lesson, LessonDetailResponseDto>()
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Teacher.FullName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.LessonGroups.Select(lg => lg.Group)));

            // 3. The Nested Group Mapping
            CreateMap<Group, LessonResponseDto>();

            // 4. Commands to Entity (For Create/Update)
            CreateMap<LessonAddRequest, Lesson>();
            CreateMap<LessonEditRequest, Lesson>();
        }
    }
}
