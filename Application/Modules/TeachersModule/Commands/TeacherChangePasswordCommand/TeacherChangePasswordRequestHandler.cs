using Application.Repositories;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.TeachersModule.Commands.TeacherChangePasswordCommand
{
    public class TeacherChangePasswordRequestHandler : IRequestHandler<TeacherChangePasswordRequest, Unit>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly UserManager<AppUser> userManager;

        public TeacherChangePasswordRequestHandler(
            ITeacherRepository teacherRepository,
            UserManager<AppUser> userManager)
        {
            this.teacherRepository = teacherRepository;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(TeacherChangePasswordRequest request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                throw new BadRequestException("Yeni şifrə və təsdiqi eyni deyil.");

            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken);
            if (teacher is null)
                throw new NotFoundException("Müəllim hesabı tapılmadı.");

            var user = await userManager.FindByIdAsync(teacher.UserId.ToString());
            if (user is null)
                throw new NotFoundException("İstifadəçi tapılmadı.");

            var result = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));

            return Unit.Value;
        }
    }
}
