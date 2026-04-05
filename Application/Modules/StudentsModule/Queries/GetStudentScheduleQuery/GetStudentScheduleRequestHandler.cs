using Application.Mappings;
using Application.Modules.LessonSchedulesModule.Queries.GetScheduleByGroup;
using Application.Repositories;
using Domain.Models.Stables;
using Infrastructure.Abstracts;
using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentScheduleQuery
{
    public class GetStudentScheduleRequestHandler
        : IRequestHandler<GetStudentScheduleRequest, StudentScheduleResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ILessonScheduleRepository scheduleRepository;
        private readonly IWeekCalculatorService weekCalculator;

        public GetStudentScheduleRequestHandler(
            IStudentRepository studentRepository,
            ILessonScheduleRepository scheduleRepository,
            IWeekCalculatorService weekCalculator)
        {
            this.studentRepository = studentRepository;
            this.scheduleRepository = scheduleRepository;
            this.weekCalculator = weekCalculator;
        }

        public async Task<StudentScheduleResponseDto> Handle(
            GetStudentScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var currentWeek = weekCalculator.GetCurrentWeekType();
            var student = await studentRepository.GetByUserIdWithDetailsAsync(
                request.UserId,
                cancellationToken);

            if (student?.StudentGroups is null || student.StudentGroups.Count == 0)
            {
                return new StudentScheduleResponseDto
                {
                    CurrentWeekType = currentWeek,
                    Days = new List<ScheduleDayDto>()
                };
            }

            var groupIds = student.StudentGroups.Select(sg => sg.GroupId).Distinct().ToList();
            var combined = new List<Domain.Models.Entities.LessonSchedule>();

            foreach (var gid in groupIds)
            {
                var rows = await scheduleRepository.GetByGroupAsync(
                    gid,
                    filter: null,
                    cancellationToken);
                combined.AddRange(rows);
            }

            combined = combined
                .GroupBy(s => s.Id)
                .Select(g => g.First())
                .ToList();

            var days = combined
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

            return new StudentScheduleResponseDto
            {
                CurrentWeekType = currentWeek,
                Days = days
            };
        }
    }
}
