using Application.Identity;
using Application.Repositories;
using Application.Services;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.TeachersModule.Commands.RecoverTeacherNumberCommand
{
    public class RecoverTeacherNumberRequestHandler
        : IRequestHandler<RecoverTeacherNumberRequest, RecoverTeacherNumberResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IGovernmentIdentityService governmentIdentityService;

        public RecoverTeacherNumberRequestHandler(
            ITeacherRepository teacherRepository,
            IGovernmentIdentityService governmentIdentityService)
        {
            this.teacherRepository = teacherRepository;
            this.governmentIdentityService = governmentIdentityService;
        }

        public async Task<RecoverTeacherNumberResponseDto> Handle(
            RecoverTeacherNumberRequest request,
            CancellationToken cancellationToken)
        {
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

            var number = teacher.TeacherNumber ?? "";
            if (string.IsNullOrWhiteSpace(number))
                throw new NotFoundException("Müəllim nömrəsi təyin edilməyib.");

            return new RecoverTeacherNumberResponseDto { TeacherNumber = number };
        }
    }
}
