using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetByIdQuery
{
    public class LessonScheduleGetByIdRequestHandler : IRequestHandler<LessonScheduleGetByIdRequest, LessonScheduleGetByIdResponseDto>
    {
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly IMapper mapper;

        public LessonScheduleGetByIdRequestHandler(ILessonScheduleRepository lessonScheduleRepository, IMapper mapper)
        {
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.mapper = mapper;
        }

        public async Task<LessonScheduleGetByIdResponseDto> Handle(LessonScheduleGetByIdRequest request, CancellationToken cancellationToken)
        {

            var schedule = await lessonScheduleRepository
                .GetByIdWithIncludesAsync(request.Id, cancellationToken);

            if (schedule == null)
                throw new NotFoundException("LessonSchedule not found.");

            return mapper.Map<LessonScheduleGetByIdResponseDto>(schedule);

            //var schedule = await lessonScheduleRepository.GetByIdWithIncludesAsync(request.Id, cancellationToken);
            //if (schedule == null) throw new NotFoundException("LessonSchedule not found");

            //return new LessonScheduleGetByIdResponseDto
            //{
            //    Id = schedule.Id,
            //    LessonId = schedule.LessonId,
            //    SubjectName = schedule.Lesson?.Subject?.Name,
            //    TeacherFullName = schedule.Lesson?.Teacher?.FullName,
            //    GroupId = schedule.GroupId,
            //    GroupName = schedule.Group?.Name,
            //    DayOfWeek = schedule.DayOfWeek,
            //    StartTime = schedule.StartTime,
            //    EndTime = schedule.EndTime,
            //    RoomId = schedule.RoomId,
            //    RoomDisplay = schedule.Room != null ? $"{schedule.Room.Building?.Id}-{schedule.Room.Number}" : "N/A",
            //    LessonType = schedule.LessonType,
            //    WeekType = schedule.WeekType
            //};
        }
    }
}