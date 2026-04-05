using Application.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Commands.LessonScheduleEditCommand
{
    public class LessonScheduleEditRequestHandler : IRequestHandler<LessonScheduleEditRequest, LessonScheduleResponseDto>
    {
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly IMapper mapper;

        public LessonScheduleEditRequestHandler(ILessonScheduleRepository lessonScheduleRepository, IMapper mapper)
        {
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.mapper = mapper;
        }

        public async Task<LessonScheduleResponseDto> Handle(LessonScheduleEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await lessonScheduleRepository.GetAsync(
                x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            if (entity == null)
                throw new NotFoundException("LessonSchedule not found.");

            mapper.Map(request, entity);  // maps request fields onto existing entity

            await lessonScheduleRepository.EditAsync(entity);
            await lessonScheduleRepository.SaveAsync(cancellationToken);
            return mapper.Map<LessonScheduleResponseDto>(entity);

            //var entity = await lessonScheduleRepository.GetAsync(x => x.Id == request.Id && x.DeletedAt == null, cancellationToken);

            //if (entity == null)
            //{
            //    throw new NotFoundException("LessonSchedule not found.");
            //}

            //entity.LessonId = request.LessonId;
            //entity.GroupId = request.GroupId;
            //entity.DayOfWeek = request.DayOfWeek;
            //entity.StartTime = request.StartTime;
            //entity.EndTime = request.EndTime;
            //entity.RoomId = request.RoomId;
            //entity.LessonType = request.LessonType;
            //entity.WeekType = request.WeekType;
            //entity.RoomId = request.RoomId;

            //await lessonScheduleRepository.EditAsync(entity);
            //await lessonScheduleRepository.SaveAsync(cancellationToken);

            //return new LessonScheduleResponseDto
            //{
            //    Id = entity.Id,
            //    LessonId = entity.LessonId,
            //    GroupId = entity.GroupId,
            //    DayOfWeek = entity.DayOfWeek,
            //    StartTime = entity.StartTime,
            //    EndTime = entity.EndTime,
            //    RoomId = entity.RoomId,
            //    LessonType = entity.LessonType,
            //    WeekType = entity.WeekType,
            //};
        }
    }
}