using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleAddCommand
{
    public class LessonScheduleAddRequestHandler : IRequestHandler<LessonScheduleAddRequest, LessonScheduleResponseDto>
    {
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly IMapper mapper;

        public LessonScheduleAddRequestHandler(ILessonScheduleRepository lessonScheduleRepository, IMapper mapper)
        {
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.mapper = mapper;
        }

        public async Task<LessonScheduleResponseDto> Handle(LessonScheduleAddRequest request, CancellationToken cancellationToken)
        {

            var entity = mapper.Map<LessonSchedule>(request);
            await lessonScheduleRepository.AddAsync(entity, cancellationToken);
            await lessonScheduleRepository.SaveAsync(cancellationToken);
            return mapper.Map<LessonScheduleResponseDto>(entity);

            //var entity = new LessonSchedule
            //{
            //    LessonId = request.LessonId,
            //    GroupId = request.GroupId,
            //    DayOfWeek = request.DayOfWeek,
            //    StartTime = request.StartTime,
            //    EndTime = request.EndTime,
            //    LessonType = request.LessonType,
            //    WeekType = request.WeekType,
            //    RoomId = request.RoomId
            //};

            //await lessonScheduleRepository.AddAsync(entity, cancellationToken);
            //await lessonScheduleRepository.SaveAsync(cancellationToken);

            //return new LessonScheduleResponseDto
            //{
            //    Id = entity.Id,
            //    LessonId = entity.LessonId,
            //    GroupId = entity.GroupId,
            //    DayOfWeek = entity.DayOfWeek,
            //    StartTime = entity.StartTime,
            //    EndTime = entity.EndTime,
            //    LessonType = entity.LessonType,
            //    WeekType = entity.WeekType,
            //    RoomId = entity.RoomId
            //};
        }
    }
}