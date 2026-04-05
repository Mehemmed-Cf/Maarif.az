using Domain.Models.Stables;

namespace Infrastructure.Abstracts
{
    public interface IWeekCalculatorService
    {
        WeekType GetCurrentWeekType();
    }
}
