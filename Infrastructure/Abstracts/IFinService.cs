using Domain.Models.ValueObjects;

namespace Infrastructure.Abstracts
{
    public interface IFinService
    {
        Task<FinData> GetByFinCodeAsync(string finCode);
    }
}
