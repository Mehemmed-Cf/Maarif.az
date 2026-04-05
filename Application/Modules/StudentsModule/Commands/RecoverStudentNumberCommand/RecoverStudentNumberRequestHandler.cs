using Application.Identity;
using Application.Repositories;
using Application.Services;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.RecoverStudentNumberCommand
{
    public class RecoverStudentNumberRequestHandler
        : IRequestHandler<RecoverStudentNumberRequest, RecoverStudentNumberResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IGovernmentIdentityService governmentIdentityService;

        public RecoverStudentNumberRequestHandler(
            IStudentRepository studentRepository,
            IGovernmentIdentityService governmentIdentityService)
        {
            this.studentRepository = studentRepository;
            this.governmentIdentityService = governmentIdentityService;
        }

        public async Task<RecoverStudentNumberResponseDto> Handle(
            RecoverStudentNumberRequest request,
            CancellationToken cancellationToken)
        {
            var serialNorm = IdentityDocumentSerialNormalizer.Normalize(request.SerialNumber);
            var finNorm = IdentityDocumentSerialNormalizer.Normalize(request.FinCode);
            if (string.IsNullOrEmpty(serialNorm) || string.IsNullOrEmpty(finNorm))
                throw new BadRequestException("FIN kod və sənəd seriyası tələb olunur.");

            var verified = await governmentIdentityService.VerifyAsync(
                request.SerialNumber,
                request.FinCode,
                cancellationToken);

            if (verified is null)
                throw new BadRequestException("FIN kod və ya sənədin seriya nömrəsi yanlışdır.");

            var student = await studentRepository.GetByFinCodeAsync(finNorm, cancellationToken);
            if (student is null)
                throw new NotFoundException("Bu məlumatlarla qeydiyyat tapılmadı.");

            IdentityRecoveryGuard.EnsureSerialMatchesStoredOrUnset(student.DocumentSerialNumber, serialNorm);

            return new RecoverStudentNumberResponseDto { StudentNumber = student.StudentNumber };
        }
    }
}
