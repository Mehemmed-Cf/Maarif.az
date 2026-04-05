using Domain.Models.ValueObjects;

namespace Application.Services
{
    public interface IGovernmentIdentityService
    {
        Task<FinData?> VerifyAsync(
            string serialNumber,
            string finCode,
            CancellationToken ct = default);

        Task<TeacherGovernmentData?> VerifyTeacherAsync(
            string serialNumber,
            string finCode,
            CancellationToken ct = default);
    }
}
