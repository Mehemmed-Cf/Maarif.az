using Domain.Models.Stables;
using Infrastructure.Abstracts;

namespace Application.Services
{
    public class WeekCalculatorService : IWeekCalculatorService
    {
        // Academic year starts first Monday of September
        // Week 1 = Upper, Week 2 = Lower, alternating
        public WeekType GetCurrentWeekType()
        {
            var today = DateTime.Today;
            var academicStart = GetAcademicYearStart(today);
            var weekNumber = (int)((today - academicStart).TotalDays / 7) + 1;
            return weekNumber % 2 == 1 ? WeekType.Upper : WeekType.Lower;
        }

        private static DateTime GetAcademicYearStart(DateTime today)
        {
            // Academic year starts in September
            var year = today.Month >= 9 ? today.Year : today.Year - 1;
            var september1 = new DateTime(year, 9, 1);

            // Find first Monday of September
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)september1.DayOfWeek + 7) % 7;
            return september1.AddDays(daysUntilMonday);
        }
    }
}
