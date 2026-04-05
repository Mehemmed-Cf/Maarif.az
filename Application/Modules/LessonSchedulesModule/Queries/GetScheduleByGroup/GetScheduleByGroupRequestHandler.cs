using Application.Mappings;
using Application.Repositories;
using Domain.Models.Stables;
using Infrastructure.Abstracts;
using MediatR;

namespace Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup
{
    public class GetScheduleByGroupQueryHandler
        : IRequestHandler<GetScheduleByGroupRequest, GetScheduleByGroupResponseDto>
    {
        private readonly ILessonScheduleRepository scheduleRepository;
        private readonly IWeekCalculatorService weekCalculator;

        public GetScheduleByGroupQueryHandler(
            ILessonScheduleRepository scheduleRepository,
            IWeekCalculatorService weekCalculator)
        {
            this.scheduleRepository = scheduleRepository;
            this.weekCalculator = weekCalculator;
        }

        public async Task<GetScheduleByGroupResponseDto> Handle(
            GetScheduleByGroupRequest request,
            CancellationToken cancellationToken)
        {
            var currentWeek = weekCalculator.GetCurrentWeekType();
            var filter = request.WeekType ?? WeekType.Both;

            var schedules = await scheduleRepository
                .GetByGroupAsync(request.GroupId, filter, cancellationToken);

            var days = schedules
                .GroupBy(s => s.DayOfWeek)
                .OrderBy(g => ((int)g.Key + 6) % 7)
                .Select(g => new ScheduleDayDto
                {
                    DayOfWeek = g.Key,
                    Lessons = g.Select(LessonScheduleDtoMapping.ToDto).ToList()
                }).ToList();

            return new GetScheduleByGroupResponseDto
            {
                CurrentWeekType = currentWeek,
                Days = days
            };
        }
    }
}
