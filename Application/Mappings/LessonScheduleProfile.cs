using Application.Modules.LessonSchedulesModule;
using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleAddCommand;
using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleEditCommand;
using Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetAllQuery;
using Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class LessonScheduleProfile : Profile
    {
        public LessonScheduleProfile()
        {
            // Commands → Entity
            CreateMap<LessonScheduleAddRequest, LessonSchedule>();
            CreateMap<LessonScheduleEditRequest, LessonSchedule>();

            // Entity → Response DTOs (flat fields only — computed fields like RoomDisplay are ignored)
            CreateMap<LessonSchedule, LessonScheduleResponseDto>();
            CreateMap<LessonSchedule, LessonScheduleGetAllResponseDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Lesson.Subject.Name))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Lesson.Teacher.FullName))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.RoomDisplay, opt => opt.MapFrom(src =>
                    src.Room != null ? $"{src.Room.Building.Id}-{src.Room.Number}" : "N/A"));

            CreateMap<LessonSchedule, LessonScheduleGetByIdResponseDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Lesson.Subject.Name))
                .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Lesson.Teacher.FullName))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.RoomDisplay, opt => opt.MapFrom(src =>
                    src.Room != null ? $"{src.Room.Building.Id}-{src.Room.Number}" : "N/A"));
        }
    }
}