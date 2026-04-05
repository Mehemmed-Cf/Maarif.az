using Application.Mappings;
using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Application.Repositories;
using Domain.Models.Stables;
using Infrastructure.Abstracts;
using MediatR;

namespace Application.Modules.TeachersModule.Queries.GetTeacherScheduleQuery
{
    public class GetTeacherScheduleRequestHandler
        : IRequestHandler<GetTeacherScheduleRequest, TeacherScheduleResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly ILessonScheduleRepository scheduleRepository;
        private readonly IWeekCalculatorService weekCalculator;

        public GetTeacherScheduleRequestHandler(
            ITeacherRepository teacherRepository,
            ILessonScheduleRepository scheduleRepository,
            IWeekCalculatorService weekCalculator)
        {
            this.teacherRepository = teacherRepository;
            this.scheduleRepository = scheduleRepository;
            this.weekCalculator = weekCalculator;
        }

        public async Task<TeacherScheduleResponseDto> Handle(
            GetTeacherScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var currentWeek = weekCalculator.GetCurrentWeekType();
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(
                request.UserId,
                cancellationToken);

            if (teacher is null)
            {
                return new TeacherScheduleResponseDto
                {
                    CurrentWeekType = currentWeek,
                    Days = new List<ScheduleDayDto>()
                };
            }

            var schedules = await scheduleRepository.GetByTeacherAsync(
                teacher.Id,
                filter: null,
                cancellationToken);

            var days = schedules
                .GroupBy(s => s.DayOfWeek)
                .OrderBy(g => ((int)g.Key + 6) % 7)
                .Select(g => new ScheduleDayDto
                {
                    DayOfWeek = g.Key,
                    Lessons = g
                        .OrderBy(s => s.StartTime)
                        .Select(LessonScheduleDtoMapping.ToDto)
                        .ToList()
                })
                .ToList();

            return new TeacherScheduleResponseDto
            {
                CurrentWeekType = currentWeek,
                Days = days
            };
        }
    }
}
