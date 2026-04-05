using Application.Identity;
using Application.Repositories;
using Application.Services;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.TeachersModule.Commands.ForgotTeacherPasswordCommand
{
    public class ForgotTeacherPasswordRequestHandler : IRequestHandler<ForgotTeacherPasswordRequest, Unit>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IGovernmentIdentityService governmentIdentityService;
        private readonly UserManager<AppUser> userManager;

        public ForgotTeacherPasswordRequestHandler(
            ITeacherRepository teacherRepository,
            IGovernmentIdentityService governmentIdentityService,
            UserManager<AppUser> userManager)
        {
            this.teacherRepository = teacherRepository;
            this.governmentIdentityService = governmentIdentityService;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(ForgotTeacherPasswordRequest request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                throw new BadRequestException("Yeni şifrə və təsdiqi eyni deyil.");

            var serialNorm = IdentityDocumentSerialNormalizer.Normalize(request.SerialNumber);
            var finNorm = IdentityDocumentSerialNormalizer.Normalize(request.FinCode);
            if (string.IsNullOrEmpty(serialNorm) || string.IsNullOrEmpty(finNorm))
                throw new BadRequestException("FIN kod və sənəd seriyası tələb olunur.");

            var verified = await governmentIdentityService.VerifyTeacherAsync(
                request.SerialNumber,
                request.FinCode,
                cancellationToken);

            if (verified is null)
                throw new BadRequestException("FIN kod və ya sənədin seriya nömrəsi yanlışdır.");

            var teacher = await teacherRepository.GetByFinCodeAsync(finNorm, cancellationToken);
            if (teacher is null)
                throw new NotFoundException("Bu məlumatlarla qeydiyyat tapılmadı.");

            IdentityRecoveryGuard.EnsureSerialMatchesStoredOrUnset(teacher.DocumentSerialNumber, serialNorm);

            var user = await userManager.FindByIdAsync(teacher.UserId.ToString());
            if (user is null)
                throw new NotFoundException("İstifadəçi tapılmadı.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var reset = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!reset.Succeeded)
                throw new BadRequestException(string.Join(" ", reset.Errors.Select(e => e.Description)));

            return Unit.Value;
        }
    }
}
