using Application.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetAllQuery
{
    public class LessonScheduleGetAllRequestHandler : IRequestHandler<LessonScheduleGetAllRequest, IEnumerable<LessonScheduleGetAllResponseDto>>
    {
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly IMapper mapper;

        public LessonScheduleGetAllRequestHandler(ILessonScheduleRepository lessonScheduleRepository, IMapper mapper)
        {
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LessonScheduleGetAllResponseDto>> Handle(LessonScheduleGetAllRequest request, CancellationToken cancellationToken)
        {
            var schedules = await lessonScheduleRepository.GetAllWithIncludesAsync(cancellationToken);
            return mapper.Map<IEnumerable<LessonScheduleGetAllResponseDto>>(schedules);

            //var schedules = await lessonScheduleRepository.GetAllWithIncludesAsync(cancellationToken);

            //return schedules.Select(x => new LessonScheduleGetAllResponseDto
            //{
            //    Id = x.Id,
            //    LessonId = x.LessonId,
            //    SubjectName = x.Lesson?.Subject?.Name,
            //    TeacherFullName = x.Lesson?.Teacher?.FullName,
            //    GroupId = x.GroupId,
            //    GroupName = x.Group?.Name,
            //    DayOfWeek = x.DayOfWeek,
            //    StartTime = x.StartTime,
            //    EndTime = x.EndTime,
            //    RoomId = x.RoomId,
            //    RoomDisplay = x.Room != null ? $"{x.Room.Building?.Id}-{x.Room.Number}" : "N/A",
            //    LessonType = x.LessonType,
            //    WeekType = x.WeekType
            //}).ToList();
        }
    }
}